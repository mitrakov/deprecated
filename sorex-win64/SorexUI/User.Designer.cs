//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SorexUI {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "17.12.0.0")]
    internal sealed partial class User : global::System.Configuration.ApplicationSettingsBase {
        
        private static User defaultInstance = ((User)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new User())));
        
        public static User Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("<?xml version=\"1.0\" encoding=\"utf-16\"?>\n<ArrayOfString xmlns:xsd=\"http://www.w3.o" +
            "rg/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" />")]
        public global::System.Collections.Specialized.StringCollection recentFiles {
            get {
                return ((global::System.Collections.Specialized.StringCollection)(this["recentFiles"]));
            }
            set {
                this["recentFiles"] = value;
            }
        }
    }
}
