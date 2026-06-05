using System.Windows;
using static ArnoldVinkCode.AVProcess;
using static ArnoldVinkCode.AVStartup;

namespace ZuseMe
{
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            try
            {
                //Setup application defaults
                SetupDefaults(ProcessPriorityClasses.Normal, true, false);

                //Run application startup code
                await AppStartup.Startup();
            }
            catch { }
        }
    }
}