#region Disclaimer / License
// Copyright (C) 2011, Xervbackup
// http://www.hexad.dk, opensource@hexad.dk
// 
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
// 
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
// 
#endregion
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Data.LightDatamodel;
using System.Drawing;
using XervBackup.Datamodel;

namespace XervBackup.GUI
{
    static class Program
    {
        /// <summary>
        /// The name of the environment variable that holds the path to the data folder used by XervBackup
        /// </summary>
        public const string DATAFOLDER_ENV_NAME = "XERVBACKUP_HOME";

        /// <summary>
        /// The environment variable that holdes the database key used to encrypt the SQLite database
        /// </summary>
        public const string DB_KEY_ENV_NAME = "XERVBACKUP_DB_KEY";

        /// <summary>
        /// Gets the folder where XervBackup data is stored
        /// </summary>
        public static string DATAFOLDER { get { return Library.Utility.Utility.AppendDirSeparator(Environment.ExpandEnvironmentVariables("%" + DATAFOLDER_ENV_NAME + "%").TrimStart('"').TrimEnd('"')); } }

        /// <summary>
        /// A flag indicating if database encryption is in use
        /// </summary>
        public static bool UseDatabaseEncryption;

        /// <summary>
        /// This is the only access to the database
        /// </summary>
        public static IDataFetcherWithRelations DataConnection;

        /// <summary>
        /// This is the lock to be used before manipulating the shared resources
        /// </summary>
        public static object MainLock = new object();

        /// <summary>
        /// This is the scheduling thread
        /// </summary>
        public static Scheduler Scheduler;
        
        /// <summary>
        /// This is the working thread
        /// </summary>
        public static WorkerThread<IDuplicityTask> WorkThread;

        /// <summary>
        /// The path to the file that contains the current database
        /// </summary>
        public static string DatabasePath;

        /// <summary>
        /// The actual runner, do not call directly. Only used for events.
        /// </summary>
        public static XervBackupRunner Runner;

        /// <summary>
        /// The controller interface for pause/resume and throttle options
        /// </summary>
        public static LiveControls LiveControl;

        /// <summary>
        /// The main form that contains the tray icon
        /// </summary>
        public static MainForm DisplayHelper;

        /// <summary>
        /// The single instance keeper
        /// </summary>
        public static SingleInstance SingleInstance;

        /// <summary>
        /// A value describing if XervBackup is running without a tray
        /// </summary>
        public static bool TraylessMode = Library.Utility.Utility.IsClientLinux ? true : false;
		
		/// <summary>
		/// A flag indicating if the main message loop is still running,
		/// used to force exit when running on OSX
		/// </summary>
        public static bool IsRunningMainLoop;
		
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //If we are on Windows, append the bundled "win-tools" programs to the search path
            //We add it last, to allow the user to override with other versions
            if (!Library.Utility.Utility.IsClientLinux)
            {
                string wintools = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "win-tools");
                Environment.SetEnvironmentVariable("PATH",
                    Environment.GetEnvironmentVariable("PATH") +
                    System.IO.Path.PathSeparator.ToString() +
                    wintools +
                    System.IO.Path.PathSeparator.ToString() +
                    System.IO.Path.Combine(wintools, "gpg") //GPG needs to be in a subfolder for wrapping reasons
                );
            }

            Library.Utility.UrlUtillity.ErrorHandler = new XervBackup.Library.Utility.UrlUtillity.ErrorHandlerDelegate(DisplayURLOpenError);

            //If we are on windows we encrypt the database by default
            //We do not encrypt on Linux as most distros use a SQLite library without encryption support,
            //Linux users can use an encrypted home folder, or install a SQLite library with encryption support
            if (!Library.Utility.Utility.IsClientLinux && string.IsNullOrEmpty(Environment.GetEnvironmentVariable(DB_KEY_ENV_NAME)))
            {
                //Note that the password here is a default password and public knowledge
                //
                //The purpose of this is to prevent casual read of the database, as well
                // as protect from harddisk string scans, not to protect from determined
                // attacks.
                //
                //If you desire better security, start XervBackup once with the commandline option
                // --unencrypted-database to decrypt the database.
                //Then set the environment variable XERVBACKUP_DB_KEY to the desired key, 
                // and run XervBackup again without the --unencrypted-database option 
                // to re-encrypt it with the new key
                //
                //If you change the key, please note that you need to supply the same
                // key when restoring the setup, as the setup being backed up will
                // be encrypted as well.
                Environment.SetEnvironmentVariable(DB_KEY_ENV_NAME, "XervBackup_Key_42");
            }


