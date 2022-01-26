using System;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace ZuseMe
{
    public partial class Settings
    {
        public static object Setting_Load(Configuration sourceConfig, string settingName)
        {
            try
            {
                if (sourceConfig == null)
                {
                    sourceConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                return sourceConfig.AppSettings.Settings[settingName].Value;
            }
            catch
            {
                return null;
            }
        }

        public static void Setting_Save(Configuration sourceConfig, string settingName, string settingValue)
        {
            try
            {
                if (sourceConfig == null)
                {
                    sourceConfig = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                }
                sourceConfig.AppSettings.Settings.Remove(settingName);
                sourceConfig.AppSettings.Settings.Add(settingName, settingValue);
                sourceConfig.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }
        }

        public static void ManageStartupShortcut()
        {
            try
            {
                //Set application shortcut paths
                string targetFilePath = Assembly.GetEntryAssembly().CodeBase.Replace("file:///", string.Empty);
                string targetName = Assembly.GetEntryAssembly().GetName().Name;
                string targetFileShortcut = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Startup), targetName + ".url");

                //Check if the shortcut already exists
                if (!File.Exists(targetFileShortcut))
                {
                    Debug.WriteLine("Adding application to Windows startup.");
                    using (StreamWriter StreamWriter = new StreamWriter(targetFileShortcut))
                    {
                        StreamWriter.WriteLine("[InternetShortcut]");
                        StreamWriter.WriteLine("URL=" + targetFilePath);
                        StreamWriter.WriteLine("IconFile=" + targetFilePath);
                        StreamWriter.WriteLine("IconIndex=0");
                        StreamWriter.Flush();
                    }
                }
                else
                {
                    Debug.WriteLine("Removing application from Windows startup.");
                    File.Delete(targetFileShortcut);
                }
            }
            catch
            {
                Debug.WriteLine("Failed creating startup shortcut.");
            }
        }
    }
}