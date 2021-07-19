using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AngryBirds
{
    public partial class AngryBirdsForm : Form
    {
        private int ground = 430;
        private Pig pig;
        private Bird bird;
        private int pigSize;
        private int pigPosition;
        private List<Sprite> spriteList;
        private Random random = new Random();
        private int score = 0;
        private Timer speedEncreaseTimer;
        public AngryBirdsForm()
        {
            InitializeComponent();
            spriteList = new List<Sprite>();
            MouseUp += AngryBirdsForm_MouseUp;
            MouseDown += AngryBirdsForm_MouseDown;
            GetNewBird();
            GetNewPig();
            speedEncreaseTimer = new Timer();
            speedEncreaseTimer.Interval = 300;
            speedEncreaseTimer.Tick += SpeedEncreaseTimer_Tick;
        }

        private void SpeedEncreaseTimer_Tick(object sender, EventArgs e)
        {
            if (speedValueBar.Value >= speedValueBar.Maximum)
            {
                speedEncreaseTimer.Stop();
                return;
            }
            speedValueBar.PerformStep();
        }

        private void Pig_OnExplode(object sender, EventArgs e)
        {
            GetNewPig();
        }

        private void Bird_Miss(object sender, EventArgs e)
        {
            GetNewBird();
        }

        private void Bird_OnExplode(object sender, EventArgs e)
        {
            score++;
            scoreLabel.Text = score.ToString();
            GetNewBird();
        }

        private void GetNewPig()
        {
            spriteList.Remove(pig);
            speedValueBar.Value = speedValueBar.Minimum;
            pigSize = random.Next(50, 100);
            pigPosition = random.Next(ClientSize.Width / 3, ClientSize.Width - pigSize);
            pig = new Pig(this, pigSize, pigPosition, ground, spriteList);
            spriteList.Add(pig);
            pig.OnExplode += Pig_OnExplode;
        }

        private void GetNewBird()
        {
            spriteList.Remove(bird);
            speedValueBar.Value = speedValueBar.Minimum;
            bird = new Bird(this, 80, 50, ground, spriteList);
            spriteList.Add(bird);
            bird.OutOfBoard += Bird_Miss;
            bird.OnExplode += Bird_OnExplode;
            bird.Miss += Bird_Miss;
            MouseDown += AngryBirdsForm_MouseDown;
            MouseUp += AngryBirdsForm_MouseUp;
        }

        private void AngryBirdsForm_MouseDown(object sender, MouseEventArgs e)
        {
            speedEncreaseTimer.Start();
        }

        private void AngryBirdsForm_MouseUp(object sender, MouseEventArgs e)
        {
            speedEncreaseTimer.Stop();
            bird.SetSpeed(speedValueBar.Value / 10);
            bird.MoveTo(e.X, e.Y);
            MouseDown -= AngryBirdsForm_MouseDown;
            MouseUp -= AngryBirdsForm_MouseUp;  
        }
    }
}
