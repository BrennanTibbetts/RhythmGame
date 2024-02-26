using System;
using System.Net.Mime;
using System.Threading;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using static Sprint5BeanTeam.IState;

namespace Sprint5BeanTeam
{
    public class SoundEffectManager : ISoundEffectManager
    {
        public enum MusicStateEnum { Ready, Playing, Paused };
        private MusicStateEnum musicState;
        private Song bbc_001;
        private Song lrs_002;
        private Song cld_003;
        private Song ukt_004;
        private Song gtk_005;
        private SoundEffect guideSound;
        private SoundEffect cancelSound;
        private SoundEffect confirmSound;
        private SoundEffect damageSound;
        private SoundEffect entrySound;
        private SoundEffect pauseSound;
        private SoundEffect tapSound;
        private SoundEffect startSound;
        private Song currentSong;
        protected bool Muted;

        public SoundEffectManager(ContentManager content)
        {
            this.bbc_001 = content.Load<Song>("Song/001_buckeye_battle_cry");
            this.lrs_002 = content.Load<Song>("Song/002_le_regiment_short_mixed");
            this.cld_003 = content.Load<Song>("Song/003_cloudless");
            this.ukt_004 = content.Load<Song>("Song/004_umiyuri_kaiteitan");
            this.gtk_005 = content.Load<Song>("Song/005_goodtek");

            this.currentSong = this.bbc_001;

            this.guideSound = content.Load<SoundEffect>("SoundEffect/answer");
            this.cancelSound = content.Load<SoundEffect>("SoundEffect/Cancel");
            this.confirmSound = content.Load<SoundEffect>("SoundEffect/Confirm");
            this.damageSound = content.Load<SoundEffect>("SoundEffect/Damage");
            this.entrySound = content.Load<SoundEffect>("SoundEffect/Entry");
            this.pauseSound = content.Load<SoundEffect>("SoundEffect/Pause");
            this.tapSound = content.Load<SoundEffect>("SoundEffect/tap_perfect");
            this.startSound = content.Load<SoundEffect>("SoundEffect/track_start");
            this.Muted = false;
        }

        public void FastReset()
        {
            this.StopMusic();
        }

        public void MuteOrReverse()
        {
            this.Muted = !this.Muted;
            MediaPlayer.IsMuted = this.Muted;
        }

        public void Pause()
        {
            if (!Muted) this.pauseSound.Play();
            MediaPlayer.Pause();
            this.musicState = MusicStateEnum.Paused;
        }

        public void PlayMusic()
        {
            if (this.musicState == MusicStateEnum.Ready && this.currentSong is not null)
                MediaPlayer.Play(this.currentSong);
            else MediaPlayer.Resume();
        }

        public void StopMusic()
        {
            MediaPlayer.Stop();
            this.musicState = MusicStateEnum.Ready;
        }

        public void Update(string state)
        {
            switch (state)
            {
                case "play":
                    this.PlayMusic();
                    break;
                case "pause":
                    this.Pause();
                    break;
                case "stop":
                    this.StopMusic();
                    break;
                case "mute":
                    this.MuteOrReverse();
                    break;
                case "reset":
                    this.FastReset();
                    break;
                case "select_lrs":
                    this.currentSong = this.lrs_002;
                    break;
                case "select_cld":
                    this.currentSong = this.cld_003;
                    break;
                case "select_ukt":
                    this.currentSong = this.ukt_004;
                    break;
                case "select_bbc":
                    this.currentSong = this.bbc_001;
                    break;
                case "confirm":
                    if (!Muted) this.confirmSound.CreateInstance().Play();
                    break;
                case "cancel":
                    if (!Muted) this.cancelSound.CreateInstance().Play();
                    break;
                case "tap":
                    if (!Muted) this.tapSound.CreateInstance().Play();
                    break;
                case "guide":
                    if (!Muted) this.guideSound.CreateInstance().Play();
                    // // Console.WriteLine("Guide Should Play");
                    break;
                default:
                    throw new NullReferenceException("INVALID NOTIFICATION: " + state);
            }
        }
    }
}