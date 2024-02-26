using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sprint5BeanTeam
{
    public class ScoreSystem
    {
        private HitboxSystem Hitboxes;
        private NoteManager Manager;
        private SpriteFont font;
        private Color[] colors;
        private float[] _alpha;
        private string _grade;
        private int _total;
        private int _multiplier;
        private int _bestCombo;
        public ScoreSystem(HitboxSystem hitbox, NoteManager manager, SpriteFont font)
        {
            Manager = manager;
            Hitboxes = hitbox;
            this.font = font;
            _grade = "-";
            colors = new Color[6];
            _alpha = new float[6];
            _total= 0;
            _multiplier = 1;
            _bestCombo = 0;
            for (int i = 0; i < 6; i++)
            {
                colors[i] = Hitboxes.getHitboxColor(i + 1);
                _alpha[i] = 0;
            }
        }
        public void setToFade(int lane)
        {
            _alpha[lane] = 1;
        }
        public void ResetSystem()
        {
            _total = 0;
            _bestCombo = 0;
            Hitboxes.notesHit = 0;
        }
        public void Update(GameTime gameTime)
        {
            if (Hitboxes.noteHit)
            {
                _total += (Hitboxes.currScore * _multiplier);
                setToFade(Hitboxes.recentNoteLane - 1);
                Hitboxes.noteHit = false;
            }
            for (int i = 0; i < colors.Length; i++)
            {
                if (_alpha[i] > 0)
                {
                    _alpha[i] -= 0.02f;
                }
            }
            if (Manager.totalSpawned != 0)
            {
                calculateAccuracy();
            }
        }
        public void Draw(SpriteBatch batch)
        {
            batch.Begin();
            batch.DrawString(font, "SCORE: " + _total, new Vector2(0, 45), Color.White);
            batch.DrawString(font, "HITS: " + Hitboxes.notesHit, new Vector2(0, 65), Color.White);
            batch.DrawString(font, Hitboxes.recentNoteStatus[0], new Vector2(62, 200), colors[0] * _alpha[0]);
            batch.DrawString(font, Hitboxes.recentNoteStatus[1], new Vector2(206, 200), colors[1] * _alpha[1]);
            batch.DrawString(font, Hitboxes.recentNoteStatus[2], new Vector2(330, 200), colors[2] * _alpha[2]);
            batch.DrawString(font, Hitboxes.recentNoteStatus[3], new Vector2(484, 200), colors[3] * _alpha[3]);
            batch.DrawString(font, Hitboxes.recentNoteStatus[4], new Vector2(628, 200), colors[4] * _alpha[4]);
            batch.DrawString(font, Hitboxes.recentNoteStatus[5], new Vector2(330, 200), colors[5] * _alpha[5]);
            batch.DrawString(font, "COMBO: " + Manager.combo, new Vector2(640, 120), colors[5]);
            batch.DrawString(font, calculateCombo(), new Vector2(650, 140), colors[5]);
            batch.End();
        }
        public string displayFinalScore()
        {
            string score = "FINAL SCORE: " + _total + "\n" + "TOTAL NOTES HIT: " + Hitboxes.notesHit + "\n" + "HIGHEST COMBO: " + _bestCombo + "\n" + "FINAL GRADE: " + _grade;
            return score;
        }
        private void calculateAccuracy()
        {
            float grade = ((float)_total / ((Manager.totalSpawned) * 2.1f * 250));
            if (grade >= 1)
            {
                _grade = "S+";
            }
            else if (grade > 0.9)
            {
                _grade = "S";
            }
            else if (grade > 0.8)
            {
                _grade = "A";
            }
            else if (grade > 0.7)
            {
                _grade = "B";
            }
            else if (grade > 0.6)
            {
                _grade = "C";
            }
            else
            {
                _grade = "D";
            }
        }
        private string calculateCombo()
        {
            string comboMultiplier = "1x";
            if (Manager.combo > 50)
            {
                comboMultiplier = "8x";
                _multiplier = 8;
            }
            else if (Manager.combo > 30)
            {
                comboMultiplier = "4x";
                _multiplier = 4;
            }
            else if (Manager.combo > 10)
            {
                comboMultiplier = "2x";
                _multiplier = 2;
            }
            else
            {
                _multiplier = 1;
            }
            if (Manager.combo > _bestCombo )
            {
                _bestCombo = Manager.combo;
            }
            return comboMultiplier;
        }
    }
}
