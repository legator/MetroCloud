using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace SugarSync
{
    public class SugarSyncAPI
    {
        public List<Contact> ContactList = new List<Contact>();
        public List<ReceivedShare> sharedfolder = new List<ReceivedShare>();
        public List<workspaces> syncfoldercoll = new List<workspaces>();
        public List<workspaces> workspaceslist = new List<workspaces>();
        public List<workspace> workspacelist = new List<workspace>();
        public List<workspaces> workspacescontent = new List<workspaces>();
        public User userInfo = new User();
        public string user { get; set; }
        public string refreshToken { get; set; }
        public string accessToken { get; set; }
        private string application = "/sc/2580395/253_188802672";
        private string accessKeyId = "MjU4MDM5NTEzMjkxNDk3MzEyNTA";
        private string privateAccessKey = "MTFiMDJkOGYwZjEwNDQ2ZmJlY2IwMWQwYTczZGUzYjM";
        /// <summary>
        /// set_app_config
        /// </summary>
        /// <param name="app"></param>
        /// <param name="keyid"></param>
        /// <param name="accesskey"></param>
        public void set_app_config(string app,string keyid,string accesskey)
        {
            application = app;
            accessKeyId = keyid;
            privateAccessKey = accesskey;
        }
        /// <summary>
        /// app-authorization 
        /// </summary>
        public void app_authorization(string user, string pass)
        {
            HttpWebRequest request = WebRequest.Create("https://api.sugarsync.com/app-authorization") as HttpWebRequest;
            request.Method = "POST";
            string xml_request = "<?xml version='1.0' encoding='UTF-8' ?>" +
                "<appAuthorization>" +
                    "<username>" + user + "</username>" +
                    "<password>" + pass + "</password>" +
                    "<application>" + application + "</application>" +
                    "<accessKeyId>" + accessKeyId + "</accessKeyId>" +
                    "<privateAccessKey>" + privateAccessKey + "</privateAccessKey>" +
                "</appAuthorization>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                refreshToken = response.Headers["Location"].ToString();
            }
        }
        /// <summary>
        /// authorization 
        /// </summary>
        public void authorization()
        {
            HttpWebRequest request = WebRequest.Create("https://api.sugarsync.com/authorization") as HttpWebRequest;
            request.Method = "POST";
            string xml_request = "<?xml version='1.0' encoding='UTF-8' ?>" +
                "<tokenAuthRequest>" +
                    "<accessKeyId>" + accessKeyId + "</accessKeyId>" +
                    "<privateAccessKey>" + privateAccessKey + "</privateAccessKey>" +
                    "<refreshToken>" + refreshToken + "</refreshToken>" +
                "</tokenAuthRequest>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                accessToken = response.Headers["Location"].ToString();

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//user"))
                {
                    user = xitem.InnerText;
                }
            }
        }
        ///<summary>
        /// Retrieving User Information
        ///</summary>
        public void user_info()
        {
            HttpWebRequest request = WebRequest.Create(user) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//username"))
                {
                    userInfo.username = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//nickname"))
                {
                    userInfo.nickname = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//limit"))
                {
                    userInfo.quota_limit = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//usage"))
                {
                    userInfo.quota_usage = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//workspaces"))
                {
                    userInfo.workspaces = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//syncfolder"))
                {
                    userInfo.syncfolder = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//deleted"))
                {
                    userInfo.deleted = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//magicBriefcase"))
                {
                    userInfo.magicBriefcase = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//webArchive"))
                {
                    userInfo.webArchive = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//mobilePhoto"))
                {
                    userInfo.mobilePhoto = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//albums"))
                {
                    userInfo.albums = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//recentActivities"))
                {
                    userInfo.recentActivities = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//publicLinks"))
                {
                    userInfo.publicLinks = xitem.InnerText;
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//maximumPublicLinkSize"))
                {
                    userInfo.maximumPublicLinkSize = xitem.InnerText;
                }
            }
        }
        ///<summary>
        ///Retrieving Workspaces Collection
        ///</summary>
        public void workspaces_coll()
        {
            HttpWebRequest request = WebRequest.Create(user + "/workspaces/contents") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//collection"))
                {
                    workspaces ws = new workspaces();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        ws.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        ws.contents = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//iconId"))
                    {
                        ws.iconId = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        ws.reference = citem.InnerText;
                    }
                    workspaceslist.Add(ws);
                }
            }
        }
        ///<summary>
        ///Retrieving Workspace Information
        ///</summary>
        public void workspaceInfo(string workspace_resource)
        {
            HttpWebRequest request = WebRequest.Create(workspace_resource) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//collection"))
                {
                    workspace ws = new workspace();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        ws.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//dsid"))
                    {
                        ws.dsid = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//iconId"))
                    {
                        ws.iconId = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        ws.contents = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//timeCreated"))
                    {
                        ws.timeCreated = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//collections"))
                    {
                        ws.collections = citem.InnerText;
                    }
                    workspacelist.Add(ws);
                }
            }
        }
        ///<summary>
        ///Retrieving Workspace Contents
        ///</summary>
        public void workcontent(string workspace_resource)
        {
            HttpWebRequest request = WebRequest.Create(workspace_resource + "/contents") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//collection"))
                {
                    workspaces ws = new workspaces();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        ws.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        ws.reference = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        ws.contents = citem.InnerText;
                    }
                    workspacescontent.Add(ws);
                }
            }
        }
        ///<summary>
        ///Updating Workspace Information
        ///</summary>
        public void updateworkspace(string workspace_resource, workspace ws)
        {
            HttpWebRequest request = WebRequest.Create(workspace_resource) as HttpWebRequest;
            request.Method = "PUT";
            string xml_request = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>" +
                "<workspace>" +
                    "<displayName>" + ws.displayName + "</displayName>" +
                    "<dsid>" + ws.dsid + "</dsid>" +
                    "<timeCreated>" + ws.timeCreated + "</timeCreated>" +
                    "<collections>" + ws.collections + "</collections>" +
                    "<files>" + ws.contents + "?type=file" + "</files>" +
                    "<contents>" + ws.contents + "</contents>" +
                    "<iconId>" + ws.iconId + "</iconId>" +
                "</workspace>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Retrieving Sync Folders Collection
        ///</summary>
        public void SyncFoldersColl()
        {
            HttpWebRequest request = WebRequest.Create(user + "/folders/contents") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//collection"))
                {
                    workspaces ws = new workspaces();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        ws.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        ws.reference = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        ws.contents = citem.InnerText;
                    }
                    syncfoldercoll.Add(ws);
                }
            }
        }
        ///<summary>
        ///Retrieving Folders
        ///</summary>
        public List<workspaces> folders(string url)
        {
            List<workspaces> folderlist = new List<workspaces>();
            HttpWebRequest request = WebRequest.Create(url + "/contents?type=folder") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//collection"))
                {
                    workspaces ws = new workspaces();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        ws.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        ws.reference = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        ws.contents = citem.InnerText;
                    }
                    folderlist.Add(ws);
                }
            }
            return folderlist;
        }
        ///<summary>
        ///Retrieving Folder Information
        ///Return Folder
        ///</summary>
        public Folder folderinfo(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                Folder f = new Folder();
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//folder"))
                {
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        f.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//dsid"))
                    {
                        f.dsid = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//timeCreated"))
                    {
                        f.timeCreated = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//collections"))
                    {
                        f.collections = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//files"))
                    {
                        f.files = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        f.contents = citem.InnerText;
                    }
                }
                return f;
            }
        }
        ///<summary>
        ///Retrieving Folder Contents
        ///How to return
        ///</summary>
        public void foldercontent(string url)
        {
            HttpWebRequest request = WebRequest.Create(url + "/contents") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                List<File> lfil = new List<File>();
                List<Folder> lfol = new List<Folder>();
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//file"))
                {
                    File f = new File();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        f.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        f.reference = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//size"))
                    {
                        f.size = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//lastModified"))
                    {
                        f.lastModified = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//mediaType"))
                    {
                        f.mediaType = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//presentOnServer"))
                    {
                        f.presentOnServer = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//fileData"))
                    {
                        f.fileData = citem.InnerText;
                    }
                    lfil.Add(f);
                }
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//collection"))
                {
                    Folder f = new Folder();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        f.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        f.reference = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        f.contents = citem.InnerText;
                    }
                    lfol.Add(f);
                }
            }
        }
        ///<summary>
        ///Creating a Folder
        ///</summary>
        public void CreateFolder(string path, string name)
        {
            HttpWebRequest request = WebRequest.Create(path) as HttpWebRequest;
            request.Method = "POST";
            request.Headers.Add("Authorization", accessToken);
            string xml_request = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>" +
                "<folder>" +
                    "<displayName>" + name + "</displayName>" +
                "</folder>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Creating a File in a Folder
        ///Return path to file
        ///</summary>
        public string CreateFile(string path, string name, string mediatype)
        {
            HttpWebRequest request = WebRequest.Create(path) as HttpWebRequest;
            request.Method = "POST";
            request.Headers.Add("Authorization", accessToken);
            string xml_request = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>" +
                "<folder>" +
                    "<displayName>" + name + "</displayName>" +
                    "<mediaType>" + name + "</mediaType>" +
                "</folder>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return response.Headers["Location"].ToString();
            }
        }
        ///<summary>
        ///Copying a File
        ///Return path to file
        ///</summary>
        public string CopyFile(string toPath, string urlFile, string copyName)
        {
            HttpWebRequest request = WebRequest.Create(toPath) as HttpWebRequest;
            request.Method = "POST";
            request.Headers.Add("Authorization", accessToken);
            string xml_request = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>" +
                "<fileCopy source='" + urlFile + "'>" +
                    "<displayName>" + copyName + "</displayName>" +
                "</fileCopy>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return response.Headers["Location"].ToString();
            }
        }
        ///<summary>
        ///Deleting a Folder
        ///</summary>
        public void DeleteFOlder(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "DELETE";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Updating Folder Information
        ///</summary>
        public void UpdateFolderInfo(string url, Folder fol)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "PUT";
            request.Headers.Add("Authorization", accessToken);
            string xml_request = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>" +
                "<folder>" +
                    "<displayName>" + fol.displayName + "</displayName>" +
                    "<dsid>" + fol.dsid + "</dsid>" +
                    "<timeCreated>" + fol.timeCreated + "</timeCreated>" +
                    "<parent>" + fol.parent + "</parent>" +
                    "<collections>" + fol.collections + "</collections>" +
                    "<files>" + fol.files + "</files>" +
                    "<contents>" + fol.contents + "</contents>" +
                    "<sharing enabled='" + fol.sharing + "'/>" +
                "</folder>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Retrieving Received Share Information
        ///</summary>
        public ReceivedShare ShareInfo(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            ReceivedShare rs = new ReceivedShare();
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//receivedShare "))
                {
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        rs.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//timeReceived"))
                    {
                        rs.timeReceived = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//sharedFolder"))
                    {
                        rs.sharedFolder = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        rs.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        rs.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//owner"))
                    {
                        rs.owner = citem.InnerText;
                    }
                }
            }
            return rs;
        }
        ///<summary>
        ///Retrieving the Received Shares List
        ///</summary>
        public void ShareList()
        {
            HttpWebRequest request = WebRequest.Create(user + "/receivedShares/contents") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//receivedShare "))
                {
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    ReceivedShare rs = new ReceivedShare();
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        rs.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//timeReceived"))
                    {
                        rs.timeReceived = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//sharedFolder"))
                    {
                        rs.sharedFolder = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        rs.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        rs.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//owner"))
                    {
                        rs.owner = citem.InnerText;
                    }
                    sharedfolder.Add(rs);
                }
            }
        }
        ///<summary>
        ///Retrieving Contact Information
        ///</summary>
        public void ContactInfo(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());

                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//contact"))
                {
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    Contact c = new Contact();
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//primaryEmailAddress"))
                    {
                        c.primaryEmailAddress = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//firstName"))
                    {
                        c.firstName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//lastName"))
                    {
                        c.lastName = citem.InnerText;
                    }
                    ContactList.Add(c);
                }
            }
        }
        ///<summary>
        ///Retrieving File Information
        ///</summary>
        public File FileInfo(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                File f = new File();
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//file"))
                {
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        f.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//dsid"))
                    {
                        f.dsid = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//timeCreated"))
                    {
                        f.timeCreated = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//parent"))
                    {
                        f.parent = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//size"))
                    {
                        f.size = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//lastModified"))
                    {
                        f.lastModified = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//mediaType"))
                    {
                        f.mediaType = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//presentOnServer"))
                    {
                        f.presentOnServer = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//fileData"))
                    {
                        f.fileData = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//versions"))
                    {
                        f.versions = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//publicLink"))
                    {
                        f.publicLink = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//height"))
                    {
                        f.image_height = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//width"))
                    {
                        f.image_width = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//rotation"))
                    {
                        f.image_rotation = citem.InnerText;
                    }
                }
                return f;
            }
        }
        ///<summary>
        ///Public File link
        ///</summary>
        public string PublicFileLink(string url)
        {
            string xml = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><file><publicLink enabled=\"true\"/></file>";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "PUT";
            request.Headers.Add("Authorization", accessToken);

            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            StreamReader sr2 = new StreamReader(request.GetResponse().GetResponseStream());
            return sr2.ReadToEnd();
        }
        ///<summary>
        ///Updating File Information
        ///</summary>
        public void UpdateFileInfo(string url, File fil)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "PUT";
            request.Headers.Add("Authorization", accessToken);
            string xml_request = "<?xml version='1.0' encoding='UTF-8' standalone='yes'?>" +
                "<file>" +
                    "<displayName>" + fil.displayName + "</displayName>" +
                    "<dsid>" + fil.dsid + "</dsid>" +
                    "<timeCreated>" + fil.timeCreated + "</timeCreated>" +
                    "<parent>" + fil.parent + "</parent>" +
                    "<size>" + fil.size + "</size>" +
                    "<lastModified>" + fil.lastModified + "</lastModified>" +
                    "<mediaType>" + fil.mediaType + "</mediaType>" +
                    "<presentOnServer>" + fil.mediaType + "</presentOnServer>" +
                    "<fileData>" + fil.fileData + "</fileData>" +
                    "<versions>" + fil.versions + "</versions>" +
                    "<sharing enabled='" + fil.sharing + "'/>" +
                    "<image>" +
                    "<height>" + fil.image_height + "</height>" +
                    "<width>" + fil.image_width + "</width>" +
                    "<rotation>" + fil.image_rotation + "</rotation>" +
                    "</image>" +
                "</file>";
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(xml_request);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Deleting a File
        ///</summary>
        public void DeleteFile(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "DELETE";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Retrieving Version History
        ///</summary>
        public List<FileVer> FileVerHistory(string url)
        {
            List<FileVer> lfv = new List<FileVer>();
            HttpWebRequest request = WebRequest.Create(url + "/version") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//fileVersion"))
                {
                    XmlDocument wdoc = new XmlDocument();
                    FileVer f = new FileVer();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//size"))
                    {
                        f.size = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//lastModified"))
                    {
                        f.lastModified = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//mediaType"))
                    {
                        f.mediaType = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//presentOnServer"))
                    {
                        f.presentOnServer = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//fileData"))
                    {
                        f.fileData = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        f.reference = citem.InnerText;
                    }
                    lfv.Add(f);
                }
                return lfv;
            }
        }
        ///<summary>
        ///Creating a New File Version
        ///</summary>
        public void NewFileVer(string url)
        {
            HttpWebRequest request = WebRequest.Create(url + "/version") as HttpWebRequest;
            request.Method = "POST";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Retrieving File Data
        ///Returned data
        ///</summary>
        public string FileData(string url)
        {
            HttpWebRequest request = WebRequest.Create(url + "/data") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return reader.ReadToEnd();
            }
        }
        ///<summary>
        ///Uploading File Data
        ///</summary>
        public void UploadFileData(string url, string FData)
        {
            HttpWebRequest request = WebRequest.Create(url + "/data") as HttpWebRequest;
            request.Method = "PUT";
            request.Headers.Add("Authorization", accessToken);
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(FData);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Transcoding Images
        ///</summary>
        ///
        ///<summary>
        ///Retrieving File Version Information
        ///Return FileVer
        ///</summary>
        public FileVer FileVerInfo(string url, string idversion)
        {
            FileVer fv = new FileVer();
            HttpWebRequest request = WebRequest.Create(url + "/version/" + idversion) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//fileVersion"))
                {
                    XmlDocument wdoc = new XmlDocument();

                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//size"))
                    {
                        fv.size = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//lastModified"))
                    {
                        fv.lastModified = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//mediaType"))
                    {
                        fv.mediaType = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//presentOnServer"))
                    {
                        fv.presentOnServer = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//fileData"))
                    {
                        fv.fileData = citem.InnerText;
                    }
                }
            }
            return fv;
        }
        ///<summary>
        ///Retrieving File Version Data
        ///</summary>
        public string FileVerData(string url, string idversion)
        {
            FileVer fv = new FileVer();
            HttpWebRequest request = WebRequest.Create(url + "/version/" + idversion + "/data") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                return reader.ReadToEnd();
            }
        }
        ///<summary>
        ///Uploading File Version Data
        ///</summary>
        public void UploadFileVerData(string url, string idversion, string FData)
        {
            HttpWebRequest request = WebRequest.Create(url + "/version/" + idversion + "/data") as HttpWebRequest;
            request.Method = "PUT";
            request.Headers.Add("Authorization", accessToken);
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(FData);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
            }
        }
        ///<summary>
        ///Retrieving Albums Collection
        ///</summary>
        public List<workspaces> AlbumColl()
        {
            List<workspaces> lw = new List<workspaces>();
            HttpWebRequest request = WebRequest.Create(user + "/albums/contents") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//collection"))
                {
                    workspaces ws = new workspaces();
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        ws.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        ws.contents = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        ws.reference = citem.InnerText;
                    }
                    lw.Add(ws);
                }
            }
            return lw;
        }
        ///<summary>
        ///Retrieving Album Information
        ///</summary>
        public Album AlbumInfo(string url)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            Album al = new Album();
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//album"))
                {
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        al.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//dsid"))
                    {
                        al.dsid = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//files"))
                    {
                        al.files = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//contents"))
                    {
                        al.contents = citem.InnerText;
                    }
                }
            }
            return al;
        }
        ///<summary>
        ///Retrieving Album Contents
        ///</summary>
        public File AlbumContents(string url)
        {
            HttpWebRequest request = WebRequest.Create(url + "/data") as HttpWebRequest;
            request.Method = "GET";
            request.Headers.Add("Authorization", accessToken);
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());

                XmlDocument doc = new XmlDocument();
                doc.LoadXml(reader.ReadToEnd());
                File f = new File();
                foreach (XmlElement xitem in doc.DocumentElement.SelectNodes("//file"))
                {
                    XmlDocument wdoc = new XmlDocument();
                    wdoc.LoadXml("<root>" + xitem.InnerXml + "</root>");
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//displayName"))
                    {
                        f.displayName = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//ref"))
                    {
                        f.reference = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//size"))
                    {
                        f.size = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//lastModified"))
                    {
                        f.lastModified = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//presentOnServer"))
                    {
                        f.presentOnServer = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//fileData"))
                    {
                        f.fileData = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//mediaType"))
                    {
                        f.mediaType = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//presentOnServer"))
                    {
                        f.presentOnServer = citem.InnerText;
                    }
                    foreach (XmlElement citem in wdoc.DocumentElement.SelectNodes("//fileData"))
                    {
                        f.fileData = citem.InnerText;
                    }
                }
                return f;
            }
        }
    }

    /// <summary>
    /// class about user
    /// </summary>
    public class User
    {
        public string username { get; set; }
        public string nickname { get; set; }
        public string quota_limit { get; set; }
        public string quota_usage { get; set; }
        public string workspaces { get; set; }
        public string syncfolder { get; set; }
        public string deleted { get; set; }
        public string magicBriefcase { get; set; }
        public string webArchive { get; set; }
        public string mobilePhoto { get; set; }
        public string albums { get; set; }
        public string recentActivities { get; set; }
        public string publicLinks { get; set; }
        public string maximumPublicLinkSize { get; set; }
    }
    ///<summary>
    /// Contact class
    /// </summary>
    public class Contact
    {
        public string primaryEmailAddress { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
    }
    ///<summary>
    ///class about workspaces collection
    ///</summary>
    public class workspaces
    {
        public string displayName { get; set; }
        public string reference { get; set; }
        public string iconId { get; set; }
        public string contents { get; set; }

    }
    ///<summary>
    ///class about workspace
    ///</summary>
    public class workspace
    {
        public string displayName { get; set; }
        public string dsid { get; set; }
        public string iconId { get; set; }
        public string contents { get; set; }
        public string timeCreated { get; set; }
        public string collections { get; set; }
    }
    ///<summary>
    ///class about workspace
    ///</summary>
    public class Folder
    {
        public string displayName { get; set; }
        public string reference { get; set; }
        public string dsid { get; set; }
        public string parent { get; set; }
        public string files { get; set; }
        public string contents { get; set; }
        public string timeCreated { get; set; }
        public string collections { get; set; }
        public string sharing { get; set; }
    }
    ///<summary>
    ///class about File
    ///</summary>
    public class File
    {
        public string displayName { get; set; }
        public string reference { get; set; }
        public string dsid { get; set; }
        public string timeCreated { get; set; }
        public string parent { get; set; }
        public string size { get; set; }
        public string sharing { get; set; }
        public string lastModified { get; set; }
        public string mediaType { get; set; }
        public string presentOnServer { get; set; }
        public string fileData { get; set; }
        public string versions { get; set; }
        public string publicLink { get; set; }
        public string image_height { get; set; }
        public string image_width { get; set; }
        public string image_rotation { get; set; }
    }
    ///<summary>
    ///class about File version
    ///</summary>
    public class FileVer
    {
        public string size { get; set; }
        public string lastModified { get; set; }
        public string mediaType { get; set; }
        public string presentOnServer { get; set; }
        public string fileData { get; set; }
        public string reference { get; set; }
    }
    ///<summary>
    ///class about receivedShare 
    ///</summary>
    public class ReceivedShare
    {
        public string displayName { get; set; }
        public string timeReceived { get; set; }
        public string sharedFolder { get; set; }
        public string readAllowed { get; set; }
        public string writeAllowed { get; set; }
        public string owner { get; set; }
    }
    ///<summary>
    ///class about Album 
    ///</summary>
    public class Album
    {
        public string displayName { get; set; }
        public string dsid { get; set; }
        public string files { get; set; }
        public string contents { get; set; }
    }
}
