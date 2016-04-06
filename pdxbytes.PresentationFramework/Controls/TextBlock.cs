using System;
using Microsoft.SPOT;
using pdxbytes.ComponentModel;

namespace pdxbytes.PresentationFramework.Controls
{
    public class TextBlock : Control
    {
        public TextBlock() { }



        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBlock), new PropertyMetadata(""));


    }
}
