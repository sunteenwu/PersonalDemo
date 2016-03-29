using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CListeView
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;       
        ObservableCollection<Employee> Employeestargelist = new ObservableCollection<Employee>
        {            
        };
        ObservableCollection<Employee> Employeeslist = new ObservableCollection<Employee>
        {
            new Employee() {Id=1,Name="employee1"},
            new Employee() {Id=2,Name="employee2"},
            new Employee() {Id=3,Name="employee3"},
            new Employee() {Id=4,Name="employee4"},
        };
        public MainPage()
        {
            this.InitializeComponent();         
        }

        private void StatusCB_Checked(object sender, RoutedEventArgs e)
        {
            var checkbox = sender as CheckBox;
            if(checkbox!=null)
            {
                var employee =(Employee)checkbox.DataContext;
                Employeeslist.Remove(employee);
                Employeestargelist.Add(employee);
            }           
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }

    }
    public class Goods
    {
        string goodsname;
        double price;
        bool isSelected = false;

        public Goods()
        {
        }

        public Goods(String good)
        {
            this.goodsname = good;
        }

        public string Goodsname
        {
            get
            {
                return goodsname;
            }

            set
            {
                goodsname = value;
            }
        }

        public double Price
        {
            get
            {
                return price;
            }

            set
            {
                price = value;
            }
        }

        public bool IsSelected
        {
            get
            {
                return isSelected;
            }

            set
            {
                isSelected = value;
            }
        }
    }
}