            //Find commandline options here for handling special startup cases
            Dictionary<string, string> commandlineOptions = CommandLine.CommandLineParser.ExtractOptions(new List<string>(args));

            foreach (string s in args)
                if (
                    s.Equals("help", StringComparison.InvariantCultureIgnoreCase) ||
                    s.Equals("/help", StringComparison.InvariantCultureIgnoreCase) ||
                    s.Equals("usage", StringComparison.InvariantCultureIgnoreCase) ||
                    s.Equals("/usage", StringComparison.InvariantCultureIgnoreCase))
                    commandlineOptions["help"] = "";

            //If the commandline issues --help, just stop here
            if (commandlineOptions.ContainsKey("help"))
            {
                List<string> lines = new List<string>();
                foreach (Library.Interface.ICommandLineArgument arg in SupportedCommands)
                    lines.Add(string.Format(Strings.Program.HelpDisplayFormat, arg.Name, arg.LongDescription));
                MessageBox.Show(string.Format(Strings.Program.HelpDisplayDialog, string.Join(Environment.NewLine, lines.ToArray())), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (commandlineOptions.ContainsKey("trayless"))
                Program.TraylessMode = Library.Utility.Utility.ParseBoolOption(commandlineOptions, "trayless");

#if DEBUG
            //Log various information in the logfile
            if (!commandlineOptions.ContainsKey("log-file"))
            {
                commandlineOptions["log-file"] = System.IO.Path.Combine(Application.StartupPath, "XervBackup.debug.log");
                commandlineOptions["log-level"] = XervBackup.Library.Logging.LogMessageType.Profiling.ToString();
            }
#endif

            if (commandlineOptions.ContainsKey("log-level"))
                foreach (string s in Enum.GetNames(typeof(XervBackup.Library.Logging.LogMessageType)))
                    if (s.Equals(commandlineOptions["log-level"].Trim(), StringComparison.InvariantCultureIgnoreCase))
                        XervBackup.Library.Logging.Log.LogLevel = (XervBackup.Library.Logging.LogMessageType)Enum.Parse(typeof(XervBackup.Library.Logging.LogMessageType), s);

            if (commandlineOptions.ContainsKey("log-file"))
            {
                if (System.IO.File.Exists(commandlineOptions["log-file"]))
                    System.IO.File.Delete(commandlineOptions["log-file"]);
                XervBackup.Library.Logging.Log.CurrentLog = new XervBackup.Library.Logging.StreamLog(commandlineOptions["log-file"]);
            }

            //Set the %XERVBACKUP_HOME% env variable, if it is not already set
            if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable(DATAFOLDER_ENV_NAME)))
            {
#if DEBUG
                //debug mode uses a lock file located in the app folder
                Environment.SetEnvironmentVariable(DATAFOLDER_ENV_NAME, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));
#else
                bool portableMode = Library.Utility.Utility.ParseBoolOption(commandlineOptions, "portable-mode");

                if (portableMode)
                {
                    //Portable mode uses a data folder in the application home dir
                    Environment.SetEnvironmentVariable(DATAFOLDER_ENV_NAME, System.IO.Path.Combine(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "data"));
                }
                else
                {
                    //Normal release mode uses the systems "Application Data" folder
                    Environment.SetEnvironmentVariable(DATAFOLDER_ENV_NAME, System.IO.Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), Application.ProductName));
                }
