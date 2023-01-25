using ArnoldVinkCode;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using ZuseMe.Classes;
using static ArnoldVinkCode.AVActions;
using static ArnoldVinkCode.AVSettings;

namespace ZuseMe.Api
{
    public class ApiAuth
    {
        public static async Task AuthLinkLogin()
        {
            try
            {
                //Reset current auth
                AuthUnlinkLogin();

                //Get auth token
                AuthToken loginToken = await AuthGetAuthToken();
                if (loginToken == null)
                {
                    MessageBox.Show("Failed to get Last.fm auth token, please try again.", "ZuseMe");
                    return;
                }
                else
                {
                    //Update settings
                    SettingSave(null, "LastFMAuthToken", loginToken.token);
                }

                //Update interface
                AppVariables.WindowMain.button_LinkLastFM.Content = "Cancel Last.fm link";
                AppVariables.WindowMain.button_UnlinkLastFM.IsEnabled = false;
                AppVariables.WindowMain.progress_LoginStatus.Visibility = Visibility.Visible;
                AppVariables.WindowMain.textblock_LoginStatus.Visibility = Visibility.Visible;

                //Continue in browser
                Process.Start(ApiVariables.UrlLogin + "?api_key=" + ApiVariables.KeyPublic + "&token=" + loginToken.token);

                //Start task to check login
                TaskStartLoop(AuthLoginCheckLoop, AppTasks.vTask_LoginCheck);
            }
            catch { }
        }

        public static async Task AuthCancelLogin()
        {
            try
            {
                //Reset current auth
                AuthUnlinkLogin();

                //Update interface
                AVActions.ActionDispatcherInvoke(delegate
                {
                    AppVariables.WindowMain.button_LinkLastFM.Content = "Link my Last.fm profile";
                    AppVariables.WindowMain.button_UnlinkLastFM.IsEnabled = true;
                    AppVariables.WindowMain.progress_LoginStatus.Visibility = Visibility.Collapsed;
                    AppVariables.WindowMain.textblock_LoginStatus.Visibility = Visibility.Collapsed;
                });

                //Stop the task loop
                await TaskStopLoop(AppTasks.vTask_LoginCheck, 5000);
            }
            catch { }
        }

        public static void AuthUnlinkLogin()
        {
            try
            {
                //Update settings
                SettingSave(null, "LastFMUsername", string.Empty);
                SettingSave(null, "LastFMAuthToken", string.Empty);
                SettingSave(null, "LastFMSessionToken", string.Empty);

                //Update interface
                AVActions.ActionDispatcherInvoke(delegate
                {
                    AppVariables.WindowMain.UpdateLastFMUsername();
                });
            }
            catch { }
        }

        private static async Task AuthLoginCheckLoop()
        {
            try
            {
                while (TaskCheckLoop(AppTasks.vTask_LoginCheck))
                {
                    try
                    {
                        //Get session token
                        SessionToken sessionToken = await AuthGetSessionToken();
                        if (sessionToken == null)
                        {
                            Debug.WriteLine("Failed to get Last.fm session token.");
                        }
                        else
                        {
                            //Update settings
                            SettingSave(null, "LastFMUsername", sessionToken.name);
                            SettingSave(null, "LastFMSessionToken", sessionToken.key);

                            //Update interface
                            AVActions.ActionDispatcherInvoke(delegate
                            {
                                AppVariables.WindowMain.UpdateLastFMUsername();
                                AppVariables.WindowMain.button_LinkLastFM.Content = "Link my Last.fm profile";
                                AppVariables.WindowMain.button_UnlinkLastFM.IsEnabled = true;
                                AppVariables.WindowMain.progress_LoginStatus.Visibility = Visibility.Collapsed;
                                AppVariables.WindowMain.textblock_LoginStatus.Visibility = Visibility.Collapsed;
                            });

                            //Stop the task loop
                            await TaskStopLoop(AppTasks.vTask_LoginCheck, 5000);
                        }
                    }
                    catch { }
                    finally
                    {
                        //Delay the loop task
                        await TaskDelayLoop(2000, AppTasks.vTask_LoginCheck);
                    }
                }
            }
            catch { }
        }

        private static async Task<AuthToken> AuthGetAuthToken()
        {
            try
            {
                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("method", "auth.getToken");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);

                //Generate api signature
                string apiSignature = ApiFunctions.GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate url parameter
                requestParameters.Add("format", "json");
                string urlParameter = ApiFunctions.GenerateUrlParameters(requestParameters);

                //Download token
                Uri apiUrl = new Uri(ApiVariables.UrlApi + urlParameter);
                string apiResult = await AVDownloader.DownloadStringAsync(2500, "ZuseMe", null, apiUrl);

                //Extract token
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
                return JsonConvert.DeserializeObject<AuthToken>(apiResult, jsonSettings);
            }
            catch
            {
                return null;
            }
        }

        private static async Task<SessionToken> AuthGetSessionToken()
        {
            try
            {
                //Get auth token
                string authToken = SettingLoad(null, "LastFMAuthToken", typeof(string));
                if (string.IsNullOrWhiteSpace(authToken))
                {
                    return null;
                }

                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("method", "auth.getSession");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);
                requestParameters.Add("token", authToken);

                //Generate api signature
                string apiSignature = ApiFunctions.GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate url parameter
                requestParameters.Add("format", "json");
                string urlParameter = ApiFunctions.GenerateUrlParameters(requestParameters);

                //Download token
                Uri apiUrl = new Uri(ApiVariables.UrlApi + urlParameter);
                string apiResult = await AVDownloader.DownloadStringAsync(2500, "ZuseMe", null, apiUrl);

                //Extract token
                JsonSerializerSettings jsonSettings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Error };
                return JsonConvert.DeserializeObject<Session>(apiResult, jsonSettings).session;
            }
            catch
            {
                return null;
            }
        }
    }
}