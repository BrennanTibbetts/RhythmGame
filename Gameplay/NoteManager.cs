using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using MaiLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Sprint5BeanTeam;
using static MaiLib.Chart;

namespace Sprint5BeanTeam
{
    public class NoteManager
    {
        private Chart incomingChart;
        public List<INoteObject> NoteOnTime;
        private Game1 Game;
        private double _time;
        private double fadeInTime = 0;
        private const double fadeOutTime = 3;
        private const int criticalLinePos = 440;
        private int maxChartDefinition;
        private double initBPM;
        private bool songStarted;
        private double endTimer;
        private double fps;

        public bool DemonstrateAuto { get; set; }

        private double lastNoteTiming;

        private int totalNotes;
        private int notesPassed;
        public int totalSpawned;
        public int combo;
        public double ProcessOfSong { get { return (double)this.notesPassed / this.totalNotes; } }
        private NotifyObserver observer;
        private HitboxSystem hitbox;

        /// <summary>
        /// Visual offset in ms, act by adjusting note spawn time
        /// </summary>
        public double VisualOffset { get; private set; }

        /// <summary>
        /// Visual offset in ms, act by adjusting fade-in time
        /// </summary>
        public double AudioOffset { get; private set; }

        // public event EventHandler PlayGuideSound;

        // protected virtual void OnPlayGuideSound(EventArgs e)
        // {
        //     PlayGuideSound?.Invoke(this, e);
        // }

        public NoteManager(Chart incomingChart, List<Texture2D> textureList, HitboxSystem hitbox, Game1 game)
        {
            this.VisualOffset = 0;
            this.AudioOffset = 0;
            this.observer = new NotifyObserver(game.SE.Update);
            this.incomingChart = incomingChart;
            this.maxChartDefinition = incomingChart.Definition;
            this.initBPM = incomingChart.BPMChanges.ChangeNotes[0].BPM;
            this.hitbox = hitbox;
            this.endTimer = 0;
            this.NoteOnTime = new();
            this.Game = game;
            this.fps = Game.FrameRate;
            foreach (Note note in incomingChart.Notes)
            {
                if (note is Tap)
                {
                    if (!note.Key.Equals("5")) this.NoteOnTime.Add(new SingleNote(note.TickTimeStamp, textureList[0], int.Parse(note.Key) + 1, (int)note.BPM / (10*(int)fps/60), 0.4f));
                    else this.NoteOnTime.Add(new BarNote(note.TickTimeStamp, textureList[2], (int)note.BPM / (10 * (int)fps / 60), 0.6f));
                    if (fadeInTime == 0) fadeInTime = (double)criticalLinePos / (note.BPM / (10 * (int)fps / 60)) * (1d/fps) + AudioOffset;
                    // Console.WriteLine("{0}\t{1}", note.Compose(1), note.TickTimeStamp);
                }
                else if (note is Hold)
                {
                    double startTiming = note.TickTimeStamp;
                    if (note.CalculatedLastTime == 0) { throw new InvalidOperationException("This note does not have calculated last time"); }
                    const int holdPeriod = 12; // 12 is 1/32
                    List<HoldNote> holdCandidate = new();
                    for (int holdTiming = note.TickStamp; holdTiming <= note.TickStamp + note.LastLength; holdTiming += holdPeriod)
                    {
                        Hold localNote = new Hold(note);
                        localNote.Bar = holdTiming / 384;
                        localNote.Tick = holdTiming % 384;

                        localNote.WaitLength = 0;
                        localNote.TickTimeStamp = this.incomingChart.GetTimeStamp(localNote.TickStamp);
                        localNote.Update();

                        HoldNote candidate = new HoldNote(localNote.TickTimeStamp, textureList[1], int.Parse(localNote.Key) + 1, (int)localNote.BPM / (10 * (int)fps / 60), 0.4f);
                        candidate.Guided = true;
                        holdCandidate.Add(candidate);
                        // if (localNote.Tick == note.Tick) { throw new InvalidOperationException("This created not did not update timing"); }
                        holdTiming += holdPeriod;
                    }
                    holdCandidate.First().Guided = false;
                    // holdCandidate.Last().Guided = false;
                    this.NoteOnTime.AddRange(holdCandidate);
                }
            }
            this.lastNoteTiming = this.NoteOnTime.Last().Timing;
            this.totalSpawned = this.NoteOnTime.Count();
            this.totalNotes = this.NoteOnTime.Count();
        }


