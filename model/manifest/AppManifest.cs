using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace AppxManager.model
{
    public class appManifestPackage
    {
        public string? Xml { get; set; }
        public string? Xmlns { get; set; }
        public string? mp { get; set; }
        public string? desktop { get; set; }
        public string? uap { get; set; }
        public string? uap3 { get; set; }
        public string? uap5 { get; set; }
        public string? rescap { get; set; }
        public string? wincap { get; set; }
        public string? desktop6 { get; set; }
        public List<string>? IgnorableNamespaces { get; set; } = new List<string>();
        public string? build { get; set; }
        public string? comment { get; set; }
        
        public Properties? Properties { get; set; } = new Properties();
        public Applications? Applications { get; set; } = new Applications();
        public Capabilities? Capabilities { get; set; } = new Capabilities();
        public Dependencies? Dependencies { get; set; } = new Dependencies();
        public Extensions? Extensions { get; set; } = new Extensions();
        public Identity? Identities { get; set; } = new Identity();
        public Metadata? Metadata { get; set; } = new Metadata();
        public PhoneIdentity? phoneIdentity { get; set; } = new PhoneIdentity();
        public Resources? Resources { get; set; } = new Resources();

        public appManifestPackage() { }
    }
}
