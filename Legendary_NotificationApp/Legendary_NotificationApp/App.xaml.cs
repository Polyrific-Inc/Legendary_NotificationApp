using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Legendary_NotificationApp
{
    public partial class App : Application
    {
        public App(string url)
        {
            InitializeComponent();

            MainPage = new MainPage();

            if (!string.IsNullOrEmpty(url))
            {
                Launcher.OpenAsync(new Uri(url));
            }
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public void OpenUrlFromNotificationMessage(string url)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                bool canOpen = await Launcher.CanOpenAsync(new Uri(url));
                if (canOpen)
                {
                    await Launcher.OpenAsync(new Uri(url));
                }
            });
        }

    }
}
