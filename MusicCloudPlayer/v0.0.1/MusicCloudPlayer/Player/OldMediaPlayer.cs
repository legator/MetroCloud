using MusicCloudPlayer;
using MusicCloudPlayer.Player;
using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;

namespace MusicCloudPlayer.Player
{
	public class OldMediaPlayer : IStreamMediaPlayer, IDisposable
	{
		private MediaPlayer playerObject;

		private bool isPlaying;

		private DispatcherTimer positionTimer;
        
		public TimeSpan Duration
		{
			get
			{
				TimeSpan zero;
				Duration naturalDuration = this.playerObject.NaturalDuration;
				bool hasTimeSpan = !naturalDuration.HasTimeSpan;
				if (hasTimeSpan)
				{
					zero = TimeSpan.Zero;
				}
				else
				{
					naturalDuration = this.playerObject.NaturalDuration;
					zero = naturalDuration.TimeSpan;
				}
				return zero;
			}
		}

		public bool IsPlaying
		{
			get
			{
				bool flag = this.isPlaying;
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
					zero = this.playerObject.Position;
				}
				return zero;
			}
			set
			{
				bool flag = this.playerObject == null;
				if (!flag)
				{
					this.playerObject.Position = value;
				}
			}
		}

		public string Source
		{
			get
			{
				string originalString;
				bool source;
				if (this.playerObject == null)
				{
					source = true;
				}
				else
				{
					source = !(this.playerObject.Source != null);
				}
				bool flag = source;
				if (flag)
				{
					originalString = null;
				}
				else
				{
					originalString = this.playerObject.Source.OriginalString;
				}
				return originalString;
			}
		}

		public double Volume
		{
			get
			{
				double volume;
				bool flag = this.playerObject == null;
				if (flag)
				{
					volume = 100;
				}
				else
				{
					volume = this.playerObject.Volume * 100;
				}
				return volume;
			}
			set
			{
				bool flag = this.playerObject == null;
				if (!flag)
				{
					this.playerObject.Volume = value;
				}
			}
		}

		public OldMediaPlayer()
		{
		}

		public void Dispose()
		{
			this.playerObject = null;
			this.positionTimer.Stop();
		}

		public void Init()
		{
			this.playerObject = new MediaPlayer();
			this.playerObject.MediaOpened += new EventHandler(this.PlayerObjectMediaOpened);
			this.playerObject.MediaFailed += new EventHandler<ExceptionEventArgs>(this.PlayerObjectMediaFailed);
			this.playerObject.MediaEnded += new EventHandler(this.PlayerObjectMediaEnded);
			DispatcherTimer dispatcherTimer = new DispatcherTimer();
			dispatcherTimer.Interval = TimeSpan.FromMilliseconds(500);
			this.positionTimer = dispatcherTimer;
			this.positionTimer.Tick += new EventHandler(this.PositionTimerTick);
		}

		public void Next()
		{
		}

		public void Pause()
		{
			bool flag = this.playerObject != null;
			if (flag)
			{
				this.isPlaying = false;
				this.positionTimer.Stop();
				this.playerObject.Pause();
			}
		}

		public void Play(string url)
		{
			bool flag = this.playerObject != null;
			if (flag)
			{
				this.playerObject.Open(new Uri(url));
				this.Play();
			}
		}

		public void Play()
		{
			bool flag = this.playerObject != null;
			if (flag)
			{
				this.isPlaying = true;
				this.positionTimer.Start();
				this.playerObject.Play();
			}
		}

		private void PlayerObjectMediaEnded(object sender, EventArgs e)
		{
			bool flag = this.MediaEnded == null;
			if (!flag)
			{
				this.MediaEnded();
			}
		}

		private void PlayerObjectMediaFailed(object sender, ExceptionEventArgs e)
		{
			bool flag = this.MediaError == null;
		}

		private void PlayerObjectMediaOpened(object sender, EventArgs e)
		{
			bool flag = this.PlayStateChanged == null;
			if (!flag)
			{
				this.PlayStateChanged();
			}
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
		}

		public void Stop()
		{
			bool flag = this.playerObject != null;
			if (flag)
			{
				this.isPlaying = false;
				this.positionTimer.Stop();
				this.playerObject.Stop();
			}
		}

		public event MediaChangedHandler MediaChanged;

		public event MediaEndedHandler MediaEnded;

		public event MediaErrorHandler MediaError;

		public event PlayStateChangedHandler PlayStateChanged;

		public event PositionChangedHandler PositionChanged;
	}
}