using AppxManager.model;
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
using System.Windows.Shapes;

namespace AppxManager.ui_windows
{
    /// <summary>
    /// Logique d'interaction pour details.xaml
    /// </summary>
    public partial class details : Window
    {
        public AppxPackage myAppx { get; set; }

        public bool installed { get; set; } = true;
        public details(AppxPackage apx)
        {
            InitializeComponent();
            myAppx = apx;
            if (myAppx == null) return;
            apxname.Text = myAppx.Name;
            apxnameFull.Text = myAppx.Manifest.Properties.DisplayName;
        }
    }
}
