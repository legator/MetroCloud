using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicCloudPlayer.Player
{
    public interface IStreamMediaPlayer : IDisposable
    {
        TimeSpan Duration
        {
            get;
        }

        bool IsPlaying
        {
            get;
        }

        TimeSpan Position
        {
            get;
            set;
        }

        string Source
        {
            get;
        }

        double Volume
        {
            get;
            set;
        }

        void Init();

        void Next();

        void Pause();

        void Play(string url);

        void Play();

        void Prev();

        void Stop();

        event MediaChangedHandler MediaChanged;

        event MediaEndedHandler MediaEnded;

        event MediaErrorHandler MediaError;

        event PlayStateChangedHandler PlayStateChanged;

        event PositionChangedHandler PositionChanged;
    }
}
