using System;
using Microsoft.SPOT;

namespace pdxbytes.ComponentModel
{
    public class PropertyMetadata
    {
        public PropertyMetadata(object defaultValue)
        {
            this.DefaultValue = defaultValue;
        }
        public object DefaultValue { get; set; }
    }
}
