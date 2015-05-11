using System;
using System.Windows;
using System.Windows.Media;
using System.ServiceProcess;
using System.Security.Principal;
using System.Collections.Generic;
using System.Windows.Controls;

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
        public string[] services = {};

        /// <summary>
        /// Number of curently running Services
        /// </summary>
        private int servicesRunning = 0;

        /// <summary>
        /// Configuration
        /// </summary>
        private Config c;

        /// <summary>
        /// 
        /// </summary>
        public MainWindow()
        {
            // load config an set combobox
            try
            {
                this.c = new Config().load("config.xml");
            }
            catch (Exception e)
            {
                //string[] error = {"\n[" + DateTime.Now.ToString() + "] Ooops an Error while processing Config file! (" + e.Message + ")"};
            }
            InitializeComponent();

            // notify if not run with admin privileges
            if (this.IsAdministrator() == false)
            {
                this.txtMessages.Text += "Warning!!!\nIt seems you are not an Administrator. The App may not work as desired. Please start again with administrator privileges.";
            }
            else
            {
                this.txtMessages.Text += "[" + DateTime.Now.ToString() + "] Welcome :-)";
            }

            // load config an set combobox
            try
            {
                this.comboBoxServiceGroups.Items.Add("all services");
                foreach (KeyValuePair<string, List<string>> ConfigGroups in this.c.getServiceGroups())
                {
                    this.comboBoxServiceGroups.Items.Add(ConfigGroups.Key.ToString());
                    string[] serviceGroupsServices = ConfigGroups.Value.ToArray();
                }
            }
            catch (Exception e)
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Ooops an Error while processing Config file! (" + e.Message + ")";
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkAllServices(object sender, RoutedEventArgs e)
        {
            this.servicesRunning = 0;
            this.txtMessages.Text += "\n-------------------------------------\n[" + DateTime.Now.ToString() + "] Check Service Status ....";
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void startAllServices(object sender, RoutedEventArgs e)
        {
            this.btnAction.Content = " .... please wait ....";
            this.btnAction.IsEnabled = false;
            this.txtMessages.Text += "\n-------------------------------------\n[" + DateTime.Now.ToString() + "] Try to Start Services ....";
            foreach (string service in this.services)
            {
                this.startService(service);
            }
            this.checkAllServices(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void stopAllServices(object sender, RoutedEventArgs e)
        {
            this.btnAction.Content = " .... please wait ....";
            this.btnAction.IsEnabled = false;
            this.txtMessages.Text += "\n-------------------------------------\n[" + DateTime.Now.ToString() + "] Try to Stop Services ....";
            foreach (string service in this.services)
            {
                this.stopService(service);
            }
            this.checkAllServices(sender, e);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        private void startService(string Name)
        {
            DateTime date = DateTime.Now;

            try 
            {
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
                        this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' could not started! (" + e.InnerException.Message.ToString() + ")";
                    }
                }
                else
                {
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' allready running!";
                }
            }
            catch (Exception e)
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Ooops an Error while processing '" + Name + "'. (" + e.Message + ")";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        private void stopService(string Name)
        {
            DateTime date = DateTime.Now;

            try 
            {
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
            catch (Exception e)
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Ooops an Error while processing '" + Name + "'. (" + e.Message + ")";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Name"></param>
        private void checkService(string Name)
        {
            try
            {
                ServiceController service = new ServiceController(Name);
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' stopped!";
                }
                else if (service.Status == ServiceControllerStatus.Running)
                {
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' running!";
                    this.servicesRunning++;
                }
                else if (service.Status == ServiceControllerStatus.StartPending || service.Status == ServiceControllerStatus.StopPending)
                {
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Service '" + Name + "' pending!";
                    this.servicesRunning++;
                }
                else
                {
                    this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Status of Service '" + Name + "' unknown or not defined!";
                }
            }
            catch (Exception e)
            {
                this.txtMessages.Text += "\n[" + DateTime.Now.ToString() + "] Ooops an Error while processing '" + Name + "'. (" + e.Message + ")";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void scrollDown(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            this.txtMessages.ScrollToEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openInfoWindow(object sender, RoutedEventArgs e)
        {
            InfoWindow i = new InfoWindow();
            i.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openSettingsWindow(object sender, RoutedEventArgs e)
        {
            SettingsWindow s = new SettingsWindow();
            s.ShowDialog();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxServiceGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedGroup = this.comboBoxServiceGroups.SelectedValue.ToString();
            this.btnAction.IsEnabled = false;
            foreach (KeyValuePair<string, List<string>> ConfigGroups in this.c.getServiceGroups())
            {
                string[] serviceGroupsServices = ConfigGroups.Value.ToArray();
                if (selectedGroup == ConfigGroups.Key.ToString())
                {
                    this.services = serviceGroupsServices;
                }
                else if(selectedGroup == "all services")
                {
                    int oldServicesLength = this.services.Length;
                    Array.Resize<String>(ref this.services, oldServicesLength + serviceGroupsServices.Length);
                    Array.Copy(serviceGroupsServices, 0, this.services, oldServicesLength, serviceGroupsServices.Length);
                }
            }
        }

    }
}
