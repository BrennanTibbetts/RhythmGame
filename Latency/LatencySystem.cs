using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam.Latency
{
    public class LatencySystem
    {
        private Stopwatch stopwatch;
        private SoundEffectInstance testSound;

        private int audioLatency; // Audio Latency in ms
        private int videoLatency; // Video Latency in ms

        public LatencySystem(ContentManager content)
        {
            stopwatch = new Stopwatch();
            testSound = content.Load<SoundEffect>("TestBeep").CreateInstance();
            testSound.Volume = 0.0f;

            audioLatency = SetAudioLatency();
            videoLatency = 0;
        }

        private int SetAudioLatency()
        {
            stopwatch.Start();
            testSound.Play();
            stopwatch.Stop();

            return (int)stopwatch.ElapsedMilliseconds;
        }

        public void AdjustAudioLatency(int latency)
        {
            this.audioLatency = latency;
        }

        public void AdjustVideoLatency(int latency)
        {
            this.videoLatency = latency;

        }
    }
}
