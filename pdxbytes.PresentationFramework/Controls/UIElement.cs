using System;
using Microsoft.SPOT;
using pdxbytes.PresentationFramework.Controls;
using pdxbytes.Graphics.Shapes;
using pdxbytes.Structures;
using pdxbytes.ComponentModel;

namespace pdxbytes.PresentationFramework.Controls
{
    public class UIElement : DependencyObject
    {
        public UIElement()
        {
            _controls = new ControlCollection(this);
            this.Surface = new Rectangle(0, 0, 0, 0);
            this.IsVisible = true;
            this.IsInvalid = true;
        }

        public bool IsVisible
        {
            get { return _IsVisible; }
            set
            {
                if (value != _IsVisible)
                {
                    _IsVisible = value;
                    if (value)
                        this.Show();
                    else
                        this.Hide();
                }
            }
        }
        private bool _IsVisible;

        public ControlCollection Controls { get { return _controls; } }
        private ControlCollection _controls;


        public void Show()
        {
            this.IsVisible = true;
            this.OnShow();
        }
        public void Hide()
        {
            this.IsVisible = false;
            this.OnHide();
        }

        protected void OnShow()
        {
            this.Invalidate();
        }
        private void OnHide()
        {
            this.Invalidate();
        }

        

        private void Invalidate()
        {
            this.IsInvalid = true;
            if (null != this.Parent)
                this.Parent.Invalidate();
        }

        internal bool IsInvalid { get; set; }

        /// <summary>
        /// sets the position of this control, realatively all child controls will be positioned within this cont
        /// rol
        /// </summary>
        public Vec216 Position
        {
            get { return _Position; }
            set
            {
                if (value != _Position)
                {
                    _Position = value;
                    this.Invalidate();
                }
            }
        }
        private Vec216 _Position;

        public Shape Surface { get; set; }

        public int HitAreaOffset { get; set; }

        public virtual short X
        {
            get { return this.Surface.X; }
            set
            {
                if (value != this.Surface.X)
                {
                    this.Surface.X = value;
                }
            }
        }

        public virtual short Y
        {
            get { return this.Surface.Y; }
            set
            {
                if (value != this.Surface.Y)
                {
                    this.Surface.Y = value;
                }
            }
        }
        public virtual short Width
        {
            get { return this.Surface.Width; }
            set
            {
                if (value != this.Surface.Width)
                {
                    this.Surface.Width = value;
                }
            }
        }

        public virtual short Height
        {
            get { return this.Surface.Height; }
            set
            {
                if (value != this.Surface.Height)
                {
                    this.Surface.Height = value;
                }
            }
        }

        public Color BackgroundColor { get { return this.Surface.BackgroundColor; } set { this.Surface.BackgroundColor = value; } }

        public int Zindex { get { return this.Surface.Zindex; } set { this.Surface.Zindex = value; } }

        public UIElement Parent { get; set; }

        internal virtual void HandleTap(Touch touch)
        {
            //Debug.Print(this.GetType().Name + ".HandleTap x: " + touch.Touch1.X + ", y: " + touch.Touch1.Y);
        }
        internal virtual void HandleDoubleTap(Touch touch)
        {

        }
    }
}
