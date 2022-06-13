using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading.Tasks;

namespace AppxManager.model.Factories
{
    public static class appManifestFactory
    {
        public static List<model.appManifestPackage> loadManifests()
        {
            List<appManifestPackage> toReturn = new List<appManifestPackage>();

            using (RunspacePool rsp = RunspaceFactory.CreateRunspacePool())
            {
                rsp.Open();
                var sessionState = InitialSessionState.CreateDefault();
                sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;
                using (PowerShell powershell = PowerShell.Create(sessionState))
                {

                    powershell.RunspacePool = rsp;
                    powershell.AddScript("Import-Module -Name Appx -UseWIndowsPowershell;" +
                                         $"Get-AppxPackage {(appSettings.AllUsers ? " -AllUsers" : String.Empty)} | Get-AppxPackageManifest");
                    try
                    {
                        Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass");
                        Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                        foreach (PSObject apx in PSIResults)
                        {
                            appManifestPackage toAdd = new appManifestPackage();
                            toAdd.Xml = apx?.Members["xml"]?.Value?.ToString();
                            toReturn.Add(toAdd);
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
                                         $"$manifest = Get-AppxPackage {(appSettings.AllUsers ? "-AllUsers" : String.Empty)} | Get-AppxPackageManifest;" +
                                         "$manifest.Package.Properties");

                    Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass");
                    Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                    int index = 0;
                    foreach (PSObject apx in PSIResults)
                    {
                        toReturn[index].Properties.Logo = apx.Members["Logo"].Value.ToString();
                        toReturn[index].Properties.PublisherDisplayName = apx.Members["PublisherDisplayName"].Value.ToString();
                        toReturn[index].Properties.DisplayName = apx.Members["DisplayName"].Value.ToString();
                        index++;
                    }
                    powershell.Dispose();
                }
                sessionState = null;
                rsp.Close();
                rsp.Dispose();
            }

            return toReturn;
        }
    }
}
