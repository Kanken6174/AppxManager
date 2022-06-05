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
    public static class appxManager
    {

        public static void UninstallAppx(AppxPackage toUninstall)
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
                                         $"Get-AppxPackage {toUninstall.Name} | remove-appxpackage");
                    try
                    {
                        Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass");
                        Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                    }
                    catch (Exception ex)
                    {

                    }
                    powershell.Dispose();
                }
                sessionState = null;
                rsp.Close();
                rsp.Dispose();
            }
        }

        public static void InstallAppx(AppxPackage toInstall)
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
                                         $"Add-AppxPackage -DisableDevelopmentMode -Register \"{toInstall.InstallLocation}\\AppXManifest.xml\"");
                    try
                    {
                        Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass");
                        Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                    }
                    catch (Exception ex)
                    {

                    }
                    powershell.Dispose();
                }
                sessionState = null;
                rsp.Close();
                rsp.Dispose();
            }
        }

        public static void InstallAppx(String PackageName)
        {

        }
    }
}
