using System;
using Microsoft.SPOT;
using pdxbytes.DeviceInterfaces;

namespace pdxbytes.PresentationFramework.Core
{
    public class TouchManager
    {
        public TouchManager(ITouchInterface iTouch, IDisplay display)
        {
            this.Display = display;
            this.ITouch = iTouch;
            this.InitializeTouchDelegate();
            this.ITouch.Touched += ITouch_Touched;
        }

        private void ITouch_Touched(Structures.Touch touch)
        {
            var handler = Touched;
            if (null != handler)
            {
                var actualTouch = ConvertTouch(touch);
                handler(actualTouch);
            }
        }

        private ITouchInterface ITouch { get; set; }
        private IDisplay Display { get; set; }

        void InitializeTouchDelegate()
        {
            /*
                Currently not supported:
                    Display.Orientation - the coordinate system should be evaluated based on the current orientation. for now it's fixed since the need for this capability is not important
            */

            if (ITouch.CoordinateSystem == CoordinateSystems.TopLeft)
            {
                ConvertTouch = (t) => t;
            }
            else if (ITouch.CoordinateSystem == CoordinateSystems.TopRight)
            {
                ConvertTouch = (t) =>
                {
                    return new Structures.Touch()
                    {
                        Touch1 = new Structures.Vec2()
                        {
                            X = Display.Width - t.Touch1.X,
                            Y = t.Touch1.Y
                        },
                        Touch2 = new Structures.Vec2()
                        {
                            X = Display.Width - t.Touch2.X,
                            Y = t.Touch2.Y
                        }
                    };
                };
            }
            else if (ITouch.CoordinateSystem == CoordinateSystems.BottomLeft)
            {
                ConvertTouch = (t) =>
                {
                    return new Structures.Touch()
                    {
                        Touch1 = new Structures.Vec2()
                        {
                            X = t.Touch1.X,
                            Y = Display.Height - t.Touch1.Y
                        },
                        Touch2 = new Structures.Vec2()
                        {
                            X = t.Touch2.X,
                            Y = Display.Height - t.Touch2.Y
                        }
                    };
                };
            }
            else
            {
                ConvertTouch = (t) =>
                {
                    return new Structures.Touch()
                    {
                        Touch1 = new Structures.Vec2()
                        {
                            X = Display.Width - t.Touch1.X,
                            Y = Display.Height - t.Touch1.Y
                        },
                        Touch2 = new Structures.Vec2()
                        {
                            X = Display.Width - t.Touch2.X,
                            Y = Display.Height - t.Touch2.Y
                        }
                    };
                };
            }
        }
        private ConvertTouchDelegate ConvertTouch;
        delegate Structures.Touch ConvertTouchDelegate(Structures.Touch source);


        public event TouchDelegate Touched;
    }
}
