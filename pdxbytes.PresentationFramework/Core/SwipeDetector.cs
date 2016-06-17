using System;
using Microsoft.SPOT;
using pdxbytes.Structures;
using System.Collections;
using System.Threading;

namespace pdxbytes.PresentationFramework.Core
{
    public class SwipeDetector
    {
        public SwipeDetector()
        {
            Timer = new Timer(Timer_Up, null, Timeout.Infinite, Timeout.Infinite);
        }

        const int SWIPE_MINLENGTH = 40;

        public Touch LastTouch { get; set; }
        private ArrayList touchlist = new ArrayList();
        private DateTime StartTime = DateTime.MinValue;
        private Timer Timer;

        public void OnTouchDetected(Touch touch)
        {
            if (touchlist.Count == 0)
                Timer.Change(500, Timeout.Infinite);

            touchlist.Add(touch);
        }

        private void Timer_Up(object state)
        {

            if (null != touchlist && touchlist.Count > 0)
            {
                foreach (Touch t in touchlist)
                {
                    Debug.Print(t.Touch1.X + "  " + t.Touch1.Y + "  " + t.Touched.Ticks);
                }
            }
            touchlist.Clear();
        }
    }
}
