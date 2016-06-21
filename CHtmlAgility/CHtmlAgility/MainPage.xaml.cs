using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CHtmlAgility
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

        private async void parsetable_Click(object sender, RoutedEventArgs e)
        {
            WebRequest request = HttpWebRequest.Create("http://www.eurogymnasium-waldenburg.de/egw_content/Stunden_Vertretungsplan/home.html");
            WebResponse response = await request.GetResponseAsync();
            Stream stream = response.GetResponseStream();
            var result = "";
            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadToEnd();
            }
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(result);
            var node = htmlDoc.DocumentNode.SelectSingleNode("/html/body[@class='ui-widget']/div[@id='main']/div[@id='vplan']/div[@id='bereichaktionen']");
            System.Diagnostics.Debug.WriteLine(node.Name);           
 
        }
    }
}
