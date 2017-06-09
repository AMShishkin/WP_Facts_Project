using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;

namespace Facts
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        // ----------------------------------------------------------------------------------------------------------------- //
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
         
        
    
            this.AppBarLogic();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppEmail_Click(object sender, EventArgs e)
        {
            EmailComposeTask emailComposeTask = new EmailComposeTask { Subject = "[И это факт]", To = "AMShishkin.vrn@gmail.com" };
            emailComposeTask.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppBarLogic()
        {
            this.ApplicationBar.Mode = AppHelper.AppBar ? Microsoft.Phone.Shell.ApplicationBarMode.Default : Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
        }
        // ----------------------------------------------------------------------------------------------------------------- //
       
    }
}