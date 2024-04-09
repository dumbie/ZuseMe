﻿using System.Windows;

namespace ZuseMe
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Run application startup code
                await AppStartup.Startup();
            }
            catch { }
        }
    }
}