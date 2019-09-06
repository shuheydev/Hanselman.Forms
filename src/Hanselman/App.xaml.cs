﻿using Xamarin.Forms;
using Hanselman.Views;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using MonkeyCache.FileStore;
using Shiny.Jobs;
using MediaManager;
using MediaManager.Playback;
using Hanselman.Helpers;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.AppCenter.Distribute;

// ElectricHavoc cheered 10 March 29, 2019
// KymPhillpotts cheered 50 March 29, 2019
// ElecticHavoc cheered 40 March 29, 2019

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Hanselman
{
    public partial class App : Application
    {
        public static bool IsWindows10 { get; set; }
        public App()
        {
            InitializeComponent();

            Barrel.ApplicationId = AppInfo.PackageName;

            // The root page of your application
            if (DeviceInfo.Platform == DevicePlatform.UWP)
                MainPage = new HomePage();
            else
                MainPage = new AppShell();
        }

        const string AppCenteriOS = "APPCENTER_IOS";
        const string AppCenterAndroid = "APPCENTER_ANDROID";
        const string AppCenterUWP = "APPCENTER_UWP";

        protected override void OnStart()
        {
#if !DEBUG
            AppCenter.Start($"ios={AppCenteriOS};" +
                $"android={AppCenterAndroid};" +
                $"uwp={AppCenterUWP}", 
                typeof(Analytics), 
                typeof(Crashes),
                typeof(Distribute));
#endif
            // Handle when your app starts
            CrossMediaManager.Current.PositionChanged += PlaybackPositionChanged;
        }

        void PlaybackPositionChanged(object sender, PositionChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Settings.PlaybackId) || !CrossMediaManager.Current.IsPlaying())
                return;

            var current = e.Position.Ticks;
            Settings.SavePlaybackPosition(Settings.PlaybackId, current);
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
