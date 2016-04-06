using System;
using Microsoft.SPOT;
using System.Collections;

namespace pdxbytes.PresentationFramework.Controls
{
    public static class ControlExtensions
    {
        public static ArrayList Where(this IEnumerable subject, WhereControntrolDelegate func)
        {
            ArrayList list = new ArrayList();
            foreach(var c in subject)
            {
                if (func((Control)c))
                    list.Add(c);
            }
            return list;
        }
        

    }

    public delegate bool WhereControntrolDelegate(Control candidate);
    public delegate int OrderControntrolDelegate(Control candidate);
}
