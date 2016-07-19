using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using DistanceTracker.Model;
using System.Collections.ObjectModel;
using SQLite.Net;
using System.Threading.Tasks;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace DistanceTracker.myPage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NewGroupPage : Page
    {
        string path;
        SQLiteConnection conn;
        ObservableCollection<GroupClass> myGroup = new ObservableCollection<GroupClass>();
        public NewGroupPage()
        {
            this.InitializeComponent();
            path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db1.sqlite");
            SelectItem();       
        }
        public void SelectItem()
        {
            using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
            {
                string queryString = "Select * From GroupClass";
                List<GroupClass> listKM = conn.Query<GroupClass>(queryString).ToList<GroupClass>();
                myList.ItemsSource = listKM;
            }
        }
        public async void AddNewItem()
        {
                using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
                {
                    bool exist = false;
                    var inputItem = txtName.Text.ToString();
                    string queryString = "Select * From GroupClass";
                    List<GroupClass> listKM = conn.Query<GroupClass>(queryString).ToList<GroupClass>();
                    foreach (var item in listKM)
                    {
                        if (item.Group.ToString() == inputItem)
                        {
                            exist = true;
                        }
                    }
                    if (exist == false)
                    {
                        conn.Insert(new GroupClass() { Group = txtName.Text.ToString() });
                    }
                    else
                    {
                        var msg = new MessageDialog("The name '" + inputItem + "' exist in Database!");
                        var cancelBtn = new UICommand("Close");
                        msg.Commands.Add(cancelBtn);
                        IUICommand result = await msg.ShowAsync();
                        txtName.Text = "";
                    }
                    SelectItem();
                }
        }
        public void UpdateItem(int id, string Value,string oldValue)
        {            
            using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
            {
                        string updateString = "UPDATE GroupClass SET [Group] = '" + Value + "' WHERE [Group] = '" + oldValue + "'";
                        string updateString2 = "UPDATE KilometManager SET [Group] = '" + Value + "' WHERE [Group] = '" + oldValue + "'";
                        conn.Execute(updateString);
                        conn.Execute(updateString2);               
            }
            txtName.Text = "";
            SelectItem();
        }
        public async void DeleteItem(int id,string delGroup)
        {
            var inputItem = txtName.Text.ToString();
            var msg = new MessageDialog("Delete all records in this group too, Are you sure?");
            var okBtn = new UICommand("Yes");
            var cancelBtn = new UICommand("No");
            msg.Commands.Add(okBtn);
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();
            if (result != null && result.Label == "Yes")
            {
                using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
                {
                    string queryString = "Delete From GroupClass Where Id = " + id;
                    conn.Delete<GroupClass>(id);                    
                    string queryString2 = "Delete From KilometManager Where [Group] = '" + delGroup + "'";
                    conn.Query<KilometManager>(queryString2);
                }
            }            
            SelectItem();
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

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            if(txtName.Text != "")
            {
                AddNewItem();
                txtName.Text = "";
            }
            else
            {
                ShowMessage("Name of group cannot be null!");
            }
        }
        private void myList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectItem = myList.SelectedItem as GroupClass;
            if (selectItem != null)
            {
                txtName.Text = selectItem.Group.ToString();                
            }
        }

        private void btUpdate_Click(object sender, RoutedEventArgs e)
        {           
            if(txtName.Text != "")
            {
                var selectedID = myList.SelectedItem as GroupClass;
                bool exit = false;
                using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
                {
                    string selecteTB = "Select * From GroupClass";
                    List<GroupClass> listGr = conn.Query<GroupClass>(selecteTB).ToList();
                    foreach (var item in listGr)
                    {
                        if (item.Group == txtName.Text)
                        {
                            ShowMessage("This name '" + txtName.Text + "' exist in Database!");
                            exit = true;
                        }
                    }
                    if (exit == false)
                    {
                        UpdateItem(selectedID.Id, txtName.Text, selectedID.Group);
                        txtName.Text = "";
                    }
                }
            }
            else
            {
                ShowMessage("Name can not be null!");
            }
        }

        private void btDelete_Click(object sender, RoutedEventArgs e)
        {
            if(txtName.Text=="")
            {
                ShowMessage("Please select a group to Delete!");
            }
            else
            {
                var selectedID = myList.SelectedItem as GroupClass;
                DeleteItem(selectedID.Id, selectedID.Group);
                txtName.Text = "";
            }
        }

        private void btGoTracking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool exist = false;
                if (txtName.Text != "")
                {                    
                    var selectedGroup = myList.SelectedItem as GroupClass;
                    using (conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path))
                    {
                        
                        string queryString = "Select * From GroupClass";
                        List<GroupClass> listKM = conn.Query<GroupClass>(queryString).ToList<GroupClass>();
                        foreach (var item in listKM)
                        {
                            if (item.Group == txtName.Text)
                            {
                                exist = true;                                                                                          
                            }                                                      
                        }
                        if(exist == true)
                        {
                            this.Frame.Navigate(typeof(GPS_Tracker), selectedGroup.Group);
                        }
                        else
                        {
                            ShowMessage("Please select a group to Begin tracking!");
                        }

                    }
                }
                else
                {
                    ShowMessage("Please select a group to Begin tracking!");
                }
            }
            catch (Exception)
            {
            }
            
            
        }
        public async void ShowMessage(string msgB)
        {
            var msg = new MessageDialog(msgB);
            var cancelBtn = new UICommand("Close");
            msg.Commands.Add(cancelBtn);
            IUICommand result = await msg.ShowAsync();
        }

       
    }
}
