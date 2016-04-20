using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
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

namespace CXML
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        Windows.Data.Xml.Dom.XmlDocument doc;
        public MainPage()
        {
            this.InitializeComponent();
            InitXML();
        }

        private async void InitXML()
        {
            try
            {
                //Create a XMLFile folder in your project and add you xml file to this folder
                doc = await LoadXmlFile("XMLFile", "CXmlC.xml");
                RichEditBoxSetMsg(ShowXMLResult, doc.GetXml(), true);
            }
            catch (Exception exp)
            {
                await new Windows.UI.Popups.MessageDialog(exp.ToString()).ShowAsync();
            }
        }

        private async void BtnXmlWrite_Click(object sender, RoutedEventArgs e)
        {
            String input1value = TxtInput.Text;
            if (null != input1value && "" != input1value)
            {
                var value = doc.CreateTextNode(input1value);
                //find input1 tag in header where id=1
                var xpath = "//header[@id='1']/user_input/input1";
                var input1nodes = doc.SelectNodes(xpath);
                for (uint index = 0; index < input1nodes.Length; index++)
                {
                    input1nodes.Item(index).AppendChild(value);
                }
                RichEditBoxSetMsg(ShowXMLResult, doc.GetXml(), true);    
            }
            else
            {
                await new Windows.UI.Popups.MessageDialog("Please type in content in the  box firstly.").ShowAsync();
            }
        }
        public static async Task<Windows.Data.Xml.Dom.XmlDocument> LoadXmlFile(String folder, String file)
        {
            Windows.Storage.StorageFolder storageFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync(folder);
            Windows.Storage.StorageFile storageFile = await storageFolder.GetFileAsync(file);
            Windows.Data.Xml.Dom.XmlLoadSettings loadSettings = new Windows.Data.Xml.Dom.XmlLoadSettings();
            return await Windows.Data.Xml.Dom.XmlDocument.LoadFromFileAsync(storageFile, loadSettings);
        }
        public static void RichEditBoxSetMsg(RichEditBox richEditBox, String msg, bool fReadOnly)
        {
            richEditBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, msg);
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            var file = await Windows.Storage.ApplicationData.Current.LocalFolder.CreateFileAsync("CXmlCopy.xml", Windows.Storage.CreationCollisionOption.GenerateUniqueName);
            await doc.SaveToFileAsync(file);
            RichEditBoxSetMsg(ShowXMLResult, "Save to \"" + file.Path + "\" successfully.", true);
        }
    }
}
