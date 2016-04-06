using System;
using Microsoft.SPOT;
using pdxbytes.ComponentModel;

namespace pdxbytes.PresentationFramework.Controls
{
    public class DataContextControl : Control
    {
        public object DataContext
        {
            get { return (object)GetValue(DataContextProperty); }
            set { SetValue(DataContextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Context.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataContextProperty =
            DependencyProperty.Register("DataContext", typeof(object), typeof(DataContextControl), new PropertyMetadata(null));


    }
}
