﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XervBackup.GUI.Wizard_pages.Add_backup.Strings {
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
    internal class SelectWhen {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SelectWhen() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("XervBackup.GUI.Wizard_pages.Add_backup.Strings.SelectWhen", typeof(SelectWhen).Assembly);
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
        ///   Looks up a localized string similar to The interval between each full backup must be larger than the schedule interval..
        /// </summary>
        internal static string FullDurationShorterThanScheduleDurationError {
            get {
                return ResourceManager.GetString("FullDurationShorterThanScheduleDurationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have selected not to run the backup automatically.
        ///This means that you have to remember to manually start the backup periodically.
        ///
        ///Are you sure this is what you want?.
        /// </summary>
        internal static string NoScheduleWarning {
            get {
                return ResourceManager.GetString("NoScheduleWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have selected to only produce full backups.
        ///This may take up a lot of space on the backend.
        ///
        ///Are you sure this is what you want?.
        /// </summary>
        internal static string OnlyFullBackupsWarning {
            get {
                return ResourceManager.GetString("OnlyFullBackupsWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You have selected to only produce incremental backups.
        ///This may result in very lengthy restore operations, and increases the probability of a restore failure.
        ///
        ///Are you sure this is what you want?.
        /// </summary>
        internal static string OnlyIncrementalBackupsWarning {
            get {
                return ResourceManager.GetString("OnlyIncrementalBackupsWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to On this page you may set up when the backup is run. Automatically repeating the backup ensure that you have a backup, without requiring any action from you..
        /// </summary>
        internal static string PageDescription {
            get {
                return ResourceManager.GetString("PageDescription", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Select when the backup should run.
        /// </summary>
        internal static string PageTitle {
            get {
                return ResourceManager.GetString("PageTitle", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The time you entered is in the past..
        /// </summary>
        internal static string TimeIsInThePastWarning {
            get {
                return ResourceManager.GetString("TimeIsInThePastWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The time you entered will occur shortly..
        /// </summary>
        internal static string TimeOccursShortlyWarning {
            get {
                return ResourceManager.GetString("TimeOccursShortlyWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The settings you have selected will result in {0} incremental backups being created between each full backup.
        ///This can result in very lengthy restore operations, and increases the probability of a restore failure.
        ///
        ///Do you want to keep these settings?.
        /// </summary>
        internal static string TooManyIncrementalsWarning {
            get {
                return ResourceManager.GetString("TooManyIncrementalsWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The interval between full backups must be more than {0} minutes..
        /// </summary>
        internal static string TooShortFullDurationError {
            get {
                return ResourceManager.GetString("TooShortFullDurationError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The schedule interval must be more than {0} minutes..
        /// </summary>
        internal static string TooShortScheduleDurationError {
            get {
                return ResourceManager.GetString("TooShortScheduleDurationError", resourceCulture);
            }
        }
    }
}
