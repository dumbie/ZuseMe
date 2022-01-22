using ArnoldVinkCode;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;

namespace ZuseMe.Api
{
    public class ApiScrobble
    {
        public static async Task ScrobbleTrack(string artist, string title, string album, string duration)
        {
            try
            {
                //Get session token
                string sessionToken = Convert.ToString(ConfigurationManager.AppSettings["LastFMSessionToken"]);
                if (string.IsNullOrWhiteSpace(sessionToken))
                {
                    return;
                }

                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("method", "track.scrobble");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);
                requestParameters.Add("sk", sessionToken);

                if (!string.IsNullOrWhiteSpace(artist) && album != "Unknown") { requestParameters.Add("artist", artist); }
                if (!string.IsNullOrWhiteSpace(title) && album != "Unknown") { requestParameters.Add("track", title); }
                if (!string.IsNullOrWhiteSpace(album) && album != "Unknown") { requestParameters.Add("album", album); }
                requestParameters.Add("duration", duration);
                requestParameters.Add("timestamp", ApiFunctions.UnixTimeFromDateTime(DateTime.Now));

                //Generate api signature
                string apiSignature = ApiFunctions.GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate post content
                requestParameters.Add("format", "json");
                string postParameter = ApiFunctions.GeneratePostParameters(requestParameters);
                StringContent postContent = new StringContent(postParameter);

                //Post parameters
                Uri apiUrl = new Uri(ApiVariables.UrlApi);
                string apiResult = await AVDownloader.SendPostRequestAsync(2000, "ZuseMe", null, apiUrl, postContent);
                Debug.WriteLine(apiResult);
            }
            catch { }
        }

        public static async Task UpdateNowPlaying(string artist, string title, string album, string duration)
        {
            try
            {
                //Get session token
                string sessionToken = Convert.ToString(ConfigurationManager.AppSettings["LastFMSessionToken"]);
                if (string.IsNullOrWhiteSpace(sessionToken))
                {
                    return;
                }

                //Request parameters
                Dictionary<string, string> requestParameters = new Dictionary<string, string>();
                requestParameters.Add("method", "track.updateNowPlaying");
                requestParameters.Add("api_key", ApiVariables.KeyPublic);
                requestParameters.Add("sk", sessionToken);

                requestParameters.Add("artist", artist);
                requestParameters.Add("track", title);
                requestParameters.Add("album", album);
                requestParameters.Add("duration", duration);

                //Generate api signature
                string apiSignature = ApiFunctions.GenerateApiSignature(requestParameters);
                requestParameters.Add("api_sig", apiSignature);

                //Generate post content
                requestParameters.Add("format", "json");
                string postParameter = ApiFunctions.GeneratePostParameters(requestParameters);
                StringContent postContent = new StringContent(postParameter);

                //Post parameters
                Uri apiUrl = new Uri(ApiVariables.UrlApi);
                string apiResult = await AVDownloader.SendPostRequestAsync(2000, "ZuseMe", null, apiUrl, postContent);
                Debug.WriteLine(apiResult);
            }
            catch { }
        }

        public static async Task RemoveNowPlaying()
        {
            try
            {
                //Get session token
                string sessionToken = Convert.ToString(ConfigurationManager.AppSettings["LastFMSessionToken"]);
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
                string apiResult = await AVDownloader.SendPostRequestAsync(2000, "ZuseMe", null, apiUrl, postContent);
                Debug.WriteLine(apiResult);
            }
            catch { }
        }
    }
}