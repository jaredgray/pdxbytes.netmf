using System;
using Microsoft.SPOT;

namespace pdxbytes.ComponentModel
{
    public class DependencyProperty
    {
        private DependencyProperty()
        {

        }
        public static DependencyProperty Register(string propertyName, Type valueType, Type ownerType, PropertyMetadata meta)
        {
            var property = new DependencyProperty()
            {
                PropertyName = propertyName,
                Owner = ownerType,
                Meta = meta,
                ValueType = valueType
            };
            //DependencyPropertyRegistry[propertyName + ownerType.FullName] = property;
            var id = ObjectEntryPortal.Add(meta.DefaultValue);
            property.Id = id;
            return property;
        }
        //private static System.Collections.Hashtable DependencyPropertyRegistry = new System.Collections.Hashtable();

        public int Id { get; set; }

        public string PropertyName { get; set; }
        public PropertyMetadata Meta { get; set; }
        public Type Owner { get; set; }
        public Type ValueType { get; set; }

    }
}
