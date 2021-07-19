using AngryBirds.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace AngryBirds
{
    public abstract class Sprite : PictureBox
    {
        protected Form form;
        protected int bottomBorder;
        protected List<Sprite> gameSprites;
        private Timer explodeTimer;

        public event EventHandler OnExplode;
        public Sprite(Form form, Image image, int size, int left, int bottomBorder, List<Sprite> gameSprites)
        {
            this.form = form;
            this.bottomBorder = bottomBorder;
            Width = size;
            Height = size;
            Left = left;
            Top = this.bottomBorder - size;
            Image = image;
            SizeMode = PictureBoxSizeMode.StretchImage;
            this.gameSprites = gameSprites;
            BackColor = Color.Transparent;

            var graphicsPath = BuildTransparencyPath();
            Region = new Region(graphicsPath);
            form.Controls.Add(this);

            explodeTimer = new Timer();
            explodeTimer.Interval = 500;
            explodeTimer.Tick += ExplodeTimer_Tick;
        }

        public virtual void Explode()
        {
            Image = Resources.explode;
            Region = new Region(BuildTransparencyPath());
            explodeTimer.Start();
        }

        private GraphicsPath BuildTransparencyPath()
        {
            var bitmap = new Bitmap(Image, Width, Height);
            var graphicsPath = new GraphicsPath();
            var mask = bitmap.GetPixel(0, 0);

            for (int x = 0; x <= bitmap.Width - 1; x++)
            {
                for (int y = 0; y <= bitmap.Height - 1; y++)
                {
                    if (!bitmap.GetPixel(x, y).Equals(mask))
                    {
                        graphicsPath.AddRectangle(new Rectangle(x, y, 1, 1));
                    }
                }
            }
            bitmap.Dispose();
            return graphicsPath;
        }

        private void ExplodeTimer_Tick(object sender, EventArgs e)
        {
            explodeTimer.Stop();
            explodeTimer.Dispose();
            OnExplode?.Invoke(this, EventArgs.Empty);
            Dispose();
        }
    }
}
