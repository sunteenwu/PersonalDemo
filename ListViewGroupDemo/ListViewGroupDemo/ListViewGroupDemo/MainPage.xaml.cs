using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ListViewGroupDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string st;

        public MainPage()
        {
            this.InitializeComponent();
            ContactsCVS.Source = Contact.GetContactsGrouped(250);

            MyListView.SizeChanged += (s, e) =>
            {
                ScrollViewer sv = FindVisualChildren<ScrollViewer>(MyListView).FirstOrDefault();
                if (sv != null)
                {
                    sv.ViewChanged += (ss, ee) =>
                    {
                        IEnumerable<TextBlock> tblocks = FindVisualChildren<TextBlock>(MyListView).Where(x => x.Name == "HeaderTextBlock");
                        if (tblocks != null)
                        {
                            foreach (TextBlock tblock in tblocks)
                            {
                                if (tblock.Text == st)
                                {
                                    tblock.Foreground = new SolidColorBrush(Colors.Red);
                                }
                                else
                                {
                                    tblock.Foreground = new SolidColorBrush(Colors.Black);
                                }

                                if (IsVisibileToUser(tblock, sv))
                                {
                                    st = tblock.Text;
                                    break;
                                }
                            }
                        }
                    };
                }
            };
        }

        private static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        //private bool IsVisibileToUser(FrameworkElement element, FrameworkElement container)
        //{
        //    if (element == null || container == null)
        //        return false;

        //    if (element.Visibility != Visibility.Visible)
        //        return false;

        //    Rect elementBounds = element.TransformToVisual(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
        //    Rect containerBounds = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

        //    return (elementBounds.Top < containerBounds.Bottom && elementBounds.Bottom > containerBounds.Top);
        //}

        private bool IsVisibileToUser(FrameworkElement element, FrameworkElement container)
        {
            if (element == null || container == null)
                return false;

            if (element.Visibility != Visibility.Visible)
                return false;

            Rect elementBounds = element.TransformToVisual(container).TransformBounds(new Rect(0.0, 0.0, element.ActualWidth, element.ActualHeight));
            Rect containerBounds = new Rect(0.0, 0.0, container.ActualWidth, container.ActualHeight);

            return (elementBounds.Top <= element.ActualHeight && elementBounds.Bottom > containerBounds.Top);
        }
    }
}