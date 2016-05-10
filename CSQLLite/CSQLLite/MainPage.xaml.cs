

using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace CSQLLite
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
           // ListCustomer.ItemsSource = SQLiteHelp.getValues();
        }

        private void BtnCreateDatabase_Click(object sender, RoutedEventArgs e)
        {
           SQLiteHelp.LoadDatabase();
           
        }   
        public class SQLiteHelp
        {
            private static string DbName = "Sun.db";

            public static void LoadDatabase()
            {
                var conn = new SQLiteConnection(DbName);
                string sql = @"Create table if not exists
                                Customer(Id Integer Primary key Autoincrement not null,
                                         Name Varchar(140),
                                         City Varchar(140),
                                         Contact Varchar(140));";
                using (var statement = conn.Prepare(sql))
                {
                    statement.Step();
                }
            }
            public static void insertData(string param1, string param2, string param3, string param4)
            {
                try
                {
                    using (var connection = new SQLiteConnection(DbName))
                    {
                        using (var statement = connection.Prepare(@"INSERT INTO Customer (ID,NAME,CITY,CONTACT)
                                    VALUES(?, ?,?,?);"))
                        {
                            statement.Bind(1, param1);
                            statement.Bind(2, param2);
                            statement.Bind(3, param3);
                            statement.Bind(4, param4);
                            // Inserts data.
                            statement.Step();
                            statement.Reset();
                            statement.ClearBindings();
                        }
                    }

                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Exception\n" + ex.ToString());
                }
            }

            public static ObservableCollection<Customer> getValues()
            {
                ObservableCollection<Customer> list = new ObservableCollection<Customer>();

                using (var connection = new SQLiteConnection(DbName))
                {
                    using (var statement = connection.Prepare(@"SELECT * FROM CUSTOMER;"))
                    {

                        while (statement.Step() == SQLiteResult.ROW)
                        {

                            list.Add(new Customer()
                            {
                                Id = Convert.ToInt32(statement[0]),
                                Name = (string)statement[1],
                                City = (string)statement[2],
                                Contact = statement[3].ToString()
                            });

                            Debug.WriteLine(statement[0] + " ---" + statement[1] + " ---" + statement[2] + statement[3]);
                        }
                    }
                }
                return list;
            }
            public static void delete(string id)
            {
                using (var connection = new SQLiteConnection(DbName))
                {
                    using (var statement = connection.Prepare(@"DELETE FROM Student WHERE ID=?"))
                    {

                        statement.Bind(1, id);
                        statement.Step();
                    }
                    //  getValues();
                }
            }

        }

    }


    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Contact { get; set; }
    }
}
