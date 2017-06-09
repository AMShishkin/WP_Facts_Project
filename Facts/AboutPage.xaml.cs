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
    public partial class AboutPage : PhoneApplicationPage
    {
        public AboutPage()
        {
            InitializeComponent();
        }

        // ----------------------------------------------------------------------------------------------------------------- //
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            this.ApplicationBar.Mode = AppHelper.AppBar ? Microsoft.Phone.Shell.ApplicationBarMode.Default : Microsoft.Phone.Shell.ApplicationBarMode.Minimized;
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppShow_Click(object sender, System.EventArgs e)
        {
            MarketplaceSearchTask showMyApps = new MarketplaceSearchTask { SearchTerms = "AMShishkin" };
            showMyApps.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void AppRate_Click(object sender, System.EventArgs e)
        {
            AppHelper.Storage["IS_RATE"] = AppHelper.IsRate = true;
            AppHelper.Storage.Save();
            MarketplaceReviewTask rate = new MarketplaceReviewTask();
            rate.Show();
        }
        // ----------------------------------------------------------------------------------------------------------------- //
        private void ButNews_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("Ждите скорых обновлений приложения!", "Версия v1.0.0", MessageBoxButton.OK);
        }
    }
}