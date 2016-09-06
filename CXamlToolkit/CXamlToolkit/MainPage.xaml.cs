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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CXamlToolkit
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Random _random = new Random();
        public MainPage()
        {
            this.InitializeComponent();
            var items1 = new List<NameValueItem>();
            var items2 = new List<NameValueItem>();
            var items3 = new List<NameValueItem>();
            for (int i = 0; i < 3; i++)
            {
                items1.Add(new NameValueItem { Name = "Name" + i, Value = _random.Next(10, 100) });   
            }
            this.RunIfSelected(this.LineChart, () => ((LineSeries)this.LineChart.Series[0]).ItemsSource = items1);     

        }


        private void RunIfSelected(UIElement element, Action action)
        {
            action.Invoke();
        }

        private void LineSeries_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (line1.SelectedItem != null)
            {
                NameValueItem seleteditem = line1.SelectedItem as NameValueItem;
                System.Diagnostics.Debug.WriteLine(seleteditem.Name);
                System.Diagnostics.Debug.WriteLine(seleteditem.Value);
            }

        }
    }
    public class NameValueItem
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }

}
