using System;
using System.Reflection;
using System.Windows;

namespace vServiceHelper
{
    /// <summary>
    /// Interaktionslogik für InfoWindow.xaml
    /// </summary>
    public partial class InfoWindow : Window
    {

        public InfoWindow()
        {
            InitializeComponent();
            this.lblHeader.Content += "v " + Assembly.GetEntryAssembly().GetName().Version.ToString();
        }

        /// <summary>
        /// Close the Window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void closeWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
