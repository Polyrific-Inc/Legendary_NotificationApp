using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Legendary_NotificationApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private const string defaultUrl = "https://legendary-intranet.legendary.com";
        public MainPage()
        {
            InitializeComponent();
        }

        public void OpenUrlFromNotificationMessage(string url)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool canOpen = await Launcher.CanOpenAsync(new Uri(url));
                if(canOpen){
                    await Launcher.OpenAsync(new Uri(url));
                }
                else{
                    await Launcher.OpenAsync(new Uri(defaultUrl));
                }
            });
        }

    }
}
