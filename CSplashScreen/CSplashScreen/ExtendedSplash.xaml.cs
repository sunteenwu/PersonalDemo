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
    public sealed partial class ExtendedSplash: Page
    {
        internal Rect splashImageRect; // Rect to store splash screen image coordinates.
        private SplashScreen splash; // Variable to hold the splash screen object.
        internal bool dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame rootFrame;
        private readonly DispatcherTimer _timer;
        //Make a place to store the last time the displayed item was set
        private DateTime _lastChange;
        public ExtendedSplash(SplashScreen splashscreen, bool loadState)
        {
            this.InitializeComponent();
            // Listen for window resize events to reposition the extended splash screen image accordingly.
            // This is important to ensure that the extended splash screen is formatted properly in response to snapping, unsnapping, rotation, etc...
            Window.Current.SizeChanged += new WindowSizeChangedEventHandler(ExtendedSplash_OnResize);
            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += new TypedEventHandler<SplashScreen, Object>(DismissedEventHandler);
                // Retrieve the window coordinates of the splash screen image.
                splashImageRect = splash.ImageLocation;
                PositionImage();
                // Optional: Add a progress ring to your splash screen to show users that content is loading                
            }

            // Create a Frame to act as the navigation context
            rootFrame = new Frame();       
          
            _timer = new DispatcherTimer
            {
                //Set the interval between ticks (in this case 2 seconds to see it working)
                Interval = TimeSpan.FromSeconds(2)
            };

            //Change what's displayed when the timer ticks
            _timer.Tick += ChangeImage;
            //Start the timer
            _timer.Start();
        }

        private void ChangeImage(object sender, object e)
        {
            //Get the number of items in the flip view
            var totalItems = ImageFlipView.Items.Count;
            //Figure out the new item's index (the current index plus one, if the next item would be out of range, go back to zero)
            var newItemIndex = (ImageFlipView.SelectedIndex + 1) % totalItems;
            //Set the displayed item's index on the flip view
            ImageFlipView.SelectedIndex = newItemIndex;
        }  
    

        // Position the extended splash screen image in the same location as the system splash screen image.
        void PositionImage()
        {
            ImageFlipView.Height = ContentGrid.ActualHeight;
            ImageFlipView.Width = ContentGrid.ActualWidth;
        }

        void ExtendedSplash_OnResize(Object sender, WindowSizeChangedEventArgs e)
        {
            
                PositionImage();
            
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            dismissed = true;
            // Complete app setup operations here...
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            PositionImage();
        }

        private void BtnDismissSplash_Click(object sender, RoutedEventArgs e)
        {
            rootFrame.Navigate(typeof(MainPage));    
            // Place the frame in the current Window
            Window.Current.Content = rootFrame;
        }
    }
}
