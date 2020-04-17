using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using pms.Views;

namespace pms
{
    public partial class App : Application
    {

        public App()
        {
            MainPage = new MainPage();
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
    }
}
