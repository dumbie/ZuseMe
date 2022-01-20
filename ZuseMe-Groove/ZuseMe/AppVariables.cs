using System.Configuration;

namespace ZuseMe
{
    public class AppVariables
    {
        //Application Configuration
        public static Configuration ApplicationConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        //Application Windows
        public static MainWindow WindowMain = new MainWindow();
    }
}