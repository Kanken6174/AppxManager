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
            List<AppxPackage> apx = appxFactory.LoadAppxs();
            foreach (AppxPackage pkg in apx)
            {
                appxListEntry appx = new appxListEntry();
                appx.setAppx(pkg);
                stacker.Children.Add(appx);
                _items.Add(appx);
            }
            TaskQueueManager.StartAsync();
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
