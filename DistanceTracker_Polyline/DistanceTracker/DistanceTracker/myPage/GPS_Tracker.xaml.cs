using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using DistanceTracker.Model;
using System.Globalization;
using SQLite.Net;
using Windows.ApplicationModel.ExtendedExecution;
using Windows.UI.Xaml.Controls.Maps;
using Windows.Foundation;
using Windows.Services.Maps;
using System.Collections.Generic;
using System.Linq;



// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DistanceTracker.myPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class GPS_Tracker : Page
    {
        public Library Library = new Library();        
        string path;        
        SQLiteConnection conn;
        private ExtendedExecutionSession session;
        int lastId = 0;

        List<SeriesPoint> listSeriesPoint = new List<SeriesPoint>();

        public GPS_Tracker()
        {   
            this.InitializeComponent();
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db1.sqlite");
            txtKetQua.Text = "";
            btLuu.IsEnabled = false;
        }
        private async void StartLocationExtensionSession()
        {
            StopLocationExtensionSession();
            session = new ExtendedExecutionSession();
            session.Description = "LocationTracking";
            session.Reason = ExtendedExecutionReason.LocationTracking;
            session.Revoked += Session_Revoked;
            var result = await session.RequestExtensionAsync();
            //if (result==ExtendedExecutionResult.Allowed)
            //{
                
            //}
        }
        private void StopLocationExtensionSession()
        {
            if (session != null)
            {
                session.Dispose();
                session = null;
            }
        }
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            StopLocationExtensionSession();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var value = e.Parameter as string;
            txtGroup.Text = value.ToString();
            if(this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
        private void Session_Revoked(object sender, ExtendedExecutionRevokedEventArgs args)
        {
            //StopLocationExtensionSession();
            StartLocationExtensionSession();
        }

        public async Task<Geopoint> Position()
        {
            return (await new Geolocator().GetGeopositionAsync()).Coordinate.Point;
        }
        
        
        private async Task GetPositionAndShowPoint()
        {   
            Geopoint point = await Library.Position();    
            if(Double.IsNaN(point.Position.Longitude)||Double.IsNaN(point.Position.Latitude))
            {

            }   
            else
            {
                firstlong = point.Position.Longitude;
                firstla = point.Position.Latitude;
            }    
            
        }           
        Geolocator geolocator = null;
        bool tracking = false;       
        private async void btBatDau_Click(object sender, RoutedEventArgs e)
        {
            if (!tracking)
            {
                txtDistance.Text = "";
                StartLocationExtensionSession();
                txtStatus.Text = "Getting your location";
                distance = 0;
                await GetPositionAndShowPoint();
                geolocator = new Geolocator();
                geolocator.DesiredAccuracy = PositionAccuracy.High;
                geolocator.MovementThreshold = 100;
                geolocator.StatusChanged += geolocator_StatusChanged;
                geolocator.PositionChanged += geolocator_PositionChanged;
                tracking = true;
                btBatDau.Content = "Stop";                                
                btLuu.IsEnabled = false;
                txtKetQua.Text = "";
                SelectItem();
            }
            else
            {
                StopLocationExtensionSession();
                geolocator.StatusChanged -= geolocator_StatusChanged;
                geolocator.PositionChanged -= geolocator_PositionChanged;
                geolocator = null;
                tracking = false;
                btBatDau.Content = "Start";
                txtStatus.Text = "";
                btLuu.IsEnabled = true;
                
                if (txtDistance.Text=="")
                {
                    btLuu.IsEnabled = false;
                }
            }            
        }
        double firstlong = 0;
        double firstla = 0;
        double distance = 0;
        private async void geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
           {
               double newlong = args.Position.Coordinate.Longitude;
               double newla = args.Position.Coordinate.Latitude;
               if (Double.IsNaN(newlong) || Double.IsNaN(newla))
               {
                   //Geopoint point = await Library.Position();
               }
               else
               {
                   distance += DistanceTo(firstla, firstlong, newla, newlong, 'K');
                   firstla = newla;
                   firstlong = newlong;
                   distance = Math.Round(distance, 2);
                   txtDistance.Text = distance.ToString();
                   listSeriesPoint.Add(new SeriesPoint { Group = txtGroup.Text, Longtitude = firstlong, Latitude = firstla, Ngay = DateTime.Now.Date.ToString("yyyy-MM-dd"), Id = lastId });
               }
           });

        }
        static public double DistanceTo(double lat1, double lon1, double lat2, double lon2, char unit = 'K')
        {
            double rlat1 = Math.PI * lat1 / 180;
            double rlat2 = Math.PI * lat2 / 180;
            double theta = lon1 - lon2;
            double rtheta = Math.PI * theta / 180;
            double dist = Math.Sin(rlat1) * Math.Sin(rlat2) + Math.Cos(rlat1) * Math.Cos(rlat2) * Math.Cos(rtheta);
            dist = Math.Acos(dist);
            dist = dist * 180 / Math.PI;
            dist = dist * 60 * 1.1515;
            switch (unit)
            {
                case 'K': //Kilometers -> default
                    return dist * 1.609344;
                case 'N': //Nautical Miles 
                    return dist * 0.8684;
                case 'M': //Miles
                    return dist;
            }
            return dist;
        }

        private async void geolocator_StatusChanged(Geolocator sender, StatusChangedEventArgs args)
        {
            string status = "";
            await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                switch (args.Status)
                {
                    case PositionStatus.Disabled:
                        // the application does not have the right capability or the location master switch is off
                        status = "Location is off";
                        break;
                    case PositionStatus.Initializing:
                        // the geolocator started the tracking operation
                        status = "Getting your location";                        
                        break;
                    case PositionStatus.NoData:
                        // the location service was not able to acquire the location
                        status = "No data";
                        break;
                    case PositionStatus.Ready:
                        // the location service is generating geopositions as specified by the tracking parameters
                        status = "Tracking";
                        break;
                    case PositionStatus.NotAvailable:
                        status = "Not avalable position";
                        // not used in WindowsPhone, Windows desktop uses this value to signal that there is no hardware capable to acquire location information
                        break;
                    case PositionStatus.NotInitialized:
                        // the initial state of the geolocator, once the tracking operation is stopped by the user the geolocator moves back to this state

                        break;
                }
                txtStatus.Text = status;               
            });
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
       
        private void btLuu_Click(object sender, RoutedEventArgs e)
        {   
            var toDay = DateTime.Now.ToString("yyyy-MM-dd");   
            using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
            {             
                conn.Insert(new KilometManager() { Ngay = toDay, SoKmDiDuoc = double.Parse(txtDistance.Text), Group = txtGroup.Text.ToString() });                                
                foreach (var item in listSeriesPoint)
                {
                    //string insertPoint = "Insert into SeriesPoint Values ("+item.Id+","+item.Longtitude+","+item.Latitude+","+item.Ngay+",["+item.Group+"])";
                   // conn.Query<SeriesPoint>(insertPoint);
                    conn.Insert(new SeriesPoint() { Id = item.Id, Longtitude = item.Longtitude, Latitude = item.Latitude, Ngay = item.Ngay, Group = item.Group });
                }
                conn.Dispose();
            }
            txtKetQua.Text = DateTime.Now.Date.ToString("dd/MM/yyyy") + ": " + txtDistance.Text + " Km";
            txtDistance.Text = "";
            btLuu.IsEnabled = false;
            StopLocationExtensionSession();
        }
        public void SelectItem()
        {
            using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
            {
                string queryString = "Select * From KilometManager Where Id = (Select Max(Id) From KilometManager)";
                List<GroupClass> listKM = conn.Query<GroupClass>(queryString).ToList<GroupClass>();
                foreach (var item in listKM)
                {
                    lastId = item.Id + 1;
                }
            }
        }
    }
}

