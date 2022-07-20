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
using AppxManager.model.Factories;
using AppxManager.Extensions;

namespace AppxManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<appxListEntry> _AppxUIItems = new List<appxListEntry>();

        public List<AppxPackage> apx = new List<AppxPackage>();
        public delegate void ProcessingFinishedCallback(List<AppxPackage> packages);
        public MainWindow()
        {
            InitializeComponent();
        }

        public void LoadingCompletedHandler(List<AppxPackage> packages)
        {
            loader.Visibility = Visibility.Collapsed;
            foreach (AppxPackage pkg in packages)
            {
                appxListEntry appx = new appxListEntry();
                appx.setAppx(pkg);
                stacker.Children.Add(appx);
                _AppxUIItems.Add(appx);
            }
            TaskQueueManager.StartAsync();
            ScanButton.IsEnabled = true;
            AllUserCheckbox.IsEnabled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ScanButton.IsEnabled = false;
            AllUserCheckbox.IsEnabled = false;
            _AppxUIItems.Clear();
            stacker.Children.Clear();
            loader.Visibility = Visibility.Visible;
            TaskQueueManager.StopAll();
            List<AppxPackage> apx = new List<AppxPackage>();
            ProcessingFinishedCallback callback = LoadingCompletedHandler;
            Task t = new Task(() => apx = appxFactory.LoadAppxs(callback));
            t.Start();
        }

        private void AllUserCheckbox_Checked(object sender, RoutedEventArgs e)
        {
            appSettings.AllUsers = true;
        }

        private void AllUserCheckbox_Unchecked(object sender, RoutedEventArgs e)
        {
            appSettings.AllUsers = false;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            foreach(appxListEntry appx in _AppxUIItems)
            {
                bool hasDisplayName = (SearchTermTextBox.Text != "" && appx.myAppx.DisplayName != null);
                bool matches = false;
                matches = appx.myAppx.Name.Contains_nocase(SearchTermTextBox.Text);
                if(hasDisplayName && !matches)
                    matches = appx.myAppx.Manifest.Properties.DisplayName.Contains(SearchTermTextBox.Text);
                if(matches)
                    appx.Visibility = Visibility.Visible;
                else
                    appx.Visibility = Visibility.Collapsed;
            }
        }
    }
}
