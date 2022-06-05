using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Management.Automation;
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using AppxManager.controls;
using AppxManager.model;
using System.Windows.Threading;
using System.Threading;

namespace AppxManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<appxListEntry> _items = new List<appxListEntry>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _items.Clear();
            stacker.Children.Clear();
            TaskQueueManager.StopAll();
            LoadAppx();
        }
        private void LoadAppx()
        {
            var sessionState = InitialSessionState.CreateDefault();
            sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

            using (PowerShell powershell = PowerShell.Create(sessionState))
            {
                powershell.AddScript($"Import-Module -Name Appx -UseWIndowsPowershell;Get-AppxPackage {(appSettings.AllUsers ? "-AllUsers" : String.Empty)} ");

                Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass -Version 2.0");
                Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                int i = 0;
                stacker.Children.Clear();
                foreach (ErrorRecord error in Errors)
                {
                    Label lbl = new Label();
                    lbl.Content = error.Exception.Message.ToString();
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        stacker.Children.Add(lbl);
                    }, null);
                    i += 20;
                }
                foreach (PSObject apx in PSIResults)
                {
                    appxListEntry appx = new appxListEntry();
                    AppxPackage appxPackage = new AppxPackage(apx);
                    appx.setAppx(appxPackage);
                    stacker.Children.Add(appx);
                    _items.Add(appx);
                }
                TaskQueueManager.StartAsync();
                powershell.Stop();
                powershell.Dispose();
            }
        }

        private void AllUserCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            appSettings.AllUsers = true;
        }

        private void AllUserCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings.AllUsers = false;
        }
    }
}
