using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CExcel
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
        XmlDocument worksheet = null;
        XmlDocument ShareStringSheet = null;
        private async void btnopenfile_Click(object sender, RoutedEventArgs e)
        {
            //FileOpenPicker opener = new FileOpenPicker();
            //opener.ViewMode = PickerViewMode.Thumbnail;
            //opener.FileTypeFilter.Add(".xlsx");
            //opener.FileTypeFilter.Add(".xlsm");
            //StorageFile file = await opener.PickSingleFileAsync();
            StorageFile file =await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/ExcelReading.xlsx"));
            if (file != null)
            {
                using (var fileStream = await file.OpenReadAsync())
                {
                    using (ZipArchive archive = new ZipArchive(fileStream.AsStream(), ZipArchiveMode.Read))
                    {
                        worksheet = this.GetSheet(archive, "sheet1");
                        ShareStringSheet = this.GetShareStringSheet(archive);
                    }
                }
            }
        }

        private XmlDocument GetShareStringSheet(ZipArchive archive)
        {
            XmlDocument Sharesheet = new XmlDocument();
            ZipArchiveEntry archiveEntry = archive.GetEntry("xl/sharedStrings.xml");

            using (var archiveEntryStream = archiveEntry.Open())
            {
                using (StreamReader reader = new StreamReader(archiveEntryStream))
                {
                    string xml = reader.ReadToEnd();
                    //txtresult.Text = xml;
                    Sharesheet.LoadXml(xml);
                }
            }

            return Sharesheet;
        }
        private XmlDocument GetSheet(ZipArchive archive, string sheetName)
        {
            XmlDocument sheet = new XmlDocument();
            ZipArchiveEntry archiveEntry = archive.GetEntry("xl/worksheets/" + sheetName + ".xml");

            using (var archiveEntryStream = archiveEntry.Open())
            {
                using (StreamReader reader = new StreamReader(archiveEntryStream))
                {
                    string xml = reader.ReadToEnd();

                    txtresult.Text = xml;
                    sheet.LoadXml(xml);
                }
            }

            return sheet;
        }

        private string ReadCellAddress(XmlDocument worksheet, string cellAddress)
        {
            string value = string.Empty;
            XmlElement row = worksheet.SelectSingleNodeNS("//x:c[@r='" + cellAddress + "']", "xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"") as XmlElement;
            if (row != null)
            {
                value = row.InnerText;
            }
            return value;
        }
        private string ReadValue(XmlDocument ShareStringSheet, string cellAddress)
        {
            string value = string.Empty;
            try
            {
                var nodes = ShareStringSheet.SelectNodesNS("//x:si", "xmlns:x=\"http://schemas.openxmlformats.org/spreadsheetml/2006/main\"");
                XmlElement element = nodes[Convert.ToInt32(cellAddress)] as XmlElement;

                if (element != null)
                {
                    value = element.InnerText;
                }
            }
            catch(Exception ex)
            {

            }
             
            return value;
        }

        private void btnreaddata_Click(object sender, RoutedEventArgs e)
        {
            string resultaddress = ReadCellAddress(worksheet, "B2");
            string result = ReadValue(ShareStringSheet, resultaddress);
            txtresult.Text = result;
        }
    }
}
