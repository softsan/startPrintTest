using StarIoWrapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace testStarSdk
{
    interface IListViewItem
    {
        string Text { get; }

        string DetailText { get; }

        Color TextColor { get; }

        Color DetailTextColor { get; }
    }

    class ListViewItemCollection : ObservableCollection<IListViewItem>
    {
        public string Header { get; }

        public ListViewItemCollection(string header)
        {
            this.Header = header;
        }

      
    }

    public partial class MainPage : ContentPage
    {
        private ListViewItemCollection listViewItemCollection;


        private ObservableCollection<ListViewItemCollection> observableCollection;

        public MainPage()
        {
            InitializeComponent();
            this.observableCollection = new ObservableCollection<ListViewItemCollection>();
            this.listViewItemCollection = new ListViewItemCollection("List");
            this.observableCollection.Add(this.listViewItemCollection);

            this.listView.ItemsSource = this.observableCollection;
        }
        async void RefreshPort_Clicked(System.Object sender, System.EventArgs e)
        {
            await RefreshPortInfo();
        }

        private void Init()
        {
            bool userInteractionEnabled = false;

            Dictionary<StarIoExtEmulation, List<bool>> userInteractionEnabledDictionary = new Dictionary<StarIoExtEmulation, List<bool>>()
            {
                {StarIoExtEmulation.StarPRNT,      new List<bool>() {userInteractionEnabled, userInteractionEnabled, userInteractionEnabled, userInteractionEnabled}},
                {StarIoExtEmulation.StarLine,      new List<bool>() {userInteractionEnabled, userInteractionEnabled, userInteractionEnabled, userInteractionEnabled}},
                {StarIoExtEmulation.StarGraphic,   new List<bool>() {userInteractionEnabled, userInteractionEnabled, userInteractionEnabled, userInteractionEnabled}},
                {StarIoExtEmulation.EscPos,        new List<bool>() {userInteractionEnabled, userInteractionEnabled, userInteractionEnabled, userInteractionEnabled}},
                {StarIoExtEmulation.EscPosMobile,  new List<bool>() {userInteractionEnabled, false,                  userInteractionEnabled, userInteractionEnabled}},
                {StarIoExtEmulation.StarDotImpact, new List<bool>() {userInteractionEnabled, userInteractionEnabled, userInteractionEnabled, userInteractionEnabled}},
                {StarIoExtEmulation.StarPRNTL,     new List<bool>() {userInteractionEnabled, userInteractionEnabled, userInteractionEnabled, userInteractionEnabled}}
            };

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            //if (this.didAppear == false)
            //{
                await RefreshPortInfo();

               // didAppear = true;
            //}
        }

        public async Task RefreshPortInfo()
        {
            Dictionary<string, string> targetDictionary;

            if (Device.RuntimePlatform == Device.iOS)
            {
                targetDictionary = new Dictionary<string, string>
                {
                    {"LAN",                  "TCP:"},
                    {"Bluetooth",            "BT:"},
                    {"Bluetooth Low Energy", "BLE:"},
                    {"USB",                  "USB:"},
                    {"All",                  ""}
                };
            }
            else
            {
                targetDictionary = new Dictionary<string, string>
                {
                    {"LAN",       "TCP:"},
                    {"Bluetooth", "BT:"},
                    {"USB",       "USB:"},
                    {"All",       ""}
                };
            }

            try
            {
                string target = targetDictionary[await DisplayActionSheet("Select Interface.", "Cancel", null, targetDictionary.Keys.ToArray())];

                //this.contentPageComponent.UserInteractionEnabled = false;

                listViewItemCollection.Clear();

                try
                {
                    if (target == "")
                    {
                        Communication.SearchPrinter(SearchPrinterAction);
                    }
                    else
                    {
                        Communication.SearchPrinter(target, SearchPrinterAction);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
            catch
            {
                await Navigation.PopAsync(true);
            }
        }

        public void SearchPrinterAction(IList<PortInfo> portInfoList)
        {
            foreach (PortInfo portInfo in portInfoList)
            {
                ListViewItem listViewItem = new ListViewItem(portInfo);

                this.listViewItemCollection.Add(listViewItem);
            }

            //this.contentPageComponent.UserInteractionEnabled = true;
        }
    }


    public class ListViewItem : IListViewItem
    {
        public PortInfo PortInfo { get; }

        public string Text { get; }

        public Color TextColor { get; }

        public string DetailText { get; }

        public Color DetailTextColor { get; }

        public ListViewItem(PortInfo portInfo)
        {
            this.PortInfo = portInfo;

            this.Text = this.PortInfo.ModelName;
            this.TextColor = Color.DodgerBlue;
            this.DetailText = this.PortInfo.PortName;
            this.DetailTextColor = Color.DodgerBlue;
        }
    }
}
