using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CListViewNavigate
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            List<Images> itemsources = new List<Images>
            {
                new Images {ImageName="img1",ImageUrl="Assets/caffe1.jpg" },
                new Images {ImageName="img1" ,ImageUrl="Assets/caffe2.jpg" },
                new Images {ImageName="img1",ImageUrl="Assets/caffe3.jpg"  }
            };
            listImage.ItemsSource = itemsources;
        }

        private void Image_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            Image selectedimage = e.OriginalSource as Image;
            Images select = (Images)selectedimage.DataContext;
            Frame.Navigate(typeof(ShowImage),select);
        }
    }
    public class Images
    {
        public string ImageName { get; set; }



        public string ImageUrl { get; set; }
    }
}
