using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SkyDrive
{
    public class SkyDriveAPI
    {
        public User user = new User();
        public Quota quota = new Quota();
        private string ClientId = "00000000480CF02C";
        private string ClietSecret = "YeHKhAaNRh8n3htpbIOQR6pXKFr5Tghp";
        private string RedirectUri = "http://arleons.com";
        private string AccessToken = "";
        private string RefreshToken = "";
         

        public void set_app_config(string client_id)
        {
            ClientId = client_id;
        }

        public void AuthorizeApplication()
        {
            const string uri = "https://login.live.com/oauth20_authorize.srf";

            var authorizeUri = new StringBuilder(uri);

            authorizeUri.AppendFormat("?client_id={0}&", ClientId);
            authorizeUri.AppendFormat("scope={0}&", "wl.signin%20wl.skydrive%20wl.photos%20wl.skydrive_update%20wl.offline_access");
            authorizeUri.AppendFormat("response_type={0}&", "code");
            authorizeUri.AppendFormat("redirect_uri={0}", UrlEncode(RedirectUri));
            string location = "";
            var startInfo = new ProcessStartInfo { FileName = authorizeUri.ToString() };
            Process.Start(startInfo);
        }
        //MAKE REQUEST NOT PUT ALL DATA TO URL
        public string Auth(string code)
        {
            const string uri = "https://login.live.com/oauth20_token.srf";

            var authorizeUri = new StringBuilder(uri);
            //client_id=CLIENT_ID&redirect_uri=REDIRECT_URI&client_secret=CLIENT_SECRET&code=AUTHORIZATION_CODE&grant_type=authorization_code
            authorizeUri.AppendFormat("?client_id={0}&", ClientId);
            authorizeUri.AppendFormat("redirect_uri={0}&", UrlEncode(RedirectUri));
            authorizeUri.AppendFormat("client_secret={0}&", ClietSecret);
            authorizeUri.AppendFormat("code={0}&", code);
            authorizeUri.AppendFormat("grant_type={0}", "authorization_code");

            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            Stream data = client.OpenRead(authorizeUri.ToString());
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();

            Access acc = JsonConvert.DeserializeObject<Access>(s);
            SetAccessToken(acc.access_token);
            return acc.refresh_token;
        }

        public string RefreshAuth(string reftoken)
        {
            const string uri = "https://login.live.com/oauth20_token.srf";

            var authorizeUri = new StringBuilder(uri);
            //client_id=CLIENT_ID&client_secret=CLIENT_SECRET&redirect_uri=REDIRECT_URI&grant_type=refresh_token&refresh_token=REFRESH_TOKEN
            authorizeUri.AppendFormat("?client_id={0}&", ClientId);
            authorizeUri.AppendFormat("client_secret={0}&", ClietSecret);
            authorizeUri.AppendFormat("redirect_uri={0}&", UrlEncode(RedirectUri));
            authorizeUri.AppendFormat("grant_type={0}&", "refresh_token");
            authorizeUri.AppendFormat("refresh_token={0}", reftoken);
            WebClient client = new WebClient();
            client.Headers.Add("user-agent", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705;)");
            Stream data = client.OpenRead(authorizeUri.ToString());
            StreamReader reader = new StreamReader(data);
            string s = reader.ReadToEnd();
            data.Close();
            reader.Close();
            
            Access acc = JsonConvert.DeserializeObject<Access>(s);
            SetAccessToken(acc.access_token);
            return acc.refresh_token;
        }

        private string UrlEncode(string s)
        {
            char[] temp = HttpUtility.UrlEncode(s).ToCharArray();
            for (int i = 0; i < temp.Length - 2; i++)
            {
                if (temp[i] == '%')
                {
                    temp[i + 1] = char.ToUpper(temp[i + 1]);
                    temp[i + 2] = char.ToUpper(temp[i + 2]);
                }
            }

            var values = new Dictionary<string, string>
            {
                { "!", "%21" },
                { "#", "%23" },
                { "$", "%24" },
                { "&", "%26" },
                { "'", "%27" },
                { "(", "%28" },
                { ")", "%29" },
                { "*", "%2A" },
                { "+", "%2B" },
                { ",", "%2C" },
                { "/", "%2F" },
                { ":", "%3A" },
                { ";", "%3B" },
                { "=", "%3D" },
                { "?", "%3F" },
                { "@", "%40" },
                { "[", "%5B" },
                { "]", "%5D" }
            };

            var data = new StringBuilder(new string(temp));
            foreach (string character in values.Keys)
            {
                data.Replace(character, values[character]);
            }

            return data.ToString();
        }

        public void SetAccessToken(string atoken)
        {
            AccessToken = atoken;
        }

        public User user_info()
        {
            var requestUri = new StringBuilder("https://apis.live.net/v5.0/me");
            requestUri.AppendFormat("?access_token={0}", AccessToken);
            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = reader.ReadToEnd();
                user = JsonConvert.DeserializeObject<User>(json);
                return user;
            }
        }

        public void UserQuota()
        {
            var requestUri = new StringBuilder("https://apis.live.net/v5.0/me/skydrive/quota");
            requestUri.AppendFormat("?access_token={0}", AccessToken);
            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = "GET";

            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = reader.ReadToEnd();
                quota = JsonConvert.DeserializeObject<Quota>(json);
            }
        }

        public void CreateFolder(string folderid, string name)
        {
            var requestUri = new StringBuilder(String.Format("https://apis.live.net/v5.0/me/skydrive"));
            requestUri.AppendFormat("?access_token={0}", AccessToken);
            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = "POST";
            request.ContentType = "application/json";

            string xml_request = "{" +
                                    "\"name\": \"" + name + "\"," +
                                    "\"description\": \"A folder.\"" +
                                  "}";
            Console.WriteLine(xml_request);
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = JObject.Parse(reader.ReadToEnd());
            }
        }

        public void CopyFolder(string folderid, string tofolderid)
        {
            var requestUri = new StringBuilder(String.Format("https://apis.live.net/v5.0/" + folderid));
            requestUri.AppendFormat("?access_token={0}", AccessToken);
            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = "MOVE";
            request.ContentType = "application/json";

            string xml_request = "{ destination: \"" + tofolderid + "\" }";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = JObject.Parse(reader.ReadToEnd());
            }
        }

        public void DeleteFolder(string folderid)
        {
            var requestUri = new StringBuilder(String.Format("https://apis.live.net/v5.0/" + folderid));
            requestUri.AppendFormat("?access_token={0}", AccessToken);
            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = "DELETE";
            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = JObject.Parse(reader.ReadToEnd());
            }
        }

        public void MoveFolder(string folderid, string tofolderid)
        {
            var requestUri = new StringBuilder(String.Format("https://apis.live.net/v5.0/" + folderid));
            requestUri.AppendFormat("?access_token={0}", AccessToken);
            var request = (HttpWebRequest)WebRequest.Create(requestUri.ToString());
            request.Method = "MOVE";
            request.ContentType = "application/json";

            string xml_request = "{ destination: \"" + tofolderid + "\" }";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();
            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                var json = JObject.Parse(reader.ReadToEnd());
            }
        }
    }
}
