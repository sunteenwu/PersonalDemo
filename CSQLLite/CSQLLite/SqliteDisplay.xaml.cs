using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace CSQLLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SqliteDisplay : Page
    {
        public SqliteDisplay()
        {
            this.InitializeComponent();
           // SQLiteHelp.LoadDatabase();
            Database();
        }
        public static string DB_Name = "kamus.db";

        private ObservableCollection<ind_dict> indDatasource = new ObservableCollection<ind_dict>();

        public string TABLE_NAME_IND { get; private set; }

        private async void Database()
        {
            bool flag = await FileExistsAsync(DB_Name);
            if (!flag)
            {
                var assetsFolder = await Package.Current.InstalledLocation.GetFolderAsync("Assets");
                var dbFile = await assetsFolder.GetFileAsync(DB_Name);
                await dbFile.CopyAsync(Windows.Storage.ApplicationData.Current.LocalFolder);
            }
        }

        private void inputText_KeyUp(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                //if (indoMenu.IsChecked == true)
                //{
                string queryText = inputText.Text;
                if (!string.IsNullOrEmpty(queryText))
                {
                    ReadFileIndo();
                }
                //}
            }
        }

        private async void ReadFileIndo()
        {
            string key = string.Empty;
            TABLE_NAME_IND = "ind_dict";
            ind_dict Ind = new ind_dict();
            string value = String.Empty;
            SQLiteConnection _connection = new SQLiteConnection(Path.Combine(Package.Current.InstalledLocation.Path, @"Assets\kamus.db"));
            //SQLiteConnection _connection = new SQLiteConnection("kamus.db");
            using (var statement = _connection.Prepare(@"SELECT word, translation FROM " + TABLE_NAME_IND + " WHERE word='" + inputText.Text + "'"))
            {
                //statement.Bind(1, Ind.Word);
                //statement.Bind(2, Ind.Translation);
                SQLiteResult result = statement.Step();
                if (SQLiteResult.ROW == result)
                {
                    Ind.Word = statement[0] as String;
                    Ind.Translation = statement[1] as String;
                    indDatasource.Add(Ind);
                    if (indDatasource.Count > 0)
                    {
                        translation.ItemsSource = indDatasource;
                    }
                }
            }
        }
        public async Task<bool> FileExistsAsync(string fileName)
        {
            try
            {
                await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);
                return true;
            }
            catch (FileNotFoundException)
            {
                return false;
            }
        }
    }    
    public class ind_dict
    {
        //[SQLite.Net.Attributes.PrimaryKey]
        public string Id { get; set; }
        public string Word { get; set; }
        public string Translation { get; set; }

        public ind_dict()
        {

        }
        public ind_dict(string word, string translation)
        {
            Word = word;
            Translation = translation;
        }
    }
    //SqlHelp Example
    //public class SQLiteHelp
    //{
    //    private static string DbName = "kamus.db";
    //    public static void LoadDatabase()
    //    {
    //        var conn = new SQLiteConnection(DbName);
    //        string sql = @"Create table if not exists
    //                            Customer(Id Integer Primary key Autoincrement not null,
    //                                     Name Varchar(140),
    //                                     City Varchar(140),
    //                                     Contact Varchar(140));";
    //        using (var statement = conn.Prepare(sql))
    //        {
    //            statement.Step();
    //        }
    //    }
    //    public static void insertData(string param1, string param2, string param3, string param4)
    //    {
    //        try
    //        {
    //            using (var connection = new SQLiteConnection(DbName))
    //            {
    //                using (var statement = connection.Prepare(@"INSERT INTO Customer (ID,NAME,CITY,CONTACT)
    //                                VALUES(?, ?,?,?);"))
    //                {
    //                    statement.Bind(1, param1);
    //                    statement.Bind(2, param2);
    //                    statement.Bind(3, param3);
    //                    statement.Bind(4, param4);
    //                    // Inserts data.
    //                    statement.Step();
    //                    statement.Reset();
    //                    statement.ClearBindings();
    //                }
    //            }

    //        }
    //        catch (Exception ex)
    //        {
    //            Debug.WriteLine("Exception\n" + ex.ToString());
    //        }
    //    }

    //    public static ObservableCollection<Customer> getValues()
    //    {
    //        ObservableCollection<Customer> list = new ObservableCollection<Customer>();

    //        using (var connection = new SQLiteConnection(DbName))
    //        {
    //            using (var statement = connection.Prepare(@"SELECT * FROM CUSTOMER;"))
    //            {

    //                while (statement.Step() == SQLiteResult.ROW)
    //                {

    //                    list.Add(new Customer()
    //                    {
    //                        Id = Convert.ToInt32(statement[0]),
    //                        Name = (string)statement[1],
    //                        City = (string)statement[2],
    //                        Contact = statement[3].ToString()
    //                    });

    //                    Debug.WriteLine(statement[0] + " ---" + statement[1] + " ---" + statement[2] + statement[3]);
    //                }
    //            }
    //        }
    //        return list;
    //    }
    //    public static void delete(string id)
    //    {
    //        using (var connection = new SQLiteConnection(DbName))
    //        {
    //            using (var statement = connection.Prepare(@"DELETE FROM Student WHERE ID=?"))
    //            {

    //                statement.Bind(1, id);
    //                statement.Step();
    //            }
    //            //  getValues();
    //        }
    //    }

    //}
}
