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
    public class AppxPackage
    {
        public string Name { get; set; } = "sample";
        public string Publisher { get; set; }
        public string PublisherId { get; set; }
        public string Architecture { get; set; }
        public string ResourceId { get; set; }
        public string Version { get; set; }
        public string PackageFamilyName { get; set; }
        public string PackageFullName { get; set; }
        public string InstallLocation { get; set; }
        public string PackageUserInformation { get; set; }
        public string DependenciesRaw { get; set; }
        public List<string> Dependencies = new List<string>();

        public bool IsFramework { get; set; }
        public bool IsResourcePackage { get; set; }
        public bool IsBundle { get; set; }
        public bool IsDevelopmentMode { get; set; }
        public bool NonRemovable { get; set; }
        public bool IsPartiallyStaged { get; set; }


        public string SignatureKind { get; set; }
        public string Status { get; set; }

        public string DisplayName { get; set; }
        public appManifestPackage Manifest { get; set; }


        public AppxPackage(PSObject v)
        {
            try
            {
                Name = v.Members["Name"].Value.ToString();
                Publisher = v.Members["Publisher"].Value.ToString();
                PublisherId = v.Members["PublisherId"].Value.ToString();
                Architecture = v.Members["Architecture"].Value.ToString();
                ResourceId = v.Members["ResourceId"].Value.ToString();
                Version = v.Members["Version"].Value.ToString();
                PackageFamilyName = v.Members["PackageFamilyName"].Value.ToString();
                PackageFullName = v.Members["PackageFullName"].Value.ToString();
                InstallLocation = ((v.Members["InstallLocation"] != null) ? v.Members["InstallLocation"].Value.ToString() : "invalid");

                IsFramework = (bool)v.Members["IsFramework"].Value;
                PackageUserInformation = v.Members["PackageUserInformation"].Value.ToString();
                IsResourcePackage = (bool)v.Members["IsResourcePackage"].Value;
                IsBundle = (bool)v.Members["IsBundle"].Value;
                IsDevelopmentMode = (bool)v.Members["IsDevelopmentMode"].Value;
                NonRemovable = (bool)v.Members["NonRemovable"].Value;
                DependenciesRaw = v.Members["Dependencies"].Value.ToString();
                IsPartiallyStaged = (bool)v.Members["IsPartiallyStaged"].Value;

                SignatureKind = v.Members["SignatureKind"].Value.ToString();
                Status = v.Members["Status"].Value.ToString();


                string[] deps = DependenciesRaw.Split(' ');
                Dependencies = deps.ToList();
            }
            catch (NullReferenceException ex)
            {

            }
        }
    }
}
