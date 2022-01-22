using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace ZuseMe.Api
{
    public class ApiFunctions
    {
        public static string UnixTimeFromDateTime(DateTime dateTime)
        {
            return ((DateTimeOffset)dateTime).ToUnixTimeSeconds().ToString();
        }

        public static string GeneratePostParameters(Dictionary<string, string> requestParameters)
        {
            try
            {
                int parameterCount = 0;
                string parameterSign = string.Empty;
                string urlParameter = string.Empty;

                //Add parameters
                foreach (var keyValue in requestParameters.OrderBy(x => x.Key))
                {
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

        public static string GenerateUrlParameters(Dictionary<string, string> requestParameters)
        {
            try
            {
                int parameterCount = 0;
                string parameterSign = "?";
                string urlParameter = string.Empty;

                //Add parameters
                foreach (var keyValue in requestParameters.OrderBy(x => x.Key))
                {
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

        public static string GenerateApiSignature(Dictionary<string, string> requestParameters)
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