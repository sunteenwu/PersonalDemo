using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.ApplicationModel.Store;
using Windows.UI.Popups;
using Windows.UI.Notifications;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DistanceTracker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {   
        public MainPage()
        {
            this.InitializeComponent();
            RemoveAds();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            myFrame.Navigate(typeof(myPage.NewGroupPage));            
            txtPageNow.Text = DateTime.Now.Date.ToString("dd/MM/yyyy");
            //Start Tile
            
            
            //End Tile
        }
        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;                
        }
        private void btHome_Click(object sender, RoutedEventArgs e)
        {
            var selectButton = sender;            
            if (selectButton == btBaoCao)
            {
                myFrame.Navigate(typeof(myPage.ReportPage));
            }            
            if (selectButton == btThoat)
            {
                Application.Current.Exit();
            }
            if (selectButton == btAbout)
            {
                myFrame.Navigate(typeof(myPage.AboutPage));
            }
            if (selectButton == btGoupPage)
            {
                myFrame.Navigate(typeof(myPage.NewGroupPage));
            }
            if(selectButton == btSetting)
            {
                myFrame.Navigate(typeof(myPage.SettingP));
            }
            mySplitView.IsPaneOpen = !mySplitView.IsPaneOpen;
        }

        private void btRemoveAds_Click(object sender, RoutedEventArgs e)
        {
             BuyProduct("RemoveADS");
        }
        private async void BuyProduct(string productID)
        {
            MessageDialog msg;
            LicenseInformation licenseInformation = CurrentApp.LicenseInformation;
            if (!licenseInformation.ProductLicenses[productID].IsActive)
            {
                try
                {
                    await CurrentApp.RequestProductPurchaseAsync(productID);
                    if(licenseInformation.ProductLicenses[productID].IsActive)
                    {
                        msg = new MessageDialog("You have successed Donation and Remove ADS, Thank you!");
                        RemoveAds();
                    }
                    else
                    {
                        msg = new MessageDialog("You have not buy Remove ADS");
                    }
                }
                catch (Exception)
                {
                    msg = new MessageDialog("Something not right try again later, Thank you!");
                    throw;
                }
                
            }

        }
        private void RemoveAds()
        {
            //LicenseInformation licenseInformation = CurrentApp.LicenseInformation;
            //if (licenseInformation.ProductLicenses["RemoveADS"].IsActive)
            //{
            //    AdMediator_742CA8.Visibility = Visibility.Collapsed;
            //    btRemoveAds.Visibility = Visibility.Collapsed;
            //}
        }
        private void myAds_AdClick(object sender, AdDuplex.Banners.Models.AdClickEventArgs e)
        {
            AdMediator_742CA8.Visibility = Visibility.Collapsed;
        }
    }
}
