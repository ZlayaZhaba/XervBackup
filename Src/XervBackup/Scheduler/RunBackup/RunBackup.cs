#region Disclaimer / License
// Copyright (C) 2011, Kenneth Bergeron, IAP Worldwide Services, Inc
// NOAA :: National Marine Fisheries Service
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
using System.Diagnostics;
using System.Linq;
// This is a x86 exe - If you are using Express, see this:
// http://social.msdn.microsoft.com/Forums/en-US/Vsexpressvcs/thread/4650481d-b385-43f3-89c7-c07546a7f7cd
//
[assembly: System.Security.Permissions.PermissionSetAttribute(System.Security.Permissions.SecurityAction.RequestMinimum, Name = "FullTrust")]
namespace XervBackup.Scheduler.RunBackup
{
    class Program
    {
        /// <summary>
        /// The name of the mother package
        /// </summary>
        public const string Package = "XervBackup";
        /// <summary>
        /// Name of application
        /// </summary>
        public const string Name = "XervBackup.RunBackup";
        /// <summary>
        /// Root name of named pipe
        /// </summary>
        public const string PipeBaseName = "XervBackup.Pipe";
        /// <summary>
        /// Name of the Pipe client listener thread
        /// </summary>
        public const string ClientThreadName = "XervBackup.PipeClient";
        /// <summary>
        /// Where Monitor plugins live
        /// </summary>
        public static string StartupPath = 
            System.IO.Path.GetDirectoryName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);

