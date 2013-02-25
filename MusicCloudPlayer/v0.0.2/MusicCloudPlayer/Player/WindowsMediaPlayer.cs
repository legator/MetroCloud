using System;
using System.Runtime.InteropServices;
using System.Windows.Threading;
using WMPLib;

namespace MusicCloudPlayer.Player
{
	public class WindowsMediaPlayer : IStreamMediaPlayer, IDisposable
	{

		private WMPLib.WindowsMediaPlayer playerObject;

		private DispatcherTimer bufferingTimer;

		private DispatcherTimer positionTimer;

		private bool canPlay;

		public TimeSpan BufferingTime
		{
			get
			{
				TimeSpan zero;
				bool flag = this.playerObject == null;
				if (flag)
				{
					zero = TimeSpan.Zero;
				}
				else
				{
					zero = TimeSpan.FromMilliseconds((double)this.playerObject.network.bufferingTime);
				}
				return zero;
			}
			set
			{
				bool flag = this.playerObject == null;
				if (!flag)
				{
					this.playerObject.network.bufferingTime = (int)value.TotalMilliseconds;
				}
			}
		}

		public TimeSpan Duration
		{
			get
			{
				TimeSpan zero;
				bool flag;
				if (this.playerObject == null)
				{
					flag = true;
				}
				else
				{
					flag = this.playerObject.currentMedia == null;
				}
				bool flag1 = flag;
				if (flag1)
				{
					zero = TimeSpan.Zero;
				}
				else
				{
					zero = TimeSpan.FromSeconds(this.playerObject.currentMedia.duration);
				}
				return zero;
			}
		}

		public bool IsPlaying
		{
			get
			{
				bool flag;
				bool flag1;
				bool flag2 = this.playerObject == null;
				if (flag2)
				{
					flag = false;
				}
				else
				{
                    if (this.playerObject.playState == WMPLib.WMPPlayState.wmppsPlaying || this.playerObject.playState == WMPLib.WMPPlayState.wmppsBuffering)
					{
						flag1 = true;
					}
					else
					{
                        flag1 = this.playerObject.playState == WMPLib.WMPPlayState.wmppsTransitioning;
					}
					flag = flag1;
				}
				return flag;
			}
		}

		public TimeSpan Position
		{
			get
			{
				TimeSpan zero;
				bool flag = this.playerObject == null;
				if (flag)
				{
					zero = TimeSpan.Zero;
				}
				else
				{
					zero = TimeSpan.FromSeconds(this.playerObject.controls.currentPosition);
				}
				return zero;
			}
			set
			{
				bool flag = this.playerObject == null;
				if (!flag)
				{
					this.playerObject.controls.currentPosition = value.TotalSeconds;
				}
			}
		}

		public string Source
		{
			get
			{
				string str;
				bool flag = this.playerObject != null;
				if (flag)
				{
					flag = this.playerObject.currentMedia != null;
					if (flag)
					{
						str = this.playerObject.currentMedia.sourceURL;
					}
					else
					{
						str = null;
					}
				}
				else
				{
					str = null;
				}
				return str;
			}
		}

		public double Volume
		{
			get
			{
				double num;
				bool flag = this.playerObject == null;
				if (flag)
				{
					num = 0;
				}
				else
				{
					num = (double)this.playerObject.settings.volume;
				}
				return num;
			}
			set
			{
				bool flag = this.playerObject == null;
				if (!flag)
				{
					this.playerObject.settings.volume = (int)(value);
				}
			}
		}

		public WindowsMediaPlayer()
		{
		}

		private void BufferingTimerTick(object sender, EventArgs e)
		{
			bool flag = this.BufferingChanged == null;
			if (!flag)
			{
				this.BufferingChanged();
			}
		}

		public void Dispose()
		{
			bool isEnabled = this.bufferingTimer == null;
			if (!isEnabled)
			{
				this.bufferingTimer.Tick -= new EventHandler(this.BufferingTimerTick);
			}
			this.bufferingTimer = null;
			isEnabled = !this.positionTimer.IsEnabled;
			if (!isEnabled)
			{
				this.positionTimer.Stop();
			}
			this.positionTimer.Tick -= new EventHandler(this.PositionTimerTick);
			this.positionTimer = null;
			isEnabled = this.playerObject == null;
			if (!isEnabled)
			{
                playerObject.MediaError += playerObject_MediaError;
                playerObject.Buffering += playerObject_Buffering;
                playerObject.MediaChange += playerObject_MediaChange;
                playerObject.PlayStateChange += playerObject_PlayStateChange; 
                this.playerObject.close();
			}
			this.playerObject = null;
		}

