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

        public appManifestPackage(string package)
        {
            using (RunspacePool rsp = RunspaceFactory.CreateRunspacePool())
            {
                rsp.Open();
                var sessionState = InitialSessionState.CreateDefault();
                sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;
                using (PowerShell powershell = PowerShell.Create(sessionState))
                {

                    powershell.RunspacePool = rsp;
                    powershell.AddScript("Import-Module -Name Appx -UseWIndowsPowershell;" +
                                         $"Get-AppxPackage '{package}' {(appSettings.AllUsers ? " - AllUsers" : String.Empty)} | Get-AppxPackageManifest");
                    try
                    {
                        Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass");
                        Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                        foreach (PSObject apx in PSIResults)
                        {
                            Xml = apx.Members["xml"].Value.ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    powershell.Dispose();
                }

                using (PowerShell powershell = PowerShell.Create(sessionState))
                {
                    powershell.RunspacePool = rsp;
                    powershell.AddScript("Import-Module -Name Appx -UseWIndowsPowershell;" +
                                         $"$manifest = Get-AppxPackage '{package}' {(appSettings.AllUsers ? "-AllUsers" : String.Empty)} | Get-AppxPackageManifest;" +
                                         "$manifest.Package.Properties");

                    Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass");
                    Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                    foreach (PSObject apx in PSIResults)
                    {
                        Properties.Logo = apx.Members["Logo"].Value.ToString();
                        Properties.PublisherDisplayName = apx.Members["PublisherDisplayName"].Value.ToString();
                        Properties.DisplayName = apx.Members["DisplayName"].Value.ToString();
                    }
                    powershell.Dispose();
                }
                sessionState = null;
                rsp.Close();
                rsp.Dispose();
            }
        }
    }
}
