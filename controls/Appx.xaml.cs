using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace AppxManager.controls
{
    /// <summary>
    /// Logique d'interaction pour appx.xaml
    /// </summary>
    public partial class Appx : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public bool isInstalled { get; set; } = false;
        public string appxID { get; set; } = "";

        public Appx()
        {
            InitializeComponent();
        }

        public void uninstall_appx(object sender, RoutedEventArgs e)
        {

        }

        public void install_appx(object sender, RoutedEventArgs e)
        {

        }
    }
}
