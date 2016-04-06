using System;
using Microsoft.SPOT;

namespace pdxbytes.ComponentModel
{
    //
    // Summary:
    //     Notifies clients that a property value has changed.
    public interface INotifyPropertyChanged
    {
        //
        // Summary:
        //     Occurs when a property value changes.
        event PropertyChangedEventHandler PropertyChanged;
    }
}
