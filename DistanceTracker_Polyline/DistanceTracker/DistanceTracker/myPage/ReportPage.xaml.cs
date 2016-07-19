using DistanceTracker.Model;
using SQLite.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DistanceTracker.myPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ReportPage : Page
    {
        string path;
        SQLite.Net.SQLiteConnection conn;
        public ReportPage()
        {
            this.InitializeComponent();
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db1.sqlite");
            btViewOnMap.Visibility = Visibility.Collapsed;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SelectItem();
        }
        
        public void SelectItem()
        {
            using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
            {
                string queryString = "Select * From GroupClass";
                List<GroupClass> listKM = conn.Query<GroupClass>(queryString).ToList<GroupClass>();
                foreach (var item in listKM)
                {
                    cbGroup.Items.Add(item.Group.ToString());
                }
            }
        }
        private void btXemBaoCao_Click(object sender, RoutedEventArgs e)
        {
                var itemSelected = cbGroup.SelectedItem;

                if (itemSelected != null)
                {
                    conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);                
                    double TongCong = 0;
                    double TongCong2 = 0;
                    string queryString = "Select * From KilometManager Where Ngay >= '" + TuNgay.Date.ToString("yyyy-MM-dd") + "' And Ngay <= '" + DenNgay.Date.ToString("yyyy-MM-dd") + "' And [Group] = '" + cbGroup.SelectedItem.ToString() +"'";                
                    List<KilometManager> listKM = conn.Query<KilometManager>(queryString).ToList();
                    foreach (var item in listKM)
                    {
                        TongCong += item.SoKmDiDuoc;
                    }

                    if (TongCong == 0)
                    {
                        btViewOnMap.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        btViewOnMap.Visibility = Visibility.Visible;
                    }

                    string queryString2 = "Select * From KilometManager Where Ngay >= '" + TuNgay.Date.ToString("yyyy-MM-dd") + "' And Ngay <= '" + DenNgay.Date.ToString("yyyy-MM-dd") + "'";
                    List<KilometManager> listKM2 = conn.Query<KilometManager>(queryString2).ToList();
                    foreach (var item in listKM2)
                    {
                          TongCong2 += item.SoKmDiDuoc;                
                    }

                    txtKetQua.Text = TongCong.ToString() + " Km"  ;
                    txtTotalAllgroup.Text = TongCong2.ToString() + " Km";
                    conn.Dispose();
                    
                }
                else
                {
                    ShowMessage("Please select a group to view report");
                    btViewOnMap.Visibility = Visibility.Collapsed;
                }            
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
        public async void ShowMessage(string msgB)
        {
            var msg = new MessageDialog(msgB);
            var cancelBtn = new UICommand("Close");
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();
        }

        private void btViewOnMap_Click(object sender, RoutedEventArgs e)
        {
            string paramater = TuNgay.Date.ToString("yyyy-MM-dd") + " " + DenNgay.Date.ToString("yyyy-MM-dd") + " " + cbGroup.SelectedItem.ToString();
            this.Frame.Navigate(typeof(myPage.MapPage), paramater);
            btViewOnMap.Visibility = Visibility.Collapsed;
        }

        private void cbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btViewOnMap.Visibility = Visibility.Collapsed;
        }

        private void TuNgay_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            btViewOnMap.Visibility = Visibility.Collapsed;
        }

        private void DenNgay_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            btViewOnMap.Visibility = Visibility.Collapsed;
        }
    }
}
