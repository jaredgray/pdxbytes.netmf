using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces;
using pdxbytes.PresentationFramework.Controls;
using pdxbytes.Structures;
using VikingErik.NetMF.MicroLinq;
using static VikingErik.NetMF.MicroLinq.PrebuiltDelegates.PrebuiltDelegates;
using pdxbytes.Graphics;

namespace pdxbytes.PresentationFramework.Core
{
    internal class UIController
    {
        public UIController()
        {
        }

        public void SetMainView(View view)
        {
            this.MainView = view;
        }

        public IDisplay Display { get; set; }
        public ITouchInterface TouchInterface { get; set; }
        private TouchManager TouchManager { get; set; }
        public View MainView { get; set; }

        public void Startup()
        {
            if (null != TouchInterface)
            {
                TouchManager = new TouchManager(TouchInterface, Display);
                TouchManager.Touched += TouchInterface_Touched;
            }
            this.Render();
        }

        private void Render()
        {
            var controls = this.MainView.AllControls.Where((x) => x.IsInvalid && x.IsVisible).OrderBy(x => ((Control)x).Zindex, Comparers.IntCompare);
            Pipeline pipeline = new Pipeline(Display);
            pipeline.AddDisplay(this.MainView.Surface);
            foreach (Control c in controls)
            {
                pipeline.AddDisplay(c.Surface);
            }
            pipeline.Flush();
        }

        private void TouchInterface_Touched(Structures.Touch touch)
        {
            UIElement target = null;

            var candidates = this.MainView.AllControls.OrderBy(x => ((UIElement)x).Zindex, Comparers.IntCompare).ToList();
            candidates.Insert(0, this.MainView);
            for (int i = candidates.Count - 1; i > -1; --i)
            {
                var ctrl = (UIElement)candidates[i];
                var touched = IsTouched(touch, ctrl);
                if (touched)
                {
                    target = ctrl;
                    break;
                }
            }
            if (null != target)
            {
                target.HandleTap(touch);
            }
        }

        private static bool IsTouched(Touch touch, UIElement target)
        {
            //Debug.Print("UIController.IsTouched() touch: x - " + touch.Touch1.X + ", y - " + touch.Touch1.Y);
            //Debug.Print("Control " + target.GetType().ToString() + ". X: " + target.X + ", Y: " + target.Y + ", Width: " + target.Width + ", Height: " + target.Height);
            return ((touch.Touch1.X > (target.X - target.HitAreaOffset) && touch.Touch1.X < target.X + target.Width + target.HitAreaOffset) && (touch.Touch1.Y > target.Y - target.HitAreaOffset && touch.Touch1.Y < target.Y + target.Height + target.HitAreaOffset)); // || ((touch.Touch2.X > target.X && touch.Touch2.X < target.X + target.Width) && (touch.Touch2.Y > target.Y && touch.Touch2.Y < target.Y + target.Height));
        }
    }
}
