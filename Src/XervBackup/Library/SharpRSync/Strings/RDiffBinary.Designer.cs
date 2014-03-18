﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XervBackup.Library.SharpRSync.Strings {
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
    internal class RDiffBinary {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal RDiffBinary() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("XervBackup.Library.SharpRSync.Strings.RDiffBinary", typeof(RDiffBinary).Assembly);
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
        ///   Looks up a localized string similar to Encoded value had unexpected length.
        /// </summary>
        internal static string EncodedLengthError {
            get {
                return ResourceManager.GetString("EncodedLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid data length.
        /// </summary>
        internal static string InvalidDataLengthError {
            get {
                return ResourceManager.GetString("InvalidDataLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid delta copy command: {0}.
        /// </summary>
        internal static string InvalidDeltaCopyCommandError {
            get {
                return ResourceManager.GetString("InvalidDeltaCopyCommandError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid size for encoded value!.
        /// </summary>
        internal static string InvalidEncodedSizeError {
            get {
                return ResourceManager.GetString("InvalidEncodedSizeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Invalid literal delta command: {0}.
        /// </summary>
        internal static string InvalidLiteralCommand {
            get {
                return ResourceManager.GetString("InvalidLiteralCommand", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to size item, because it is larger than: {0}.
        /// </summary>
        internal static string SizeTooLargeError {
            get {
                return ResourceManager.GetString("SizeTooLargeError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Value must be less than: {0}.
        /// </summary>
        internal static string ValueTooLargeError {
            get {
                return ResourceManager.GetString("ValueTooLargeError", resourceCulture);
            }
        }
    }
}
