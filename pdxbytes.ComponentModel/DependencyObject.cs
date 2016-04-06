using System;
using Microsoft.SPOT;

namespace pdxbytes.ComponentModel
{
    public class DependencyObject
    {
        public DependencyObject() { }

        public object GetValue(DependencyProperty property)
        {
            return ObjectEntryPortal.Get(property.Id);
        }

        public void SetValue(DependencyProperty dp, object value)
        {
            ObjectEntryPortal.Set(dp.Id, value);
        }

    }
}
