﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.237
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace XervBackup.Library.Encryption.Strings {
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
    internal class GPGEncryption {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal GPGEncryption() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("XervBackup.Library.Encryption.Strings.GPGEncryption", typeof(GPGEncryption).Assembly);
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
        ///   Looks up a localized string similar to The GPG encryption module uses the GNU Privacy Guard program to encrypt and decrypt files. It requires that the gpg executable is available on the system. On Windows it is assumed that this is in the default installation folder under program files, under Linux and OSX it is assumed that the program is available via the PATH environment variable. It is possible to supply the path to GPG using the --gpg-program-path switch..
        /// </summary>
        internal static string Description {
            get {
                return ResourceManager.GetString("Description", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to GNU Privacy Guard, external.
        /// </summary>
        internal static string DisplayName {
            get {
                return ResourceManager.GetString("DisplayName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use this switch to specify any extra options to GPG. You cannot specify the --passphrase-fd option here. The --decrypt option is always specified..
        /// </summary>
        internal static string GpgencryptiondecryptionswitchesLong {
            get {
                return ResourceManager.GetString("GpgencryptiondecryptionswitchesLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Extra GPG commandline options for decryption.
        /// </summary>
        internal static string GpgencryptiondecryptionswitchesShort {
            get {
                return ResourceManager.GetString("GpgencryptiondecryptionswitchesShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This option has non-standard handling, please use the --{0} option instead..
        /// </summary>
        internal static string Gpgencryptiondisablearmordeprecated {
            get {
                return ResourceManager.GetString("Gpgencryptiondisablearmordeprecated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The GPG encryption/decryption will use the --armor option for GPG to protect the files with armor. Specify this switch to remove the --armor option..
        /// </summary>
        internal static string GpgencryptiondisablearmorLong {
            get {
                return ResourceManager.GetString("GpgencryptiondisablearmorLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Don&apos;t use GPG Armor.
        /// </summary>
        internal static string GpgencryptiondisablearmorShort {
            get {
                return ResourceManager.GetString("GpgencryptiondisablearmorShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use this option to supply the --armor option to GPG. The files will be larger but can be sent as pure text files..
        /// </summary>
        internal static string GpgencryptionenablearmorLong {
            get {
                return ResourceManager.GetString("GpgencryptionenablearmorLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use GPG Armor.
        /// </summary>
        internal static string GpgencryptionenablearmorShort {
            get {
                return ResourceManager.GetString("GpgencryptionenablearmorShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use this switch to specify any extra options to GPG. You cannot specify the --passphrase-fd option here. The --encrypt option is always specified..
        /// </summary>
        internal static string GpgencryptionencryptionswitchesLong {
            get {
                return ResourceManager.GetString("GpgencryptionencryptionswitchesLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Extra GPG commandline options for encryption.
        /// </summary>
        internal static string GpgencryptionencryptionswitchesShort {
            get {
                return ResourceManager.GetString("GpgencryptionencryptionswitchesShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Failed to execute GPG at &quot;{0}&quot;: {1}.
        /// </summary>
        internal static string GPGExecuteError {
            get {
                return ResourceManager.GetString("GPGExecuteError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to XervBackup was unable to verify the existence of GNU Privacy Guard.
        ///GPG may work regardless, if it is located in the system search path.
        ///If the encryption fails, no files will be backed up
        ///Do you want to continue anyway?.
        /// </summary>
        internal static string GPGNotFoundWarning {
            get {
                return ResourceManager.GetString("GPGNotFoundWarning", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to the GNU Privacy Guard program. If not supplied, XervBackup will assume that the program &quot;gpg&quot; is available in the system path..
        /// </summary>
        internal static string GpgprogrampathLong {
            get {
                return ResourceManager.GetString("GpgprogrampathLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The path to GnuPG.
        /// </summary>
        internal static string GpgprogrampathShort {
            get {
                return ResourceManager.GetString("GpgprogrampathShort", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The GNU Privacy Guard can optionally sign volumes with a special key. This feature is not currently active in XervBackup..
        /// </summary>
        internal static string SignkeyLong {
            get {
                return ResourceManager.GetString("SignkeyLong", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sign key for GnuPG.
        /// </summary>
        internal static string SignkeyShort {
            get {
                return ResourceManager.GetString("SignkeyShort", resourceCulture);
            }
        }
    }
}
