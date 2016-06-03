using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CSplashScreen
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public partial class ExtendedSplash 
    {
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;
        private readonly DispatcherTimer _timer;

        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            this.InitializeComponent();
            StatusBar statusbar = StatusBar.GetForCurrentView();
              statusbar.HideAsync();
            if (splash != null)
            {
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);
            }
            rootFrame = new Frame();
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(2)
            };
            _timer.Tick += ChangeImage;
            _timer.Start();
        }

        private void ChangeImage(object sender, object e)
        {
            var totalItems = ImageFlipView.Items.Count;
            var newItemIndex = (ImageFlipView.SelectedIndex + 1) % totalItems;
            ImageFlipView.SelectedIndex = newItemIndex;
        }
        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;            
        }
        private void BtnDismissSplash_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(MainPage));
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }
    }
}