        /// <summary>
        /// Name of this job
        /// </summary>
        public static string Job = "<none>";
        /// <summary>
        /// If set via command line argument "DryRun", no backup is actually made; used for debug
        /// </summary>
        private static bool itsDryRun = false;
        /// <summary>
        /// This is basically a very small XervBackup command line that only supports backup.
        /// This is a console app; but compiled with 'forms' to get rid of the dreaded console window in xp.
        /// </summary>
        /// <param name="aArgs">Arguments are job name and XML file name</param>
        static void Main(string[] aArgs)
        {
            // Args as a handy list
            List<string> ArgList = new List<string>(aArgs);
#if DEBUGUSER
            if (!Environment.UserInteractive) 
                System.Threading.Thread.Sleep(20000); // Give time to attach debugger
            if (!Environment.UserName.EndsWith("\\User"))
            {
                // TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST TEST 
                Utility.Su.Impersonate("User", Environment.UserDomainName, "asd");  // TEST
                Environment.SetEnvironmentVariable("TMP", "C:\\temp"); // TEST
            }
#endif
            if (ArgList.Count == 0)
            {
                System.Windows.Forms.MessageBox.Show("This program must be executed with a job name and should not be executed manually");
                return;
            }

            itsDryRun = ArgList.Contains("DryRun"); // Good for debugging
            if(itsDryRun) ArgList.Remove("DryRun");
            // The job name
            Job = ArgList[0];

            // Be nice
            System.Threading.Thread.CurrentThread.Priority = System.Threading.ThreadPriority.BelowNormal;
            // Get a log file
            string LogFile = XervBackup.Scheduler.Utility.Tools.LogFileName(Package);
            using (new Notifier())  // Attempt to put that little thingy in the tray
            // Create the log
            using (XervBackup.Library.Logging.AppendLog Log = new XervBackup.Library.Logging.AppendLog(LogFile, Job))
            {
                XervBackup.Library.Logging.Log.CurrentLog = Log;
                // Hard code level to info, deal with filters in the forms level
                XervBackup.Library.Logging.Log.LogLevel = XervBackup.Library.Logging.LogMessageType.Information;
#if !DEBUG
                try
#endif
                {
                    // Load the history XML dataset
                    using (XervBackup.Scheduler.Data.HistoryDataSet hds = new XervBackup.Scheduler.Data.HistoryDataSet())
                    {
                        hds.Load();
                        // Create a new history record for this run
                        XervBackup.Scheduler.Data.HistoryDataSet.HistoryRow HistoryRow = hds.History.AddHistoryRow(Job, DateTime.Now, "Backup", true, false, LogFile, new byte[0], string.Empty);
                        // Actually run the job
                        RunBackup(ArgList.ToArray(), HistoryRow);
                        // Just clean out history
                        foreach (XervBackup.Scheduler.Data.HistoryDataSet.HistoryRow Row in 
                            from XervBackup.Scheduler.Data.HistoryDataSet.HistoryRow qR in hds.History 
                            where !System.IO.File.Exists(qR.LogFileName) select qR)
                                Row.Delete();
                        hds.Save();     // Save the XML (later: make so the whole db need not be loaded (XMLReader)
                    }
                }
#if !DEBUG
                catch (Exception Ex)        // Log error
                {
                    Library.Logging.Log.WriteMessage(Ex.Message, XervBackup.Library.Logging.LogMessageType.Error);
                }
#endif
                UpdateMonitors(LogFile);
                // All done, set the log file (filewathers are looking for AttributeChange)
                System.IO.File.SetAttributes(LogFile, System.IO.FileAttributes.ReadOnly);
            }
        }
        /// <summary>
        /// Update any Monitor plugins
        /// </summary>
        /// <param name="aLogFile"></param>
        private static void UpdateMonitors(string aLogFile)
        {
            // Each plugin
            foreach(string Plug in System.IO.Directory.GetFiles(StartupPath, "XervBackup.Scheduler.Monitor.*.dll"))
            {
                // Load the assembly, get the type, create an instance
                System.Reflection.Assembly Ass = System.Reflection.Assembly.LoadFile(Plug);
                Type ClassType = Ass.GetTypes().Where(qR=>qR.Name == "Plugin").First();
                XervBackup.Scheduler.Data.IMonitorPlugin Monitor = Activator.CreateInstance(ClassType, null) as XervBackup.Scheduler.Data.IMonitorPlugin;
                if (Monitor != null && (DisabledMonitors == null || !DisabledMonitors.Contains(Monitor.Name)))
                {
                    // Tell big brother
                    Exception phEx = 
#if !DEBUG
                        Utility.Tools.TryCatch((Action)delegate()
#else
                        null;
#endif
                    {
                        // Update the scheduler
                        Monitor.UpdateScheduler(XervBackup.Scheduler.Data.SchedulerDataSet.DefaultPath());
                        // Update the history
                        Monitor.UpdateHistory(XervBackup.Scheduler.Data.HistoryDataSet.DefaultPath());
                        // Update the log file
                        Monitor.UpdateLog(System.IO.File.GetLastWriteTime(aLogFile), XervBackup.Library.Logging.AppendLog.LogFileToXML(aLogFile));
                    }
#if !DEBUG
                    );
#endif
                    if (phEx != null)
                        Library.Logging.Log.WriteMessage("Monitor Update failed "+Monitor.Name, 
                            XervBackup.Library.Logging.LogMessageType.Warning, phEx);
                }
            }
        }
        private static string[] DisabledMonitors = null;
        private static bool RunBackup(string[] args, XervBackup.Scheduler.Data.HistoryDataSet.HistoryRow aHistoryRow)
        {
            // Log the start
            Library.Logging.Log.WriteMessage(Name + " " + String.Join(" ", args) + (itsDryRun ? " DryRun as" : " as ") + XervBackup.Scheduler.Utility.User.UserName,
                XervBackup.Library.Logging.LogMessageType.Information);

            // See if the XML file name was on the command line
            string XML = (args.Length > 1) ? XML = args[1] : XervBackup.Scheduler.Data.SchedulerDataSet.DefaultPath();
            // Convert our options to XervBackup options
            Options BackupOptions = new Options(Job, XML);
            // Get disabled monitors
            DisabledMonitors = BackupOptions.DisabledMonitors;
            // Complain about any results from the drive mapping
            if (!string.IsNullOrEmpty(BackupOptions.MapResults))
                Library.Logging.Log.WriteMessage(BackupOptions.MapResults, XervBackup.Library.Logging.LogMessageType.Information);

            // Get the signature file temp
            XervBackup.Library.Utility.TempFolder SigTemp = new XervBackup.Library.Utility.TempFolder();
            string SigThingy = System.IO.Path.Combine(SigTemp, XervBackup.Scheduler.Data.SchedulerDataSet.DefaultName);
            if (XervBackup.Scheduler.Utility.Tools.NoException((Action)delegate() { System.IO.File.Copy(XervBackup.Scheduler.Data.SchedulerDataSet.DefaultPath(), SigThingy); }))
                BackupOptions["signature-control-files"] = SigThingy;

            // See if there is a pipe server listening, if so, connect to it for progress messages
            bool HasPipe = XervBackup.Scheduler.Utility.NamedPipeServerStream.ServerIsUp(
                XervBackup.Scheduler.Utility.NamedPipeServerStream.MakePipeName(PipeBaseName, XervBackup.Scheduler.Utility.User.UserName, System.IO.Pipes.PipeDirection.In));
            if (HasPipe) Pipe.Connecter();
            // Run the dern thing already
            string Result = "Not started";
            bool OK = false;
            if (itsDryRun)
            {
                OK = true;
                Result = "DryRun";
                if(HasPipe) TestProgress(5); // Just send fake progress
            }
            else
            {
                try
                {
                    using (XervBackup.Library.Main.Interface i = new XervBackup.Library.Main.Interface(BackupOptions.Target, BackupOptions))
                    {
                        // Set our events if we have a pipe
                        if (HasPipe)
                        {
                            i.OperationProgress += new XervBackup.Library.Main.OperationProgressEvent(Pipe.OperationProgress);
                            i.OperationCompleted += new XervBackup.Library.Main.OperationProgressEvent(Pipe.OperationProgress);
                        }
                        Result = i.Backup(BackupOptions.Source);
                    }
                    OK = true;
                }
                catch (Exception Ex)
                {
                    // Dang
                    Result = "Error: " + Ex.Message;
                }
            }
            // Log the done.
            Library.Logging.Log.WriteMessage("Finished: "+(OK?"OK":Result), OK ? XervBackup.Library.Logging.LogMessageType.Information : XervBackup.Library.Logging.LogMessageType.Error);
            // Put deletions in the log, where they belong.
            // Deleting backup at 07/06/2011 06:05:21
            foreach(string Line in Result.Split('\n'))
                if (Line.StartsWith("Deleting backup at "))
                    Library.Logging.Log.WriteMessage(Line, XervBackup.Library.Logging.LogMessageType.Information);
            // OK, made it, update the history
            aHistoryRow.Update("Backup", BackupOptions.Full, OK, Result, BackupOptions.Checksum, BackupOptions.CheckMod);
            LimitLogFiles(BackupOptions.LogFileMaxAgeDays); // zero has no effect.
            // woot
            return OK;
        }
        /// <summary>
        /// Delete old log files
        /// </summary>
        /// <param name="aMaxDays">Number of days to keep logs (0=always)</param>
        private static void LimitLogFiles(int aMaxDays)
        {
            if (aMaxDays <= 0) return;
            foreach (string Entry in System.IO.Directory.GetFiles(XervBackup.Scheduler.Utility.Tools.LogFileDirectory(Package), XervBackup.Scheduler.Utility.Tools.LogFileFilter))
            {
                if ((DateTime.Now - (new System.IO.FileInfo(Entry).LastWriteTime)).TotalDays > aMaxDays)
                    XervBackup.Scheduler.Utility.Tools.TryCatch((Action)delegate()
                        {
                            System.IO.File.SetAttributes(Entry, System.IO.FileAttributes.Normal);
                            System.IO.File.Delete(Entry);
                        });
            }
        }
        private static void TestProgress(int aSeconds)
        {
            Job = "PipeTest";
            Debug.WriteLine(Job);
            for (int i = 0; i<aSeconds*2 ; i++)
            {
                int P = (i % 100);
                Pipe.OperationProgress(null, XervBackup.Library.Main.XervBackupOperation.Backup, XervBackup.Library.Main.XervBackupOperationMode.Backup,
                    P, P, "test:" + P.ToString(), P.ToString());
                System.Threading.Thread.Sleep(500);
            }
        }
    }
}
