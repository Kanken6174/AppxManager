using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace AppxManager.model
{
    public class AppxPackage
    {
        public string Name;
        public string Publisher;
        public string PublisherId;
        public string Architecture;
        public string ResourceId;
        public string Version;
        public string PackageFamilyName;
        public string PackageFullName;
        public string InstallLocation;
        public string PackageUserInformation;
        public string DependenciesRaw;
        public List<string> Dependencies = new List<string>();

        public bool IsFramework;
        public bool IsResourcePackage;
        public bool IsBundle;
        public bool IsDevelopmentMode;
        public bool NonRemovable;
        public bool IsPartiallyStaged;


        public string SignatureKind;
        public string Status;



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
                InstallLocation = v.Members["InstallLocation"].Value.ToString();

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
