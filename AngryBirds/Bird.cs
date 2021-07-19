using AngryBirds.Properties;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;

namespace AngryBirds
{
    public class Bird : Sprite
    {
        protected System.Windows.Forms.Timer timer;
        protected static Random random = new Random();
        protected float vx;
        protected float vy;
        protected float V = 10;
        protected float g = 0.8f;
        protected float maxG;
        public event EventHandler OutOfBoard;
        public event EventHandler Miss;
        public Bird(Form form, int size, int left, int ground, List<Sprite> gameSprites) : base(form, Resources.bird, size, left, ground, gameSprites)
        {
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 20;
            timer.Tick += Timer_Tick;
            maxG = 4 * g;
        }

        public void MoveTo(float x, float y)
        {
            var length = Math.Sqrt((x - Left) * (x - Left) + (y - Top) * (y - Top));
            var cos = Math.Abs(x - Left) / length;
            var sin = Math.Abs(y - Top) / length;
            vx = V * (float)cos;
            vy = -V * (float)sin;
            if (y > bottomBorder) vy = -vy;
            if (x < Left) vx = -vx;
            StartMove();
        }

        public void SetSpeed(float speed)
        {
            V = speed;
        }

        protected void Go()
        {
            Left += (int)vx;
            Top += (int)vy;
            vy += g;
            if (this.IsOutOfBoard())
            {
                OutOfBoard?.Invoke(this, EventArgs.Empty);
                StopMove();
                timer.Dispose();
                this.Dispose();
            }
            if (this.HasMetGround())
            {
                vy = -vy;
                if (g >= maxG)
                {
                    StopMove();
                    Thread.Sleep(500);
                    Image = null;
                    timer.Dispose();
                    this.Dispose();
                    Miss?.Invoke(this, EventArgs.Empty);
                    return;
                }
                g = 2.5f * g;
            }
            foreach(var sprite in gameSprites)
            {
                if (this != sprite)
                {
                    if( Right > sprite.Left && Left < sprite.Right &&
                        Bottom > sprite.Top && Top < sprite.Bottom)
                    {
                        Explode();
                        sprite.Explode();
                    }
                }
            }
        }

        public override void Explode()
        {
            StopMove();
            timer.Dispose();
            base.Explode();
        }

        private void StartMove()
        {
            timer.Start();
        }

        private void StopMove()
        {
            timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Go();
        }

        private bool IsOutOfBoard()
        {
            return Left + Width < 0 || Left > form.ClientSize.Width || Top + Height < 0;
        }

        private bool HasMetGround()
        {
            return Bottom >= bottomBorder;
        }
    }
}
