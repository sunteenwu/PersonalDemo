using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CPathAnimation
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        //private MapShapeLayer shapeLayer;

        private Geopath path = new Geopath(new List<BasicGeoposition>(){
            new BasicGeoposition()
            {
                Latitude=42.8, Longitude=12.9
            } ,   //Italy
           new BasicGeoposition()
            {
                Latitude=51.5, Longitude=0
            } ,      //London
           new BasicGeoposition()
            {
                Latitude=40.8, Longitude=-73.9
            } ,   //New York
           new BasicGeoposition()
            {
                Latitude=47.68, Longitude=-122.3
            }
        }
        );
        private PathAnimation currentAnimation;

       
        public MainPage()
        {
            this.InitializeComponent();        
        }
     
        private void ClearMapBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ClearMap();
        }

        private void DropPinBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {

            Image forImage = new Image();
            forImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pin.png"));
            MapControl.SetLocation(forImage, MyMap.Center);
            MapItems.Items.Add(forImage);
            PushpinAnimations.Drop(forImage, null, null);
        }

        private void BouncePinBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Image forImage = new Image();
            forImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pin.png"));
            MapControl.SetLocation(forImage, MyMap.Center);
            MapItems.Items.Add(forImage);
            PushpinAnimations.Bounce(forImage, null, null);
        }

        private async void Bounce4PinsBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            for (var i = 0; i < path.Positions.Count; i++)
            {
                Image forImage = new Image();
                forImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pin.png"));

                MapControl.SetLocation(forImage, new Geopoint(path.Positions[i]));
                MapItems.Items.Add(forImage);
                PushpinAnimations.Bounce(forImage, null, null);
                await Task.Delay(500);
            }
        }

        private void MovePinOnPathBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MovePinOnPath(false);
        }

        private void MovePinOnGeodesicPathBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MovePinOnPath(true);
        }

        private void MoveMapOnPathBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MoveMapOnPath(false);
        }

        private void MoveMapOnGeodesicPathBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MoveMapOnPath(true);
        }

        private void DrawPathBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DrawPath(false);
        }

        private void DrawGeodesicPathBtn_Tapped(object sender, TappedRoutedEventArgs e)
        {
            DrawPath(true);
        }

        private void ClearMap()
        {
            //MyMap.Children.Clear();
            MapItems.Items.Clear();
        }

        private void MovePinOnPath(bool isGeodesic)
        {
            Image forImage = new Image();
            forImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/pin.png"));
            MapControl.SetLocation(forImage, new Geopoint(path.Positions[0]));
            MapItems.Items.Add(forImage);

            currentAnimation = new PathAnimation(path, (coord, pathIdx, frameIdx) =>
           {
               MapControl.SetLocation(forImage, new Geopoint(coord));
           }, isGeodesic, 10000);

            currentAnimation.Play();
        }

        private void MoveMapOnPath(bool isGeodesic)
        {
            ClearMap();

            //Change zooms levels as map reaches points along path.
            int[] zooms = new int[4] { 5, 4, 6, 5 };
            MyMap.ZoomLevel = zooms[0];
            MyMap.Center = new Geopoint(path.Positions[0]);

            currentAnimation = new PathAnimation(path, (coord, pathIdx, frameIdx) =>
           {
               MyMap.ZoomLevel = zooms[pathIdx];
               MyMap.Center = new Geopoint(coord);
           }, isGeodesic, 10000);

            currentAnimation.Play();
        }
        private void DrawPath(bool isGeodesic)
        {
            ClearMap();

            MapPolyline line = new MapPolyline();
            line.StrokeColor = Colors.Red;
            line.StrokeThickness = 4;
            List<BasicGeoposition> templine = new List<BasicGeoposition>();

            currentAnimation = new PathAnimation(path, (coord, pathIdx, frameIdx) =>
           {
               if (frameIdx == 1)
               {
                   //Create the line after the first frame so that we have two points to work with.	                
                   templine.Add(path.Positions[0]);
                   templine.Add(coord);
                   line.Path = new Geopath(templine);

                   MyMap.MapElements.Add(line);
                   
               }
               else if (frameIdx > 1)
               {
                   templine.Add(coord);
                   line.Path= new Geopath(templine);
                   MyMap.MapElements.Add(line);
               }
           }, isGeodesic, 10000);

            currentAnimation.Play();
        }
         
    }
}
