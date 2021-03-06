using AppxManager.model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;

namespace AppxManager.controls
{
    /// <summary>
    /// Logique d'interaction pour appxListEntry.xaml
    /// </summary>
    public partial class appxListEntry : UserControl
    {
        public AppxPackage myAppx { get; set; }

        public bool installed { get; set; } = true;

        public appxListEntry()
        {
            InitializeComponent();
        }

        public void setAppx(AppxPackage apx)
        {
            myAppx = apx;
            appxName.Content = apx.DisplayName + " | " +apx.Name;
            TaskQueueManager.Queue.Add(new Task(() => setLogoAndManifest()));
        }

        private void setLogoAndManifest()
        {
            try
            {
                string logopath = "", logopathlm = myAppx.Manifest.Properties.Logo;
                if(myAppx.Manifest.Properties.Logo == null){
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        appxName.Content = $"{myAppx.Name} | {((myAppx.Manifest.Properties.DisplayName == null || myAppx.Manifest.Properties.DisplayName == "ms-resource:AppName") ? "no valid app name" : myAppx.Manifest.Properties.DisplayName)}";
                    }, null);
                    return;
                }
                if (!logopathlm.Contains("Assets"))
                    logopath = @"Assets\" + logopathlm;
                else
                    logopath = logopathlm;
                string logopathlg = logopath.Insert(logopath.IndexOf('.'), ".scale-200");
                string absoluteLogoPath = myAppx.InstallLocation + @"\" + logopathlg;
                BitmapImage bmp = new BitmapImage();
                if (File.Exists(absoluteLogoPath))
                {
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        logo.Source = new BitmapImage(new Uri(absoluteLogoPath, UriKind.Absolute));
                    }, null);
                }
                else if (File.Exists(myAppx.InstallLocation + @"\" + logopath))
                {
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        logo.Source = new BitmapImage(new Uri(myAppx.InstallLocation + @"\" + logopath));
                    }, null);
                }
                else if (File.Exists(myAppx.InstallLocation + @"\logo.png"))
                {
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        logo.Source = new BitmapImage(new Uri(myAppx.InstallLocation + @"\logo.png", UriKind.Absolute));
                    }, null);
                }
                else if (File.Exists(myAppx.InstallLocation + @"\" + logopath.Insert(logopath.IndexOf('.'), ".scale-100")))
                {
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        logo.Source = new BitmapImage(new Uri(myAppx.InstallLocation + @"\" + logopath.Insert(logopath.IndexOf('.'), ".scale-100"), UriKind.Absolute));
                    }, null);
                }
                else if (File.Exists(myAppx.InstallLocation + @"\uwp\" + logopath.Insert(logopath.IndexOf('.'), ".scale-100")))
                {
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        logo.Source = new BitmapImage(new Uri(myAppx.InstallLocation + @"\uwp\" + logopath.Insert(logopath.IndexOf('.'), ".scale-100"), UriKind.Absolute));
                    }, null);
                }
                else if (File.Exists(myAppx.InstallLocation + @"\images\" + logopath.Insert(logopath.IndexOf('.'), ".scale-100")))
                {
                    App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                    {
                        logo.Source = new BitmapImage(new Uri(myAppx.InstallLocation + @"\images\" + logopath.Insert(logopath.IndexOf('.'), ".scale-100"), UriKind.Absolute));
                    }, null);
                }
                App.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (SendOrPostCallback)delegate
                {
                    appxName.Content = $"{myAppx.Name} | {((myAppx.Manifest.Properties.DisplayName == "ms-resource:AppName") ? "no valid app name" : myAppx.Manifest.Properties.DisplayName)}";
                }, null);
                
            }
            catch (NullReferenceException ex)
            {

            }
            catch (Exception e)
            {

            }
        }

        private void Button_Click(object sender, RoutedEventArgs e) //install
        {
            appxManager.UninstallAppx(myAppx);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)   //uninstall
        {
            appxManager.InstallAppx(myAppx);
        }

        private void DockPanel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ui_windows.details details = new ui_windows.details(myAppx);
            details.logo.Source = logo.Source;
            details.installed = installed;
            details.Show();
        }

        private void DockPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void DockPanel_MouseLeave(object sender, MouseEventArgs e)
        {
            Cursor = Cursors.Arrow;
        }
    }
}
