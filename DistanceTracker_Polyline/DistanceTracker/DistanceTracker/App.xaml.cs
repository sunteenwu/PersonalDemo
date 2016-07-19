using System;
using System.IO;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.Foundation.Metadata;
using Windows.UI;
using System.Threading.Tasks;
using SQLite.Net;
using DistanceTracker.Model;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;

namespace DistanceTracker
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            this.InitializeComponent();
            this.Suspending += OnSuspending;
           // HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }
        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>       
        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            AdDuplex.AdDuplexClient.Initialize("8a0747cf-f0e7-4475-92e7-3a4beb01a9ba");
            //#if DEBUG
            //            if (System.Diagnostics.Debugger.IsAttached)
            //            {
            //                this.DebugSettings.EnableFrameRateCounter = true;
            //            }
            //#endif            
            string path;
            SQLiteConnection conn;
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db1.sqlite");
            conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);            
            if (await FileExists("db1.sqlite"))
            {
                conn.CreateTable<KilometManager>();
                conn.CreateTable<GroupClass>();
                conn.CreateTable<SeriesPoint>();
            }
            if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar"))
            {

                var statusBar = StatusBar.GetForCurrentView();
                if (statusBar != null)
                {
                    statusBar.BackgroundOpacity = 1;
                    statusBar.BackgroundColor = Colors.Black;
                    statusBar.ForegroundColor = Colors.White;
                }
            }


            Frame rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(MainPage), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
        private async Task<bool> FileExists(string fileName)
        {
            var result = false;
            try
            {
                var store = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                result = true;
            }
            catch { }
            return result;
        }
        private async void HardwareButtons_BackPressed(object sender, Windows.Phone.UI.Input.BackPressedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;
            e.Handled = true;
            if (rootFrame.CanGoBack)
            {
                rootFrame.GoBack();
            }
            else
            {
                var msg = new MessageDialog("Are you sure to exit?");
                var okBtn = new UICommand("Yes");
                var cancelBtn = new UICommand("No");
                msg.Commands.Add(okBtn);
                msg.Commands.Add(cancelBtn);
                IUICommand result = await msg.ShowAsync();

                if (result != null && result.Label == "Yes")
                {
                    Application.Current.Exit();
                }
            }
        }
    }
}