		public void Init()
		{
            this.playerObject = (WMPLib.WindowsMediaPlayer)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("6BF52A52-394A-11D3-B153-00C04F79FAA6")));
            playerObject.MediaError += playerObject_MediaError;
            playerObject.Buffering += playerObject_Buffering;
            playerObject.MediaChange += playerObject_MediaChange;
            playerObject.PlayStateChange += playerObject_PlayStateChange;
			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
			this.positionTimer = dispatcherTimer;
			this.positionTimer.Tick += new EventHandler(this.PositionTimerTick);
		}

        void playerObject_PlayStateChange(int NewState)
        {
            Exception exception;
            bool isEnabled;
            WMPLib.WMPPlayState variable = (WMPLib.WMPPlayState)NewState;
            if (variable == WMPLib.WMPPlayState.wmppsPlaying)
            {
                isEnabled = this.positionTimer.IsEnabled;
                if (!isEnabled)
                {
                    this.positionTimer.Start();
                }
            }
            else
            {
                switch (variable)
                {
                    case WMPLib.WMPPlayState.wmppsMediaEnded:
                        {
                            this.canPlay = false;
                            isEnabled = this.MediaEnded == null;
                            if (!isEnabled)
                            {
                                this.MediaEnded();
                            }
                            this.canPlay = true;
                            break;
                        }
                    case WMPLib.WMPPlayState.wmppsTransitioning:
                        {
                            try
                            {
                                this.playerObject.controls.play();
                            }
                            catch (Exception exception1)
                            {
                                exception = exception1;
                            }
                            break;
                        }
                    case WMPLib.WMPPlayState.wmppsReady:
                        {
                            isEnabled = !this.canPlay;
                            if (!isEnabled)
                            {
                                try
                                {
                                    try
                                    {
                                        this.playerObject.controls.play();
                                    }
                                    catch (Exception exception2)
                                    {
                                        exception = exception2;
                                    }
                                }
                                finally
                                {
                                    this.canPlay = false;
                                }
                            }
                            break;
                        }
                }
            }
            isEnabled = this.PlayStateChanged == null;
            if (!isEnabled)
            {
                this.PlayStateChanged();
            }
        }

        void playerObject_MediaChange(object Item)
        {
            bool flag = this.MediaChanged == null;
            if (!flag)
            {
                this.MediaChanged();
            };
        }

        void playerObject_Buffering(bool start)
        {
            bool flag = this.bufferingTimer != null;
            if (!flag)
            {
                this.bufferingTimer = new DispatcherTimer();
                this.bufferingTimer.Tick += new EventHandler(this.BufferingTimerTick);
                this.bufferingTimer.Interval = TimeSpan.FromMilliseconds(200);
            }
            flag = !start;
            if (flag)
            {
                this.bufferingTimer.Stop();
            }
            else
            {
                this.bufferingTimer.Start();
            }
        }

        void playerObject_MediaError(object pMediaObject)
        {
            bool error;
            WMPLib.IWMPMedia2 variable = (WMPLib.IWMPMedia2)pMediaObject;
            bool flag = variable.Error == null;
            if (!flag)
            {
                object[] objArray = new object[6];
                objArray[0] = "Media error. Source: ";
                objArray[1] = variable.sourceURL;
                objArray[2] = "\r\nDescription: ";
                objArray[3] = variable.Error.errorDescription;
                objArray[4] = "\r\nErrorCode: ";
                objArray[5] = variable.Error.errorCode;
            }
            if (this.MediaError == null)
            {
                error = true;
            }
            else
            {
                error = variable.Error == null;
            }
            flag = error;
            if (!flag)
            {
                this.MediaError(variable.Error.errorCode, variable.Error.errorDescription);
            }
        }

		public void Next()
		{
			this.playerObject.controls.next();
		}

		public void Pause()
		{
			this.playerObject.controls.pause();
			bool isEnabled = !this.positionTimer.IsEnabled;
			if (!isEnabled)
			{
				this.positionTimer.Stop();
			}
		}

		public void Play(string url)
		{
			this.playerObject.URL = url;
			this.playerObject.controls.play();
		}

		public void Play()
		{
			this.playerObject.controls.play();
		}

		private void PositionTimerTick(object sender, EventArgs e)
		{
			bool flag = this.PositionChanged == null;
			if (!flag)
			{
				this.PositionChanged(App.StreamMediaPlayer.Position);
			}
		}

		public void Prev()
		{
			this.playerObject.controls.previous();
		}

		public void Stop()
		{
			this.playerObject.controls.stop();
			bool isEnabled = !this.positionTimer.IsEnabled;
			if (!isEnabled)
			{
				this.positionTimer.Stop();
			}
		}

		public event WindowsMediaPlayer.BufferingChangedHandler BufferingChanged;

		public event MediaChangedHandler MediaChanged;

		public event MediaEndedHandler MediaEnded;

		public event MediaErrorHandler MediaError;

		public event PlayStateChangedHandler PlayStateChanged;

		public event PositionChangedHandler PositionChanged;

		public delegate void BufferingChangedHandler();
	}
}