using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.ComponentModel;
using System.Windows.Data;
using MusicCloudPlayer.Class;
using MusicCloudPlayer;
using Google.Apis.Drive.v2;
using Google.Apis.Drive.v2.Data;
using SchedulerTV.Resources.Class;
using Google.Apis.Authentication;
using Google.Apis.Authentication.OAuth2.DotNetOpenAuth;
using Google.Apis.Authentication.OAuth2;
using DotNetOpenAuth.OAuth2;
using Google.Apis.Samples.Helper;
using Google.Apis.Util;
using MusicCloudPlayer.Player;

namespace SchedulerTV.Resources.Model
{
    public class MainViewModel:ViewModelBase
    {
        private DriveService service;
        private static string ClientID = "";
        private static string ClientSecret = "";
        private string Key = "";
        public About User { get; set; }
        private List<GDAudio> AudioList;

        private bool islogged;
        public bool IsLogged
        {
            get
            {
                bool flag = this.islogged;
                return flag;
            }
            set
            {
                bool flag = this.islogged != value;
                if (flag)
                {
                    this.islogged = value;
                    base.RaisePropertyChanged("IsLogged");
                }
            }
        }

        private bool islist;
        public bool IsList
        {
            get
            {
                bool flag = this.islist;
                return flag;
            }
            set
            {
                bool flag = this.islist != value;
                if (flag)
                {
                    this.islist = value;
                    base.RaisePropertyChanged("IsList");
                }
            }
        }

        private bool Issearch;
        public bool IsSearch
        {
            get
            {
                bool flag = this.Issearch;
                return flag;
            }
            set
            {
                bool flag = this.Issearch != value;
                if (flag)
                {
                    this.Issearch = value;
                    base.RaisePropertyChanged("IsSearch");
                }
            }
        }

        private bool Isplay;
        public bool IsPlay
        {
            get
            {
                bool flag = this.Isplay;
                return flag;
            }
            set
            {
                bool flag = this.Isplay != value;
                if (flag)
                {
                    this.Isplay = value;
                    base.RaisePropertyChanged("IsPlay");
                }
            }
        }

        private bool Istop;
        public bool IsTop
        {
            get
            {
                bool flag = this.Istop;
                return flag;
            }
            set
            {
                bool flag = this.Istop != value;
                if (flag)
                {
                    this.Istop = value;
                    base.RaisePropertyChanged("IsTop");
                }
            }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value)
                    return;
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string owner;
        public string Owner
        {
            get { return owner; }
            set
            {
                if (owner == value)
                    return;
                owner = value;
                RaisePropertyChanged("Owner");
            }
        }

        private GDAudio currentAudio;
        public GDAudio CurrentAudio
        {
            get { return currentAudio; }
            set
            {
                if (currentAudio == value)
                    return;
                currentAudio = value;
                RaisePropertyChanged("CurrentAudio");
            }
        }

        private ObservableCollection<GDAudio> cloudAudio;
        public ObservableCollection<GDAudio> CloudAudio
        {
            get { return cloudAudio; }
            set
            {
                if (cloudAudio == value)
                    return;
                cloudAudio = value;
                CloudAudioSelectedIndex = -1;
                RaisePropertyChanged("CloudAudio");
            }
        }

        private int cloudAudioSelectedIndex = -1;
        public int CloudAudioSelectedIndex
        {
            get { return cloudAudioSelectedIndex; }
            set
            {
                if (cloudAudioSelectedIndex == value)
                    return;
                cloudAudioSelectedIndex = value;
                RaisePropertyChanged("CloudAudioSelectedIndex");
            }
        }

        private ObservableCollection<GDAudio> searchResults;
        public ObservableCollection<GDAudio> SearchResults
        {
            get { return searchResults; }
            set
            {
                if (searchResults == value)
                    return;
                searchResults = value;
                SearchSelectedIndex = -1;
                RaisePropertyChanged("SearchResults");
            }
        }

        private int searchSelectedIndex = -1;
        public int SearchSelectedIndex
        {
            get { return searchSelectedIndex; }
            set
            {
                if (searchSelectedIndex == value)
                    return;
                searchSelectedIndex = value;
                RaisePropertyChanged("SearchSelectedIndex");
            }
        }

        private int volume = 50;
        public int Volume
        {
            get
            {
                return volume;
            }
            set
            {
                volume = value;
                MusicCloudPlayer.App.StreamMediaPlayer.Volume = Convert.ToDouble(value / 100);
                RaisePropertyChanged("Volume");
            }
        }