        public void DrawNotes(SpriteBatch batch)
        {
            foreach (INoteObject note in NoteOnTime)
            {
                if (!note.checkForDestroy()) note.Draw(batch);
            }
        }

        public void HandleInput(int lane)
        {
            double timing = this._time - (fadeInTime + AudioOffset);
            // Console.WriteLine("Lane input handle triggered in lane {0} of timing {1} with first note of {2}", lane, timing, NoteOnTime.First().Timing);
            // bool singleReached = false;
            // INoteObject candidate = null;
            foreach (INoteObject note in this.NoteOnTime)
            {
                if (!note.checkForDestroy() && note is not HoldNote && note.Lane == lane && Math.Abs(note.Timing - timing) <= 0.12)
                {
                    hitbox.playParticleEffect(note.Lane);
                    hitbox.notesHit++;
                    this.combo++;
                    hitbox.registerScore((float)(100 * ((0.12 - Math.Abs(note.Timing - timing)) / 0.12)), note, Math.Round((note.Timing - timing) * 1000, 2));
                    break;
                }
            }
            // if (candidate is not null) this.NoteOnTime.Remove(candidate);
        }

        public void Update(GameTime gameTime)
        {
            double timing = this._time - (fadeInTime + AudioOffset);
            // double timing = this._time - fadeInTime;
            // if (this.guideTiming.Count > 0 && Math.Abs(this.guideTiming.First() - timing) <= 0.01)
            // {
            //     this.observer.Invoke("guide");
            //     Console.WriteLine("A note arrived in critical line at time {0}", _time-fadeInTime);
            // }

            foreach (INoteObject note in NoteOnTime)
            {
                note.Update(gameTime, _time);

                //If a note passed critical line, an answer song should play
                if (Math.Abs(note.Timing - timing) < 0.01 && !note.Guided)
                {
                    this.observer.Invoke("guide");
                    note.Guided = true;
                    this.notesPassed++;

                    if (DemonstrateAuto)
                    {
                        hitbox.playParticleEffect(note.Lane);
                        note.destroy();
                        hitbox.notesHit++;
                        this.combo++;
                        this.notesPassed++;
                        hitbox.registerScore(100f, note, 0);
                    }

                    // Console.WriteLine("A note arrived in critical line at time {0} on lane {1}",_time,note.Lane);
                }
                if (!note.checkForDestroy() && note is HoldNote && hitbox.canActivate[note.Lane - 1] && note.Position.Y > 440)
                {
                    hitbox.playParticleEffect(note.Lane);
                    note.destroy();
                    hitbox.notesHit++;
                    this.combo++;
                    this.notesPassed++;
                    hitbox.registerScore(10f, note, 0);
                }
                else if (!note.checkForDestroy() && note.Guided && timing - note.Timing >= 0.3)
                {
                    note.destroy();
                    this.combo = 0;
                    this.notesPassed++;
                }
            }

            if (!songStarted && Math.Abs((fadeInTime + AudioOffset) - _time) <= 0.01)
            {
                this.observer.Invoke("play");
                // Console.WriteLine("Song played at time {0} with fade in time {1}",_time,fadeInTime);
                songStarted = true;
            }
            if (songStarted && (_time + AudioOffset) > lastNoteTiming)
            {
                this.endTimer += gameTime.ElapsedGameTime.TotalSeconds;
            }
            //endTimer > fadeOutTime
            // Determine game end here
            if (endTimer > fadeOutTime)
            {
                this.Game.CurrentGameState = IState.GameState.GameOver;
            }
            // Console.WriteLine("The process is {0} with total note {1} and hit note {2}",ProcessOfSong,totalNotes,notesPassed);
            _time += gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void AddOffset(double offset)
        {
            this.AudioOffset += offset;
        }

        public void MinusOffset(double offset)
        {
            this.AudioOffset -= offset;
        }
    }
}
