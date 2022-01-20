using ArnoldVinkCode;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using static ArnoldVinkCode.AVActions;

namespace ZuseMe.Api
{
    internal class ApiAuth
    {
        public static async Task AuthLinkLogin()
        {
            try
            {
                //Reset current auth
                AuthUnlinkLogin();

                //Generate login token
                string loginToken = await AuthGetToken();
                if (string.IsNullOrWhiteSpace(loginToken))
                {
                    MessageBox.Show("Failed to get Last.fm auth token.", "ZuseMe");
                    return;
                }

                //Open in browser
                string loginUrl = ApiVariables.UrlLogin + "?format=json&api_key=" + ApiVariables.KeyPublic + "&token=" + loginToken;
                Process.Start(loginUrl);

                //Start timer to check login
                AVActions.TaskStartLoop(AuthLoginCheckLoop, AppTasks.vTask_LoginCheck);
            }
            catch { }
        }

        public static void AuthUnlinkLogin()
        {
            try
            {
                //Reset auth settings
                AppVariables.ApplicationConfig.AppSettings.Settings["LastFMAuthToken"].Value = string.Empty;
                AppVariables.ApplicationConfig.AppSettings.Settings["LastFMSessionToken"].Value = string.Empty;
                AppVariables.ApplicationConfig.Save();
                ConfigurationManager.RefreshSection("appSettings");
            }
            catch { }
        }

        private static async void AuthLoginCheckLoop()
        {
            try
            {
                while (!AppTasks.vTask_LoginCheck.TaskStopRequest)
                {
                    //Get and set session token
                    string sessionKey = await AuthGetSession();
                    Debug.WriteLine(sessionKey);

                    //Check if the session token is set

                    //Stop the task loop
                    //await AVActions.TaskStopLoop(AppTasks.vTask_LoginCheck);

                    //Delay the loop task
                    await TaskDelayLoop(2000, AppTasks.vTask_LoginCheck);
                }
            }
            catch { }
        }

        private static async Task<string> AuthGetToken()
        {
            try
            {
                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("format", "json");
                requestParameters.Add("method", "auth.getToken");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);

                //Generate api signature
                string apiSignature = GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate url parameter
                string urlParameter = GenerateUrlParameters(requestParameters);

                //Download token
                Uri apiUrl = new Uri(ApiVariables.UrlApi + urlParameter);
                string apiResult = await AVDownloader.DownloadStringAsync(3000, "ZuseMe", null, apiUrl);

                Debug.WriteLine(apiSignature);
                Debug.WriteLine(urlParameter);

                //Extract token
                JObject apiJson = JObject.Parse(apiResult);
                string apiString = apiJson["token"].ToString();

                //Update settings
                AppVariables.ApplicationConfig.AppSettings.Settings["LastFMAuthToken"].Value = apiString;
                AppVariables.ApplicationConfig.Save();
                ConfigurationManager.RefreshSection("appSettings");

                //Return token
                return apiString;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static async Task<string> AuthGetSession()
        {
            try
            {
                //Get auth token
                string authToken = Convert.ToString(ConfigurationManager.AppSettings["LastFMAuthToken"]);

                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("format", "json");
                requestParameters.Add("method", "auth.getSession");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);
                requestParameters.Add("token", authToken);

                //Generate api signature
                string apiSignature = GenerateApiSignature(requestParameters);

                //Download token
                Uri apiUrl = new Uri(ApiVariables.UrlApi + "?format=json&method=auth.getSession&api_key=" + ApiVariables.KeyPublic + "&token=" + authToken + "&api_sig=" + apiSignature);
                string apiResult = await AVDownloader.DownloadStringAsync(3000, "ZuseMe", null, apiUrl);

                //Extract token
                JObject apiJson = JObject.Parse(apiResult);
                string apiString = apiJson["session"]["key"].ToString();

                //Update settings
                AppVariables.ApplicationConfig.AppSettings.Settings["LastFMSessionToken"].Value = apiString;
                AppVariables.ApplicationConfig.Save();
                ConfigurationManager.RefreshSection("appSettings");

                //Return token
                Debug.WriteLine(apiString);
                return apiString;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GenerateUrlParameters(Dictionary<string, string> requestParameters)
        {
            try
            {
                int parameterCount = 0;
                string urlParameter = string.Empty;

                //Add parameters
                foreach (var keyValue in requestParameters.OrderBy(x => x.Key))
                {
                    string parameterSign = "?";
                    if (parameterCount != 0) { parameterSign = "&"; }
                    urlParameter += parameterSign + HttpUtility.UrlEncode(keyValue.Key) + "=" + HttpUtility.UrlEncode(keyValue.Value);
                    parameterCount++;
                }

                return urlParameter;
            }
            catch
            {
                return string.Empty;
            }
        }

        private static string GenerateApiSignature(Dictionary<string, string> requestParameters)
        {
            try
            {
                StringBuilder stringBuilderParameters = new StringBuilder();

                //Add parameters
                foreach (var keyValue in requestParameters.OrderBy(x => x.Key))
                {
                    stringBuilderParameters.Append(keyValue.Key);
                    stringBuilderParameters.Append(keyValue.Value);
                }

                //Add secret api key
                stringBuilderParameters.Append(ApiVariables.KeySecret);

                //Compute MD5 Hash
                using (MD5 md5 = MD5.Create())
                {
                    byte[] inputBytes = Encoding.UTF8.GetBytes(stringBuilderParameters.ToString());
                    byte[] hashBytes = md5.ComputeHash(inputBytes);
                    StringBuilder stringBuilderMD5 = new StringBuilder();
                    for (int i = 0; i < hashBytes.Length; i++)
                    {
                        stringBuilderMD5.Append(hashBytes[i].ToString("x2"));
                    }
                    return stringBuilderMD5.ToString();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}