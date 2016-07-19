using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite.Net;
using DistanceTracker.Model;
using Windows.UI;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DistanceTracker.myPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        Library library = new Library();
        string path;
        SQLiteConnection conn;
        public MapPage()
        {
            this.InitializeComponent();
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db1.sqlite");
        }
        MapIcon mapIcon1 = new MapIcon();
        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:
                    Geolocator geolocator = new Geolocator();
                    Geoposition position = await geolocator.GetGeopositionAsync();
                    Geopoint myLocation = position.Coordinate.Point;


                    mapIcon1.Location = myLocation;
                    mapIcon1.NormalizedAnchorPoint = new Point(0.5, 1.0);
                    mapIcon1.Title = "My Position";
                    mapIcon1.ZIndex = 0;

                    myMapControl.MapElements.Add(mapIcon1);

                    myMapControl.Center = myLocation;
                    myMapControl.ZoomLevel = 14;
                    myMapControl.LandmarksVisible = true;

                    break;
                case GeolocationAccessStatus.Denied:
                    break;
                case GeolocationAccessStatus.Unspecified:
                    break;
            }

            var selectedParamater = e.Parameter as string;
            string fromDate = selectedParamater.Substring(0, 10);
            string toDate = selectedParamater.Substring(11, 10);
            string groupName = selectedParamater.Substring(22);
            using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
            {
                string selectStr = "Select * From KilometManager Where Ngay >= '" + fromDate + "' And Ngay <= '" + toDate + "' And [Group] = '" + groupName + "'";
                List<KilometManager> listKM = conn.Query<KilometManager>(selectStr).ToList<KilometManager>();
                foreach (var item in listKM)
                {
                    int Id = item.Id;
                    if (item.SoKmDiDuoc != 0)
                    {
                        myListViewControl.Items.Add(item);
                    }
                }

            }

        }
        List<BasicGeoposition> listPoint = new List<BasicGeoposition>();
        List<MapPolyline> mapPolylineList = new List<MapPolyline>();  
        MapPolyline line = new MapPolyline();
        private void CreatePolyLine(int Id)
        {
            listPoint.Clear();          
            if (Id != 0)
            {
                using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
                {
                    string selectStr = "Select * From SeriesPoint Where Id = " + Id;
                    List<SeriesPoint> listKM = conn.Query<SeriesPoint>(selectStr).ToList<SeriesPoint>();
                    foreach (var item in listKM)
                    {
                        listPoint.Add(new BasicGeoposition() { Longitude = item.Longtitude, Latitude = item.Latitude });
                    }
                    line.Path = new Geopath(listPoint);
                    line.StrokeThickness = 5;
                    myMapControl.MapElements.Add(line);
                    mapPolylineList.Add(line);
                }
            }
            else
            {
                myMapControl.MapElements.Remove(line);
            }
        }
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {            
            foreach (var item in mapPolylineList)
            {
                myMapControl.MapElements.Remove(item);
            }
            mapPolylineList.Clear();
        }
        private void myListViewControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedId = myListViewControl.SelectedItem as KilometManager;


            CreatePolyLine(selectedId.Id);
        }


    }
}
