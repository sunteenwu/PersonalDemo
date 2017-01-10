using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CHub
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Page_Loaded(object sender, RoutedEventArgs e)
        {
            StorageFile jsonfile = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/test.json"));
            string jsonString = await FileIO.ReadTextAsync(jsonfile);
            JsonObject jsonObject = JsonObject.Parse(jsonString);
            //Read json file, and deserialization the json string to Hubcontrol class.
            JsonArray Allhubs = jsonObject.GetNamedArray("Hubs");
            List<Hubcontrol> hubsources = new List<Hubcontrol>();
            foreach (IJsonValue jsonValue in Allhubs)
            {
                if (jsonValue.ValueType == JsonValueType.Object)
                {
                    JsonObject hubcontrolitem = jsonValue.GetObject();
                    hubsources.Add(new Hubcontrol()
                    {
                        Title = hubcontrolitem.GetNamedString("Title"),
                        Length = hubcontrolitem.GetNamedString("Length"),
                        Features = hubcontrolitem.GetNamedString("Features")
                    });
                }
            }
            //Create a new hub control, add hubsections which title is got from json
            Hub HubFromJson = new Hub();
            foreach (Hubcontrol hubcontrolitem in hubsources)
            {
                HubSection sectionitem = new HubSection();
                sectionitem.Header = hubcontrolitem.Title;
                HubFromJson.Sections.Add(sectionitem);
            }
            root.Children.Add(HubFromJson);
        }
    }

    public class Hubcontrol
    {
        public string Title { get; set; }
        public string Length { get; set; }
        public string Features { get; set; }
    }
}
