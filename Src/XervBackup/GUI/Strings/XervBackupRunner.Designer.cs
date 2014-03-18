﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XervBackup.GUI.Strings {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class XervBackupRunner {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal XervBackupRunner() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("XervBackup.GUI.Strings.XervBackupRunner", typeof(XervBackupRunner).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The backup was stopped because the application was closed.
        /// </summary>
        internal static string ApplicationExitLogMesssage {
            get {
                return ResourceManager.GetString("ApplicationExitLogMesssage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Removing old backups ....
        /// </summary>
        internal static string CleaningUpMessage {
            get {
                return ResourceManager.GetString("CleaningUpMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed during cleanup: {0}.
        /// </summary>
        internal static string CleanupError {
            get {
                return ResourceManager.GetString("CleanupError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Cleanup output:.
        /// </summary>
        internal static string CleanupLogdataHeader {
            get {
                return ResourceManager.GetString("CleanupLogdataHeader", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Error: {0}.
        /// </summary>
        internal static string ErrorMessage {
            get {
                return ResourceManager.GetString("ErrorMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The backup was stopped, the reason was: {0}.
        /// </summary>
        internal static string OtherAbortMessage {
            get {
                return ResourceManager.GetString("OtherAbortMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XervBackup was terminated before the backup completed, status is unknown.
        /// </summary>
        internal static string ShutdownWhileBackupInprogress {
            get {
                return ResourceManager.GetString("ShutdownWhileBackupInprogress", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The backup was stopped because the application was closed by the task manager.
        /// </summary>
        internal static string TaskManagerCloseMessage {
            get {
                return ResourceManager.GetString("TaskManagerCloseMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The backup was stopped by the user.
        /// </summary>
        internal static string UserClosingMessage {
            get {
                return ResourceManager.GetString("UserClosingMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The backup was stopped because the machine was being shut down.
        /// </summary>
        internal static string WindowsShutdownMessage {
            get {
                return ResourceManager.GetString("WindowsShutdownMessage", resourceCulture);
            }
        }
    }
}
