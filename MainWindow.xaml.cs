using System;
using System.Windows;
using System.Windows.Media;
using System.ServiceProcess;

namespace vServiceHelper
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        /// <summary>
        /// Services
        /// </summary>
        public string[] services = { "VMnetDHCP", "VMAuthdService", "VMware NAT Service", "VMUSBArbService" };

        /// <summary>
        /// Number of curently running Services
        /// </summary>
        private int servicesRunning = 0;

        public MainWindow()
        {
            InitializeComponent();

            this.txtMessages.Text += "[" + DateTime.Now.ToString() + "] Welcome :-)";
        }

        private void checkAllServices(object sender, RoutedEventArgs e)
        {
            this.servicesRunning = 0;
            this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Check Service Status ....";
            foreach (string service in this.services)
            {
                this.checkService(service);
            }

            if (this.servicesRunning > 0)
            {
                this.btnAction.Content = "Stop all Services";
                this.btnAction.IsEnabled = true;
                this.btnAction.Background = new SolidColorBrush(Colors.Red);
                this.btnAction.Click += new RoutedEventHandler(stopAllServices);
            }
            else
            {
                this.btnAction.Content = "Start all Services";
                this.btnAction.IsEnabled = true;
                this.btnAction.Background = new SolidColorBrush(Colors.Green);
                this.btnAction.Click += new RoutedEventHandler(startAllServices);
            }
        }

        private void startAllServices(object sender, RoutedEventArgs e)
        {
            foreach (string service in this.services)
            {
                this.startService(service);
            }
            this.checkAllServices(sender, e);
        }

        private void stopAllServices(object sender, RoutedEventArgs e)
        {
            foreach (string service in this.services)
            {
                this.stopService(service);
            }
            this.checkAllServices(sender, e);
        }

        private void startService(string Name)
        {
            DateTime date = DateTime.Now;

            ServiceController service = new ServiceController(Name);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                try
                {
                    service.Start();
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' started!";
                }
                catch (Exception e)
                {
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' could not stopped! (" + e.InnerException.Message.ToString() + ")";
                }
            }
            else
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' allready running!";
            }
        }

        private void stopService(string Name)
        {
            DateTime date = DateTime.Now;

            ServiceController service = new ServiceController(Name);
            if (service.Status == ServiceControllerStatus.Running)
            {
                try
                {
                    service.Stop();
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' stopped!";
                }
                catch (Exception e)
                {
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' could not stopped! (" + e.InnerException.Message.ToString() + ")";
                }
            }
            else
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' allready stopped!";
            }
        }

        private void checkService(string Name)
        {
            ServiceController service = new ServiceController(Name);
            if (service.Status == ServiceControllerStatus.Stopped)
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' stopped!";
            }
            else if (service.Status == ServiceControllerStatus.Running)
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + " running!";
                this.servicesRunning++;
            }
            else
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + " unknown";
            }
        }

        private void scrollDown(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.txtMessages.ScrollToEnd();
        }

        private void openInfoWindow(object sender, RoutedEventArgs e)
        {
            InfoWindow i = new InfoWindow();
            i.ShowDialog();
        }

    }
}
