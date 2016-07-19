using System;
using System.IO;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using SQLite.Net;
using Windows.UI.Xaml.Navigation;
using DistanceTracker.Model;
using System.Collections.Generic;
using System.Linq;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DistanceTracker.myPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingP : Page
    {
        string path;
        SQLiteConnection conn;
        public SettingP()
        {
            this.InitializeComponent();
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db1.sqlite");
            txtDeleteStatus.Text = "";
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
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
        public async void ShowMessage(string msgB)
        {
            var msg = new MessageDialog(msgB);
            var cancelBtn = new UICommand("Close");
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();
        }
        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteVoid();
        }
        public async void DeleteVoid()
        {
            var msg = new MessageDialog("Delete all database?");
            var okBtn = new UICommand("Yes");
            var cancelBtn = new UICommand("No");
            msg.Commands.Add(okBtn);
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();

            if (result != null && result.Label == "Yes")
            {
                using (conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
                {
                    string deleteKilo = "Delete From KilometManager";
                    string deleteGroup = "Delete From GroupClass";
                    conn.Query<Model.KilometManager>(deleteKilo);
                    conn.Query<Model.GroupClass>(deleteGroup);
                }
            }
        }

        private async void btDeletedb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var item = cbGroup.SelectedItem;
                if (item != null)
                {
                    var msg = new MessageDialog("Delete all records of '"+ cbGroup.SelectedItem.ToString() +"' group from '"+ dateFrom.Date.ToString("dd/MM/yyyy") + "' to '"+ dateTo.Date.ToString("dd/MM/yyyy") + "' ?");
                    var okBtn = new UICommand("Yes");
                    var cancelBtn = new UICommand("No");
                    msg.Commands.Add(okBtn);
                    msg.Commands.Add(cancelBtn);
                    IUICommand result = await msg.ShowAsync();

                    if (result != null && result.Label == "Yes")
                    {
                        using (conn = new SQLite.Net.SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
                        {
                            string deleteKilo = "Delete From KilometManager Where Ngay >= '" + dateFrom.Date.ToString("yyyy-MM-dd") + "' And Ngay <= '" + dateTo.Date.ToString("yyyy-MM-dd") + "' And [Group] = '" + cbGroup.SelectedItem.ToString() + "'";
                            conn.Query<Model.KilometManager>(deleteKilo);
                            string deleteSeriesPoint = "Delete From SeriesPoint Where Ngay >= '" + dateFrom.Date.ToString("yyyy-MM-dd") + "' And Ngay <= '" + dateTo.Date.ToString("yyyy-MM-dd") + "' And [Group] = '" + cbGroup.SelectedItem.ToString() + "'";
                            conn.Query<Model.SeriesPoint>(deleteSeriesPoint);
                        }
                        txtDeleteStatus.Text = "Delete done!";
                    }
                    
                }
                else
                {
                    ShowMessage("Please select a group to delete!");
                }
                
            }
            catch (Exception)
            {

                throw;
            }
           
        }
    }
}
