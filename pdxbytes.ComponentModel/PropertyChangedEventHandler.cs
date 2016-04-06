using System;
using Microsoft.SPOT;

namespace pdxbytes.ComponentModel
{
    //
    // Summary:
    //     Represents the method that will handle the System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    //     event raised when a property is changed on a component.
    //
    // Parameters:
    //   sender:
    //     The source of the event.
    //
    //   e:
    //     A System.ComponentModel.PropertyChangedEventArgs that contains the event data.
    public delegate void PropertyChangedEventHandler(object sender, PropertyChangedEventArgs e);
}
