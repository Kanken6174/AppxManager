using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace AppxManager.model.Factories
{
    public static class appxFactory
    {
        public static List<AppxPackage> LoadAppxs(MainWindow.ProcessingFinishedCallback callback)
        {
            List<AppxPackage> toReturn = new List<AppxPackage>();

            var sessionState = InitialSessionState.CreateDefault();
            sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

            using (PowerShell powershell = PowerShell.Create(sessionState))
            {
                powershell.AddScript($"Import-Module -Name Appx -UseWIndowsPowershell;Get-AppxPackage {(appSettings.AllUsers ? "-AllUsers" : String.Empty)} ");

                Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass -Version 2.0");
                Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();

                List<appManifestPackage> apl = appManifestFactory.loadManifests();
                int index = 0;
                foreach (PSObject apx in PSIResults)
                {
                    AppxPackage appxPackage = new AppxPackage(apx);
                    if (index >= apl.Count)
                    {
                        appxPackage.Manifest = new appManifestPackage();
                    }
                    else
                    {
                        appxPackage.Manifest = apl[index]; 
                    }
                    toReturn.Add(appxPackage);
                    index++;
                }

                TaskQueueManager.FinalTask = new Task(() =>
                {
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        callback(toReturn);
                    }, null);
                });

                TaskQueueManager.StartAsync();
                powershell.Stop();
                powershell.Dispose();
            }
            return toReturn;
        }
    }
}