#endif
            }

            try
            {
                try
                {
                    //This will also create Program.DATAFOLDER if it does not exist
                    SingleInstance = new SingleInstance(Application.ProductName, Program.DATAFOLDER);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(string.Format(Strings.Program.StartupFailure, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!SingleInstance.IsFirstInstance)
                {
                    //Linux shows this output
                    Console.WriteLine(Strings.Program.AnotherInstanceDetected);
                    return;
                }

                Version sqliteVersion = new Version((string)SQLiteLoader.SQLiteConnectionType.GetProperty("SQLiteVersion").GetValue(null, null));
                if (sqliteVersion < new Version(3, 6, 3))
                {
                    //The official Mono SQLite provider is also broken with less than 3.6.3
                    MessageBox.Show(string.Format(Strings.Program.WrongSQLiteVersion, sqliteVersion, "3.6.3"), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //Create the connection instance
                System.Data.IDbConnection con = (System.Data.IDbConnection)Activator.CreateInstance(SQLiteLoader.SQLiteConnectionType);

                try
                {
#if DEBUG
                    //Default is to not use encryption for debugging
                    Program.UseDatabaseEncryption = commandlineOptions.ContainsKey("unencrypted-database") ? !Library.Utility.Utility.ParseBoolOption(commandlineOptions, "unencrypted-database") : false;
#else
                    //Default is to use encryption for release
                    Program.UseDatabaseEncryption = !Library.Utility.Utility.ParseBoolOption(commandlineOptions, "unencrypted-database");
#endif

                    OpenSettingsDatabase(con, Program.DATAFOLDER);
                }
                catch (Exception ex)
                {
                    //Unwrap the reflection exceptions
                    if (ex is System.Reflection.TargetInvocationException && ex.InnerException != null)
                        ex = ex.InnerException;

                    MessageBox.Show(string.Format(Strings.Program.DatabaseOpenError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                DataConnection = new DataFetcherWithRelations(new SQLiteDataProvider(con));

                string displayLanguage = new Datamodel.ApplicationSettings(DataConnection).DisplayLanguage;
                if (!string.IsNullOrEmpty(displayLanguage) && displayLanguage != Library.Utility.Utility.DefaultCulture.Name)
                {
                    try
                    {
                        System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.GetCultureInfo(displayLanguage);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = System.Globalization.CultureInfo.GetCultureInfo(displayLanguage);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(string.Format(Strings.Program.LanguageSelectionError, ex.Message), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //This is non-fatal, just keep running with system default language
                    }
                }

                try
                {
                    DataFetcherNested logcon = new DataFetcherNested(DataConnection);
                    Log[] items = logcon.GetObjects<Log>("Subaction LIKE ?", "InProgress");
                    if (items != null && items.Length > 0)
                    {
                        foreach (Log l in items)
                            l.SubAction = "Primary";

                        logcon.CommitAllRecursive();
                    }
                }
                catch
                {
                    //Non-fatal but any interrupted backup will not show
                }

                LiveControl = new LiveControls(new ApplicationSettings(DataConnection));
                LiveControl.StateChanged += new EventHandler(LiveControl_StateChanged);
                LiveControl.ThreadPriorityChanged += new EventHandler(LiveControl_ThreadPriorityChanged);
                LiveControl.ThrottleSpeedChanged += new EventHandler(LiveControl_ThrottleSpeedChanged);

                Runner = new XervBackupRunner();
                WorkThread = new WorkerThread<IDuplicityTask>(new WorkerThread<IDuplicityTask>.ProcessItemDelegate(Runner.ExecuteTask), LiveControl.State == LiveControls.LiveControlState.Paused);
                Scheduler = new Scheduler(DataConnection, WorkThread, MainLock);

                DataConnection.AfterDataConnection += new DataConnectionEventHandler(DataConnection_AfterDataConnection);

                DisplayHelper = new MainForm();
                DisplayHelper.InitialArguments = args;
				
				Program.IsRunningMainLoop = true;
                Application.Run(DisplayHelper);
				Program.IsRunningMainLoop = false;
            }
            catch (Exception ex)
            {
				//If the helper thread aborts the main thread, it also sets IsRunningMainLoop to false
				// and in that case we accept the abort call
				if (ex is System.Threading.ThreadAbortException && !Program.IsRunningMainLoop)
					System.Threading.Thread.ResetAbort();
				else
		        	MessageBox.Show(string.Format(Strings.Program.SeriousError, ex.ToString()), Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);					
		    }

            try
            {
                //Find logs that are no longer displayed, and delete them
                DataFetcherNested con = new DataFetcherNested(DataConnection);
                foreach (Log x in con.GetObjects<Log>("EndTime < ?", Library.Utility.Timeparser.ParseTimeInterval(new ApplicationSettings(con).RecentBackupDuration, DateTime.Now, true)))
                {
                    if (x.Blob != null) //Load the blob part if required
                        con.DeleteObject(x.Blob);
                    con.DeleteObject(x);
                }

                con.CommitAllRecursive();
            }
            catch
            { }

            try
            {
                //Compact the database
                using (System.Data.IDbCommand vaccum_cmd = DataConnection.Provider.Connection.CreateCommand())
                {
                    vaccum_cmd.CommandText = "VACUUM;";
                    vaccum_cmd.ExecuteNonQuery();
                }
            }
            catch 
            { }


            if (Scheduler != null)
                Scheduler.Terminate(true);
            if (WorkThread != null)
                WorkThread.Terminate(true);
            if (SingleInstance != null)
                SingleInstance.Dispose();

#if DEBUG
            using(XervBackup.Library.Logging.Log.CurrentLog as XervBackup.Library.Logging.StreamLog)
                XervBackup.Library.Logging.Log.CurrentLog = null;
#endif
        }
        
        /// <summary>
        /// Opens the application settings database, creating a new db if missing
        /// </summary>
        /// <param name="con">the db connection</param>
        /// <param name="datafolder">the database folder path</param>
        /// <param name="dbfile">the database file name within the data folder; defaults to "XervBackup.sqlite"</param>
        internal static void OpenSettingsDatabase(System.Data.IDbConnection con, string datafolder, string dbfile = "XervBackup.sqlite") {
            DatabasePath = System.IO.Path.Combine(datafolder, dbfile); 
            if (!System.IO.Directory.Exists(System.IO.Path.GetDirectoryName(DatabasePath)))
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(DatabasePath));

            con.ConnectionString = "Data Source=" + DatabasePath;

            //Attempt to open the database, handling any encryption present
            OpenDatabase(con);

            DatabaseUpgrader.UpgradeDatabase(con, DatabasePath);
        }

        /// <summary>
        /// Handles a change in the LiveControl and updates the Runner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LiveControl_ThreadPriorityChanged(object sender, EventArgs e)
        {
            if (LiveControl.ThreadPriority == null)
                Runner.UnsetThreadPriority();
            else
                Runner.SetThreadPriority(LiveControl.ThreadPriority.Value);
        }

        /// <summary>
        /// Handles a change in the LiveControl and updates the Runner
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void LiveControl_ThrottleSpeedChanged(object sender, EventArgs e)
        {
            if (LiveControl.DownloadLimit == null)
                Runner.SetDownloadLimit(null);
            else
                Runner.SetDownloadLimit(LiveControl.DownloadLimit.Value.ToString() + "b");

            if (LiveControl.UploadLimit == null)
                Runner.SetUploadLimit(null);
            else
                Runner.SetUploadLimit(LiveControl.UploadLimit.Value.ToString() + "b");
        }

        /// <summary>
        /// This event handler updates the trayicon menu with the current state of the runner.
        /// </summary>
        static void LiveControl_StateChanged(object sender, EventArgs e)
        {
            switch (LiveControl.State)
            {
                case LiveControls.LiveControlState.Paused:
                    WorkThread.Pause();
                    Runner.Pause();
                    break;
                case LiveControls.LiveControlState.Running:
                    WorkThread.Resume();
                    Runner.Resume();
                    break;
            }
        }


        private static void DataConnection_AfterDataConnection(object sender, DataActions action)
        {
            if (action == DataActions.Insert || action == DataActions.Update)
                Scheduler.Reschedule();
        }

        /// <summary>
        /// Returns a localized name for a task type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string LocalizeTaskType(DuplicityTaskType type)
        {
            switch (type)
            {
                case DuplicityTaskType.FullBackup:
                    return Strings.TaskType.FullBackup;
                case DuplicityTaskType.IncrementalBackup:
                    return Strings.TaskType.IncrementalBackup;
                case DuplicityTaskType.ListActualFiles:
                    return Strings.TaskType.ListActualFiles;
                case DuplicityTaskType.ListBackupEntries:
                    return Strings.TaskType.ListBackupEntries;
                case DuplicityTaskType.ListBackups:
                    return Strings.TaskType.ListBackups;
                case DuplicityTaskType.ListFiles:
                    return Strings.TaskType.ListFiles;
                case DuplicityTaskType.RemoveAllButNFull:
                    return Strings.TaskType.RemoveAllButNFull;
                case DuplicityTaskType.RemoveOlderThan:
                    return Strings.TaskType.RemoveOlderThan;
                case DuplicityTaskType.Restore:
                    return Strings.TaskType.Restore;
                case DuplicityTaskType.RestoreSetup:
                    return Strings.TaskType.RestoreSetup;
                default:
                    return type.ToString();
            }
        }

        /// <summary>
        /// Helper method with logic to handle opening a database in possibly encrypted format
        /// </summary>
        /// <param name="con">The SQLite connection object</param>
        internal static void OpenDatabase(System.Data.IDbConnection con)
        {
            bool noEncryption = !Program.UseDatabaseEncryption;
            string password = Environment.GetEnvironmentVariable(DB_KEY_ENV_NAME);

            System.Reflection.MethodInfo setPwdMethod = con.GetType().GetMethod("SetPassword", new Type[] { typeof(string) });
            string attemptedPassword;

            if (noEncryption || string.IsNullOrEmpty(password))
                attemptedPassword = null; //No encryption specified, attempt to open without
            else
                attemptedPassword = password; //Encryption specified, attempt to open with

            setPwdMethod.Invoke(con, new object[] { attemptedPassword });

            try
            {
                //Attempt to open in preferred state
                con.Open();
            }
            catch
            {
                try
                {
                    //We can't try anything else without a password
                    if (string.IsNullOrEmpty(password))
                        throw; 

                    //Open failed, now try the reverse
                    if (attemptedPassword == null)
                        attemptedPassword = password;
                    else
                        attemptedPassword = null;

                    setPwdMethod.Invoke(con, new object[] { attemptedPassword });
                    con.Open();
                }
                catch
                {
                }

                //If the db is not open now, it won't open
                if (con.State != System.Data.ConnectionState.Open)
                    throw; //Report original error

                //The open method succeeded with the non-default method, now change the password
                System.Reflection.MethodInfo changePwdMethod = con.GetType().GetMethod("ChangePassword", new Type[] { typeof(string) });
                changePwdMethod.Invoke(con, new object[] { noEncryption ? null : password });
            }
        }

        /// <summary>
        /// Callback method for dealing with url open errors
        /// </summary>
        /// <param name="message">The error message to display</param>
        private static void DisplayURLOpenError(string message)
        {
            System.Windows.Forms.MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Gets a list of all supported commandline options
        /// </summary>
        private static Library.Interface.ICommandLineArgument[] SupportedCommands
        {
            get
            {
                return new XervBackup.Library.Interface.ICommandLineArgument[] {
                    new XervBackup.Library.Interface.CommandLineArgument("help", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Boolean, Strings.Program.HelpCommandDescription, Strings.Program.HelpCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("run-backup", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.String, Strings.Program.RunbackupCommandDescription, Strings.Program.RunbackupCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("run-backup-group", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.String, Strings.Program.RunbackupGroupCommandDescription, Strings.Program.RunbackupGroupCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("full", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.String, Strings.Program.FullCommandDescription, Strings.Program.FullCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("resume", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.String, Strings.Program.ResumeCommandDescription, Strings.Program.ResumeCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("pause", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.String, Strings.Program.PauseCommandDescription, Strings.Program.PauseCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("show-status", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Boolean, Strings.Program.ShowstausCommandDescription, Strings.Program.ShowstausCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("show-wizard", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Boolean, Strings.Program.ShowwizardCommandDescription, Strings.Program.ShowwizardCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("unencrypted-database", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Boolean, Strings.Program.UnencrypteddatabaseCommandDescription, Strings.Program.UnencrypteddatabaseCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("portable-mode", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Boolean, Strings.Program.PortablemodeCommandDescription, Strings.Program.PortablemodeCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("log-file", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Path, Strings.Program.LogfileCommandDescription, Strings.Program.LogfileCommandDescription),
                    new XervBackup.Library.Interface.CommandLineArgument("log-level", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Enumeration, Strings.Program.LoglevelCommandDescription, Strings.Program.LoglevelCommandDescription, "Warning", null, Enum.GetNames(typeof(XervBackup.Library.Logging.LogMessageType))),
                    new XervBackup.Library.Interface.CommandLineArgument("trayless", XervBackup.Library.Interface.CommandLineArgument.ArgumentType.Boolean, Strings.Program.TraylessCommandDescription, Strings.Program.TraylessCommandDescription, Library.Utility.Utility.IsClientLinux ? "true" : "false" )
                };
            }
        }

    }
}