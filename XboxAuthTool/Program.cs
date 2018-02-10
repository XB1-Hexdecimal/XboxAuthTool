using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XboxAuthTool.Models;

namespace XboxAuthTool
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Usage: XboxAuthTool.exe devicetoken/pathtodevicetoken sandboxID RelyingParty");
            }
            else
            {
                //Check if the provided arg is a file path or the token directly.
                if (!File.Exists(args[0]))
                {
                    GetAuthToken(args[0], args[1], args[2]).Wait();
                }
                else
                {
                    string authtoken = File.ReadAllText(args[0]);
                    GetAuthToken(authtoken, args[1], args[2]).Wait();
                }
                
            }
        }

        async static Task GetAuthToken(string DeviceToken, string SandboxId, string RelyingParty)
        {
            Uri XSTSAuthEndPoint = new Uri($"https://xsts.auth.xboxlive.com/xsts/authorize");
            HttpClient client = new HttpClient();
            //Construct the needed JSON for the request
            XstsAuthRequest xstsAuthRequest = new XstsAuthRequest();
            xstsAuthRequest.Properties = new Properties();
            xstsAuthRequest.Properties.SandboxId = SandboxId; 
            xstsAuthRequest.Properties.DeviceToken = DeviceToken; //DeviceToken doesn't expire for a loooong time, but is per mode. (Retail Mode token differs from DevMode token)
            xstsAuthRequest.RelyingParty = RelyingParty; //http://attestation.xboxlive.com, http://xboxlive.com, http://xkms.xboxlive.com, http://vortex.microsoft.com, http://update.xboxlive.com, http://login.live.com
            xstsAuthRequest.TokenType = "JWT"; //Hardcoding this as I have never seen another type. 
            StringContent content = new StringContent(xstsAuthRequest.ToJson());
            HttpResponseMessage httpResponse = await client.PostAsync(XSTSAuthEndPoint, content);
            //We got a token, serialize the JSON. 
            if (httpResponse.IsSuccessStatusCode)
            {
                string authresponse = httpResponse.Content.ReadAsStringAsync().Result;
                var authjson = XstsAuth.FromJson(authresponse);
                Console.WriteLine($"***Obtained Auth Token!***\nExpires: {authjson.NotAfter} Token: {authjson.Token}");

            }
            //Unauthorized for the requested sandbox, when using a devmode token, this will occur for RETAIl
            else if (httpResponse.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Console.WriteLine($"The Device Token is not authorized for the requested Sandbox");

            }
            //Some other issue
            else
            {
                Console.WriteLine($"xsts.auth.xboxlive.com error - status code {httpResponse.StatusCode.ToString()}");
            }

        }
    }
}
