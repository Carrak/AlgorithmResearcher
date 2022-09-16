using System;
using System.Drawing;
using System.Windows.Forms;

namespace Kursach
{
    internal class SwapAnimation
    {
        private const int TOTAL_TICKS = 7;
        private readonly Timer _timer = new Timer() { Interval = 1 };

        private readonly Control _control1;
        private readonly Control _control2;
        private readonly int _x1;
        private readonly int _x2;

        private readonly int _dx1;
        private readonly int _dx2;

        private int Ticks { get; set; } = 0;

        public delegate void AnimationFinishedHandler(SwapAnimation sa);
        public event AnimationFinishedHandler AnimationFinished;

        public SwapAnimation(Control control1, Control control2)
        {
            _control1 = control1;
            _control2 = control2;

            _dx1 = (control2.Location.X - control1.Location.X) / TOTAL_TICKS;
            _dx2 = (control1.Location.X - control2.Location.X) / TOTAL_TICKS;

            _x1 = control1.Location.X;
            _x2 = control2.Location.X;

            _timer.Tick += Tick;
        }

        private void Tick(object sender, EventArgs e)
        {
            if (Ticks == TOTAL_TICKS)
            {
                FinalizeAnimation();
                return;
            }

            _control1.Location = new Point(_control1.Location.X + _dx1, _control1.Location.Y);
            _control2.Location = new Point(_control2.Location.X + _dx2, _control2.Location.Y);
            Ticks++;
        }

        public void Start()
        {
            _timer.Start();
        }

        public void FinalizeAnimation()
        {
            _timer.Tick -= Tick;
            _timer.Stop();
            _timer.Dispose();

            _control1.Location = new Point(_x1 + TOTAL_TICKS * _dx1, _control1.Location.Y);
            _control2.Location = new Point(_x2 + TOTAL_TICKS * _dx2, _control2.Location.Y);

            string temp = _control1.Name;
            _control1.Name = _control2.Name;
            _control2.Name = temp;

            AnimationFinished?.Invoke(this);
        }

    }
}
