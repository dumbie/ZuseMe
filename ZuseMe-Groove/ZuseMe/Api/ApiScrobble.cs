using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZuseMe.Api
{
    public class ApiScrobble
    {
        public static async Task<bool> ScrobbleTrack(string artist, string title, string album, string duration, string trackNumber)
        {
            try
            {
                //Get session token
                string sessionToken = AVSettings.Load(null, "LastFMSessionToken", typeof(string));
                if (string.IsNullOrWhiteSpace(sessionToken)) { return false; }

                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("method", "track.scrobble");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);
                requestParameters.Add("sk", sessionToken);

                int durationInt = 0;
                if (!string.IsNullOrWhiteSpace(artist)) { requestParameters.Add("artist", artist); }
                if (!string.IsNullOrWhiteSpace(title)) { requestParameters.Add("track", title); }
                if (!string.IsNullOrWhiteSpace(album)) { requestParameters.Add("album", album); }
                if (!string.IsNullOrWhiteSpace(duration) && duration != "0")
                {
                    requestParameters.Add("duration", duration);
                    durationInt = Convert.ToInt32(duration);
                }
                if (!string.IsNullOrWhiteSpace(trackNumber) && trackNumber != "0") { requestParameters.Add("trackNumber", trackNumber); }
                DateTime scrobbleTime = DateTime.Now.AddSeconds(-(durationInt - 10));
                requestParameters.Add("timestamp", ApiFunctions.UnixTimeFromDateTime(scrobbleTime));

                //Generate api signature
                string apiSignature = ApiFunctions.GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate post content
                requestParameters.Add("format", "json");
                string postParameter = ApiFunctions.GeneratePostParameters(requestParameters);
                StringContent postContent = new StringContent(postParameter);

                //Post parameters
                Uri apiUrl = new Uri(ApiVariables.UrlApi);
                string apiResult = await AVDownloader.SendPostRequestAsync(2500, "ZuseMe", null, apiUrl, postContent);
                Debug.WriteLine("Scrobble result: " + apiResult);

                //Check the result
                if (apiResult != null)
                {
                    if (apiResult.Contains("\"accepted\":1"))
                    {
                        AppVariables.ScrobbleStatusMessage = "Song has been scrobbled successfully.";
                        AppVariables.ScrobbleStatusAccepted = true;
                    }
                    else
                    {
                        AppVariables.ScrobbleStatusMessage = "Song scrobble has not been accepted, check names.";
                        AppVariables.ScrobbleStatusAccepted = false;
                    }
                }
                else
                {
                    AppVariables.ScrobbleStatusMessage = "Song has not been scrobbled, check connection.";
                    AppVariables.ScrobbleStatusAccepted = false;
                }

                return true;
            }
            catch (Exception ex)
            {
                //Update the scrobble status
                AppVariables.ScrobbleStatusMessage = "Scrobble exception: " + ex.Message;
                AppVariables.ScrobbleStatusAccepted = false;
                return false;
            }
        }

        public static async Task<bool> UpdateNowPlaying(string artist, string title, string album, string duration, string trackNumber)
        {
            try
            {
                //Get session token
                string sessionToken = AVSettings.Load(null, "LastFMSessionToken", typeof(string));
                if (string.IsNullOrWhiteSpace(sessionToken))
                {
                    return false;
                }

                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("method", "track.updateNowPlaying");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);
                requestParameters.Add("sk", sessionToken);

                if (!string.IsNullOrWhiteSpace(artist)) { requestParameters.Add("artist", artist); }
                if (!string.IsNullOrWhiteSpace(title)) { requestParameters.Add("track", title); }
                if (!string.IsNullOrWhiteSpace(album)) { requestParameters.Add("album", album); }
                if (!string.IsNullOrWhiteSpace(duration) && duration != "0") { requestParameters.Add("duration", duration); }
                if (!string.IsNullOrWhiteSpace(trackNumber) && trackNumber != "0") { requestParameters.Add("trackNumber", trackNumber); }

                //Generate api signature
                string apiSignature = ApiFunctions.GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate post content
                requestParameters.Add("format", "json");
                string postParameter = ApiFunctions.GeneratePostParameters(requestParameters);
                StringContent postContent = new StringContent(postParameter);

                //Post parameters
                Uri apiUrl = new Uri(ApiVariables.UrlApi);
                string apiResult = await AVDownloader.SendPostRequestAsync(2500, "ZuseMe", null, apiUrl, postContent);
                Debug.WriteLine("Now playing result:" + apiResult);

                //Check the result
                if (apiResult.Contains("\"message\":") && apiResult.Contains("\"error\":"))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public static async Task RemoveNowPlaying()
        {
            try
            {
                //Get session token
                string sessionToken = AVSettings.Load(null, "LastFMSessionToken", typeof(string));
                if (string.IsNullOrWhiteSpace(sessionToken))
                {
                    return;
                }

                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("method", "track.removeNowPlaying");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);
                requestParameters.Add("sk", sessionToken);

                //Generate api signature
                string apiSignature = ApiFunctions.GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate post content
                requestParameters.Add("format", "json");
                string postParameter = ApiFunctions.GeneratePostParameters(requestParameters);
                StringContent postContent = new StringContent(postParameter);

                //Post parameters
                Uri apiUrl = new Uri(ApiVariables.UrlApi);
                string apiResult = await AVDownloader.SendPostRequestAsync(2500, "ZuseMe", null, apiUrl, postContent);
                Debug.WriteLine("Remove playing result: " + apiResult);
            }
            catch { }
        }
    }
}