        private TimeSpan duration;
        public TimeSpan CurrentDuration
        {
            get
            {
                TimeSpan timeSpan = this.duration;
                return timeSpan;
            }
            set
            {
                bool flag = !(this.duration == value);
                if (flag)
                {
                    this.duration = value;
                    base.RaisePropertyChanged("CurrentDuration");
                }
            }
        }

        private TimeSpan timeLeft;
        public TimeSpan TimeLeft
        {
            get
            {
                TimeSpan timeSpan = this.timeLeft;
                return timeSpan;
            }
            set
            {
                bool flag = !(this.timeLeft == value);
                if (flag)
                {
                    this.timeLeft = value;
                    base.RaisePropertyChanged("TimeLeft");
                }
            }
        }

        public TimeSpan Position
        {
            get
            {
                TimeSpan position = MusicCloudPlayer.App.StreamMediaPlayer.Position;
                return position;
            }
            set
            {
                TimeSpan position = MusicCloudPlayer.App.StreamMediaPlayer.Position;
                bool flag = Math.Abs(position.TotalMilliseconds - value.TotalMilliseconds) < 1000;
                if (!flag)
                {
                    MusicCloudPlayer.App.StreamMediaPlayer.Position = value;
                }
                base.RaisePropertyChanged("Position");
            }
        }

        private int buffering;
        public int Buffering
        {
            get
            {
                int num = this.buffering;
                return num;
            }
            set
            {
                bool flag = this.buffering != value;
                if (flag)
                {
                    this.buffering = value;
                    base.RaisePropertyChanged("Buffering");
                }
            }
        }

        private string trackTimeString;
        public string TrackTimeString
        {
            get
            {
                string str = this.trackTimeString;
                return str;
            }
            set
            {
                bool flag = !(this.trackTimeString == value);
                if (flag)
                {
                    this.trackTimeString = value;
                    base.RaisePropertyChanged("TrackTimeString");
                }
            }
        }

        public void Play()
        {
            if (CloudAudio==null)
            {
                return;
            }
            if (CloudAudioSelectedIndex==-1)
            {
                CloudAudioSelectedIndex = 0;
            }
            PlayIndex();
        }

        private void PlayIndex()
        {
            Position = TimeSpan.Zero;
            CurrentAudio = CloudAudio[CloudAudioSelectedIndex];
            Name = CurrentAudio.Name;
            Owner = CurrentAudio.Owner;
            
            Uri url = new Uri(CurrentAudio.Url);
            MusicCloudPlayer.App.StreamMediaPlayer.Play(url.ToString());
        }

        public void Pause()
        {
            MusicCloudPlayer.App.StreamMediaPlayer.Pause();
        }

        public void PlayNext()
        {
            CloudAudioSelectedIndex++;
            if (CloudAudioSelectedIndex>=CloudAudio.Count)
            {
                CloudAudioSelectedIndex = 0;
            }
            PlayIndex();
        }

        public void PlayByIndex(int index)
        {
            if (index<0 || index>=CloudAudio.Count)
            {
                return;
            }
            else { CloudAudioSelectedIndex = index; PlayIndex(); }
        }

        public void PlayPrev()
        {
            CloudAudioSelectedIndex--;
            if (CloudAudioSelectedIndex<0)
            {
                CloudAudioSelectedIndex = CloudAudio.Count - 1;
            }
            PlayIndex();
        }

        public MainViewModel()
        {
            IsList = IsSearch = IsTop = false;
        }

        public void Auth()
        {
            AsyncThread.InvokeAsync(() =>
                                         {
                                            service = new DriveService(CreateAuthenticator());
                                            LoadAudio();
                                         });
            MusicCloudPlayer.Properties.Settings.Default.Logged = true;
            MusicCloudPlayer.Properties.Settings.Default.Save();
        }

        public void LogOff()
        {
            service = null;
            CloudAudio = null;
            MusicCloudPlayer.Properties.Settings.Default.Logged = false;
            MusicCloudPlayer.Properties.Settings.Default.Save();
        }

        private IAuthenticator CreateAuthenticator()
        {
            var provider = new NativeApplicationClient(GoogleAuthenticationServer.Description);
            provider.ClientIdentifier = ClientID;
            provider.ClientSecret = ClientSecret;
            return new OAuth2Authenticator<NativeApplicationClient>(provider, GetAuthentication);
        }

