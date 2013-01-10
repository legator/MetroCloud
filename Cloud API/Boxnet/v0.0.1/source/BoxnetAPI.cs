using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Boxnet
{
    public class BoxnetAPI
    {
        private string auth_token { get; set; }
        private string ticket { get; set; }
        private string api_key { get; set; }

        public User user;


        public void set_app_config(string apikey)
        {
            api_key = apikey;
        }
        /// <summary>
        /// Get ticket to login
        /// </summary>
        /// <param name="apikey"></param>
        /// <returns>Application key</returns>
        public string GetTicket(string apikey)
        {
            api_key = apikey;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.box.net/api/1.0/rest?action=get_ticket&api_key=" + apikey);
            request.Method = "POST";

            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(reader.ReadToEnd());
            foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//ticket"))
            {
                ticket = xitem.InnerText;
            }
            return ticket;
        }
        /// <summary>
        /// Authorization app
        /// </summary>
        public void Auth()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.box.com/api/1.0/rest?action=get_auth_token&api_key=" + api_key + "&ticket=" + ticket);
                request.Method = "POST";

                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//auth_token"))
                {
                    auth_token = xitem.InnerText;
                }
                user = new User();
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//login"))
                {
                    user.login = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//email"))
                {
                    user.email = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//access_id"))
                {
                    user.access_id = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//user_id"))
                {
                    user.user_id = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//space_amount"))
                {
                    user.space_amount = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//space_used"))
                {
                    user.space_used = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//max_upload_size"))
                {
                    user.max_upload_size = xitem.InnerText;
                }
            }
            catch (Exception) { throw new System.Exception("Error to get user information"); }
        }

        public void Login(string AuthToken)
        {
            auth_token = AuthToken;
        }

        public void user_info()
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.box.net/api/1.0/rest?action=get_account_info&api_key=" + api_key + "&auth_token=" + auth_token);
                request.Method = "POST";
                StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                user = new User();
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//login"))
                {
                    user.login = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//email"))
                {
                    user.email = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//access_id"))
                {
                    user.access_id = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//user_id"))
                {
                    user.user_id = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//space_amount"))
                {
                    user.space_amount = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//space_used"))
                {
                    user.space_used = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//max_upload_size"))
                {
                    user.max_upload_size = xitem.InnerText;
                }
            }
            catch (Exception) { throw new System.Exception("Error to get user information"); }
        }

        public string GetToken()
        {
            return auth_token;
        }
        /// <summary>
        /// The new folder will be created inside of the parent folder
        /// </summary>
        /// <param name="parent_id">Id of parent folder</param>
        /// <param name="foldername">Name of new folder</param>
        /// <param name="isshared">Is folder shared</param>
        /// <returns name="FolderInfo">Information about created file</returns>
        public FolderInfo CreateFolder(string parent_id, string foldername, bool isshared)
        {
            CreateFolderInfo cfi = new CreateFolderInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/folders/" + parent_id);
            request.Method = "POST";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            string xml_request = "{\"name\":\"" + foldername + "\"}";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            cfi = JsonConvert.DeserializeObject<CreateFolderInfo>(reader.ReadToEnd());
            return ConvetrFolderInfo(cfi);
        }
        /// <summary>
        /// Convert CreateFolderInfo object to FolderInfo object
        /// </summary>
        /// <param name="cfi">CreateFolderInfo object</param>
        /// <returns>FolderInfo object</returns>
        private FolderInfo ConvetrFolderInfo(CreateFolderInfo cfi)
        {
            FolderInfo fi = new FolderInfo();
            fi.create_by = cfi.created_by;
            fi.created_at = cfi.created_at;
            fi.description = cfi.description;
            fi.id = cfi.id;
            fi.item_collection = null;
            fi.modified_at = cfi.modified_at;
            fi.modified_by = cfi.modified_by;
            fi.name = cfi.name;
            fi.owned_by = cfi.owned_by;
            fi.parent = cfi.parent;
            fi.sequence_id = cfi.sequence_id;
            fi.shared_link = null;
            fi.size = cfi.size;
            fi.type = cfi.type;
            return fi;
        }
        /// <summary>
        /// Convert FolderInfoItems object to FolderInfo object
        /// </summary>
        /// <param name="fii"></param>
        /// <returns></returns>
        private FolderInfo ConvetrFolderInfo(FolderInfoItems fii)
        {
            FolderInfo fi = new FolderInfo();
            fi.create_by = fii.created_by;
            fi.created_at = fii.created_at;
            fi.description = fii.description;
            fi.id = fii.id;
            fi.item_collection = fii.item_collection;
            fi.modified_at = fii.modified_at;
            fi.modified_by = fii.modified_by;
            fi.name = fii.name;
            fi.owned_by = fii.owned_by;
            fi.parent = fii.parent;
            fi.sequence_id = fii.sequence_id;
            fi.shared_link = null;
            fi.size = fii.size;
            fi.type = fii.type;
            return fi;
        }
        /// <summary>
        /// Convert SharedFolderInfo object to FolderInfo object
        /// </summary>
        /// <param name="shi"></param>
        /// <returns></returns>
        private FolderInfo ConvetrFolderInfo(SharedFolderInfo shi)
        {
            FolderInfo fi = new FolderInfo();
            fi.create_by = shi.created_by;
            fi.created_at = shi.created_at;
            fi.description = shi.description;
            fi.id = shi.id;
            fi.item_collection = null;
            fi.modified_at = shi.modified_at;
            fi.modified_by = shi.modified_by;
            fi.name = shi.name;
            fi.owned_by = shi.owned_by;
            fi.parent = shi.parent;
            fi.sequence_id = fi.sequence_id;
            fi.shared_link = shi.shared_link;
            fi.size = shi.size;
            fi.type = shi.type;
            return fi;
        }
        /// <summary>
        /// Shared folder
        /// </summary>
        /// <param name="folder_id"></param>
        /// <param name="access"></param>
        /// <param name="download"></param>
        /// <param name="preciew"></param>
        /// <returns></returns>
        public FolderInfo ShareFolder(string folder_id, string access, bool download, bool preciew)
        {
            FolderInfo fi = new FolderInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/folders/" + folder_id);
            request.Method = "PUT";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            string xml_request = "{\"shared_link\": {\"access\": \"open\"}}";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //shared with items
                fi = JsonConvert.DeserializeObject<FolderInfo>(response);
            }
            catch (System.Exception)
            {
                try
                {
                    //items
                    fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<FolderInfoItems>(response));
                }
                catch (System.Exception)
                {
                    try
                    {
                        //shared
                        fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<SharedFolderInfo>(response));
                    }
                    catch (System.Exception)
                    {
                        //empty
                        fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<CreateFolderInfo>(response));
                    }
                }
            }
            return fi;
        }
        /// <summary>
        /// Delete folder
        /// </summary>
        /// <param name="folder_id"></param>
        public void DeleteFolder(string folder_id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/folders/" + folder_id + "?force=true");
            request.Method = "DELETE";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
        }
        /// <summary>
        /// Copy file
        /// Error in parsing
        /// </summary>
        /// <param name="tofolder_id"></param>
        /// <param name="folder_id"></param>
        public FolderInfo CopyFolder(string tofolder_id, string folder_id)
        {
            FolderInfo fi = new FolderInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/folders/" + folder_id + "/copy");
            request.Method = "POST";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            string xml_request = "{\"parent\":{\"id\":" + tofolder_id + "}}";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //shared with items
                fi = JsonConvert.DeserializeObject<FolderInfo>(response);
            }
            catch (System.Exception)
            {
                try
                {
                    //items
                    fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<FolderInfoItems>(response));
                }
                catch (System.Exception)
                {
                    try
                    {
                        //shared
                        fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<SharedFolderInfo>(response));
                    }
                    catch (System.Exception)
                    {
                        //empty
                        fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<CreateFolderInfo>(response));
                    }
                }
            }
            return fi;
        }
        /// <summary>
        /// Get information about folder
        /// </summary>
        /// <param name="folder_id"></param>
        /// <returns></returns>
        public FolderInfo AboutFolder(string folder_id)
        {
            FolderInfo fi = new FolderInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/folders/" + folder_id);
            request.Method = "GET";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //shared with items
                fi = JsonConvert.DeserializeObject<FolderInfo>(response);
            }
            catch (System.Exception)
            {
                try
                {
                    //items
                    fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<FolderInfoItems>(response));
                }
                catch (System.Exception)
                {
                    try
                    {
                        //shared
                        fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<SharedFolderInfo>(response));
                    }
                    catch (System.Exception)
                    {
                        //empty
                        fi = ConvetrFolderInfo(JsonConvert.DeserializeObject<CreateFolderInfo>(response));
                    }
                }
            }
            return fi;
        }
        /// <summary>
        /// Get folder item list
        /// </summary>
        /// <param name="folder_id"></param>
        /// <returns>List of FolderItem</returns>
        public List<FolderItem> FolderContent(string folder_id)
        {
            List<FolderItem> fil = new List<FolderItem>();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/folders/" + folder_id + "/items");
            request.Method = "GET";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            JObject folderobject = JObject.Parse(reader.ReadToEnd());

            if ((string)folderobject["total_count"] != "0")
            {
                for (int i = 0; i < (int)folderobject["total_count"]; i++)
                {
                    FolderItem folitem = new FolderItem();
                    folitem.type = (string)folderobject["entries"]["type"][i];
                    folitem.id = (string)folderobject["entries"]["id"][i];
                    folitem.sequence_id = (string)folderobject["entries"]["sequence_id"][i];
                    folitem.name = (string)folderobject["entries"]["name"][i];
                    fil.Add(folitem);
                }
            }
            return fil;
        }
        /// <summary>
        /// Update folder information
        /// Serialization
        /// </summary>
        /// <param name="folder_info"></param>
        public void UpdateFolderInfo(FolderInfo folder_info)
        {

        }
        /// <summary>
        /// Copy file
        /// </summary>
        /// <param name="folder_id"></param>
        /// <param name="file_id"></param>
        public FileInfo CopyFile(string folder_id, string file_id)
        {
            FileInfo fi = new FileInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/files/" + file_id + "/copy");
            request.Method = "POST";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            string xml_request = "{\"parent\":{\"id\":" + folder_id + "}}";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //shared
                fi = JsonConvert.DeserializeObject<FileInfo>(response);
            }
            catch (System.Exception)
            {
                //unshared
                fi = ConvertFileInfo(JsonConvert.DeserializeObject<UnSharedFileInfo>(response));
            }
            return fi;
        }
        /// <summary>
        /// Convert UnSharedFileInfo object to FileInfo object
        /// </summary>
        /// <param name="usfi"></param>
        /// <returns></returns>
        private FileInfo ConvertFileInfo(UnSharedFileInfo usfi)
        {
            FileInfo fi = new FileInfo();
            fi.create_by = usfi.create_by;
            fi.created_at = usfi.created_at;
            fi.description = usfi.description;
            fi.etag = usfi.etag;
            fi.id = usfi.id;
            fi.modified_at = usfi.modified_at;
            fi.modified_by = usfi.modified_by;
            fi.name = usfi.name;
            fi.owned_by = usfi.owned_by;
            fi.parent = usfi.parent;
            fi.path = usfi.path;
            fi.path_id = usfi.path_id;
            fi.sequence_id = usfi.sequence_id;
            fi.share_link = null;
            fi.size = usfi.size;
            fi.type = usfi.type;
            return fi;
        }
        /// <summary>
        /// Delete file
        /// </summary>
        /// <param name="file_id"></param>
        public void DeleteFile(string file_id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/files/" + file_id);
            request.Method = "DELETE";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
        }
        /// <summary>
        /// Download file
        /// </summary>
        /// <param name="file_id"></param>
        /// <returns></returns>
        public string DownloadFile(string file_id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/files/" + file_id + "/data");
            request.Method = "GET";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            return reader.ReadToEnd();
        }
        /// <summary>
        /// Get Information About a File
        /// </summary>
        /// <param name="file_id"></param>
        /// <returns></returns>
        public FileInfo AboutFile(string file_id)
        {
            FileInfo fi = new FileInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/files/" + file_id);
            request.Method = "GET";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //shared
                fi = JsonConvert.DeserializeObject<FileInfo>(response);
            }
            catch (System.Exception)
            {
                //unshared
                fi = ConvertFileInfo(JsonConvert.DeserializeObject<UnSharedFileInfo>(response));
            }
            return fi;
        }
        /// <summary>
        /// Update File Information
        /// Serialization
        /// </summary>
        /// <param name="file_id"></param>
        /// <param name="fi"></param>
        public void UpdateFileInfo(string file_id, FileInfo fi)
        {

        }
        /// <summary>
        /// Shared file
        /// </summary>
        /// <param name="file_id"></param>
        /// <param name="access"></param>
        /// <param name="download"></param>
        /// <param name="preciew"></param>
        /// <returns></returns>
        public FileInfo ShareFile(string file_id, string access, bool download, bool preciew)
        {
            FileInfo fi = new FileInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/filse/" + file_id);
            request.Method = "PUT";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            string xml_request = "{\"shared_link\": {\"access\": \"open\"}}";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //shared
                fi = JsonConvert.DeserializeObject<FileInfo>(response);
            }
            catch (System.Exception)
            {
                //unshared
                fi = ConvertFileInfo(JsonConvert.DeserializeObject<UnSharedFileInfo>(response));
            }
            return fi;
        }
        /// <summary>
        /// Upload File
        /// </summary>
        /// <param name="parent_id"></param>
        /// <param name="data"></param>
        public FileInfo UploadFile(string parent_id, string path)
        {
            FileInfo fi = new FileInfo();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/files/data");
            request.Method = "POST";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //shared
                fi = JsonConvert.DeserializeObject<FileInfo>(response);
            }
            catch (System.Exception)
            {
                //unshared
                fi = ConvertFileInfo(JsonConvert.DeserializeObject<UnSharedFileInfo>(response));
            }
            return fi;
        }
        /// <summary>
        /// Get list of file comments
        /// </summary>
        /// <param name="file_id"></param>
        /// <returns></returns>
        public Comments CommmentFile(string file_id)
        {
            Comments lc = new Comments();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/files/" + file_id + "/comments");
            request.Method = "POST";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //with items
                lc = JsonConvert.DeserializeObject<Comments>(response);
            }
            catch (System.Exception)
            {

            }
            return lc;
        }
        /// <summary>
        /// Add to file comment
        /// </summary>
        /// <param name="file_id"></param>
        /// <param name="com"></param>
        public Comment AddCommentFile(string file_id, Comment comentry)
        {
            Comment comm = new Comment();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/files/" + file_id + "/comments");
            request.Method = "POST";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            string xml_request = "{\"message\":" + comentry + "\"}";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                //with items
                comm = JsonConvert.DeserializeObject<Comment>(response);
            }
            catch (System.Exception)
            {

            }
            return comm;
        }
        /// <summary>
        /// Delete comment
        /// </summary>
        /// <param name="comment_id"></param>
        public void DeleteComment(string comment_id)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/comments/" + comment_id);
            request.Method = "DELETE";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
        }
        /// <summary>
        /// Creating a Token for a User
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public Token UserToken(string email)
        {
            Token token = new Token();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(" https://api.box.com/2.0/tokens");
            request.Method = "POST";
            request.Headers.Add("Authorization", "BoxAuth api_key=" + api_key + "&auth_token=" + auth_token);
            string xml_request = "{\"email\":" + email + "\"}";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader reader = new StreamReader(request.GetResponse().GetResponseStream());
            string response = reader.ReadToEnd();
            try
            {
                token = JsonConvert.DeserializeObject<Token>(response);
            }
            catch (System.Exception)
            {

            }
            return token;
        }
    }
}
