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

namespace AppxManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Appx> _items = new System.Collections.Generic.List<Appx>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var sessionState = InitialSessionState.CreateDefault();
            sessionState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;

            using (PowerShell powershell = PowerShell.Create(sessionState))
            {
                powershell.AddScript("Import-Module -Name Appx -UseWIndowsPowershell;Get-AppxPackage");

                Collection<PSObject> PSIResults = powershell.Invoke("-ExecutionPolicy Bypass");
                Collection<ErrorRecord> Errors = powershell.Streams.Error.ReadAll();
                int i = 0;
                stacker.Children.Clear();
                foreach (ErrorRecord error in Errors)
                {
                    Label lbl = new Label();
                    lbl.Content = error.Exception.Message.ToString();
                    stacker.Children.Add(lbl);
                    i += 20;
                }
                foreach( PSObject apx in PSIResults)
                {
                    Appx appx = new Appx();
                    AppxPackage appxPackage = new AppxPackage(apx);
                    appx.appxID = apx.BaseObject.ToString();
                    stacker.Children.Add(appx);
                    _items.Add(appx);
                }
                powershell.Dispose();
            }
        }
    }
}