        private IAuthorizationState GetAuthentication(NativeApplicationClient client)
        {
            // You should use a more secure way of storing the key here as
            // .NET applications can be disassembled using a reflection tool.
            const string STORAGE = "google.musicplayer.dotnet.drive";
            const string KEY = "y},drdzf11x9;87";
            string scope = DriveService.Scopes.Drive.GetStringValue();

            // Check if there is a cached refresh token available.
            IAuthorizationState state = AuthorizationMgr.GetCachedRefreshToken(STORAGE, KEY);
            if (state != null)
            {
                try
                {
                    client.RefreshToken(state);
                    return state; // Yes - we are done.
                }
                catch (DotNetOpenAuth.Messaging.ProtocolException ex)
                {

                }
            }
            // Retrieve the authorization from the user.
            state = AuthorizationMgr.RequestNativeAuthorization(client, scope);
            AuthorizationMgr.SetCachedRefreshToken(STORAGE, KEY, state);
            return state;
        }

        private void LoadAudio()
        {
            List<Google.Apis.Drive.v2.Data.File> lfile = new List<Google.Apis.Drive.v2.Data.File>();
            AudioList = new List<GDAudio>();
            lfile = LoadFile();
            foreach (Google.Apis.Drive.v2.Data.File item in lfile)
            {
                if (item.MimeType == "audio/mpeg")
                {
                    GDAudio a = new GDAudio();
                    a.id = item.Id;
                    a.Url = item.DownloadUrl;
                    a.Name = item.Title;
                    a.Owner = "";
                    foreach (string it in item.OwnerNames)
                    {
                        a.Owner = it + "/" + a.Owner;
                    }
                    AudioList.Add(a);
                }
            }
            count = AudioList.Count;
            CloudAudio = new ObservableCollection<GDAudio>(AudioList);
        }

        public int count = 0;

        private List<Google.Apis.Drive.v2.Data.File> LoadFile()
        {
            List<Google.Apis.Drive.v2.Data.File> result = new List<Google.Apis.Drive.v2.Data.File>();
            FilesResource.ListRequest request = service.Files.List();
            do
            {
                try
                {
                    FileList files = request.Fetch();
                    result.AddRange(files.Items);
                    request.PageToken = files.NextPageToken;
                }
                catch (Exception e)
                {
                    //Console.WriteLine("An error occurred: " + e.Message);
                    request.PageToken = null;
                }
            } while (!String.IsNullOrEmpty(request.PageToken));
            return result;
        }

        public void Search(string query)
        {
            AsyncThread.InvokeAsync(() =>
                {
                    List<GDAudio> SearchList = new List<GDAudio>();
                    if (AudioList!=null)
                    {
                        foreach (GDAudio item in AudioList)
                        {
                            if (item.Name.IndexOf(query) != -1)
                            {
                                searchResults.Add(item);
                            }
                        }
                    }
                    SearchResults = new ObservableCollection<GDAudio>(SearchList);
                });
        }

        public void Load()
        {
            MusicCloudPlayer.App.StreamMediaPlayer.PositionChanged += new PositionChangedHandler(this.PlayerPositionChanged);
            MusicCloudPlayer.App.StreamMediaPlayer.MediaChanged += new MediaChangedHandler(this.PlayerMediaChanged);
            MusicCloudPlayer.App.StreamMediaPlayer.MediaEnded += new MediaEndedHandler(this.PlayerMediaEnded);
            MusicCloudPlayer.App.StreamMediaPlayer.PlayStateChanged += new PlayStateChangedHandler(this.PlayerPlayStateChanged);
            MusicCloudPlayer.App.StreamMediaPlayer.MediaError += new MediaErrorHandler(this.StreamMediaPlayerMediaError);
        }

        private void StreamMediaPlayerMediaError(int errorCode, string errorDescription)
        {
            throw new NotImplementedException();
        }

        private void PlayerPlayStateChanged()
        {
            IsPlay = MusicCloudPlayer.App.StreamMediaPlayer.IsPlaying;
        }

        private void PlayerMediaEnded()
        {
            PlayNext();
        }

        private void PlayerMediaChanged()
        {
            this.CurrentDuration = MusicCloudPlayer.App.StreamMediaPlayer.Duration;
        }

        private void PlayerPositionChanged(TimeSpan newPosition)
        {
            string str;
            string str1 = "m\\:ss";
            TimeSpan currentDuration = this.CurrentDuration;
            bool seconds = currentDuration.Hours < 1;
            if (!seconds)
            {
                str1 = "h\\:mm\\:ss";
            }
            this.Position = newPosition;
            this.CurrentDuration = MusicCloudPlayer.App.StreamMediaPlayer.Duration;
            MainViewModel mainViewModel = this;
            if (this.CurrentDuration > TimeSpan.Zero)
            {
                currentDuration = this.Position;
                currentDuration = this.CurrentDuration;
                str = string.Format("{0} / {1}", currentDuration.ToString(str1), currentDuration.ToString(str1));
            }
            else
            {
                currentDuration = this.Position;
                str = currentDuration.ToString(str1);
            }
            mainViewModel.TrackTimeString = str;
        }
    }
}
