using Microsoft.SPOT;

namespace pdxbytes.ComponentModel
{
    //
    // Summary:
    //     Provides data for the System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    //     event.
    public class PropertyChangedEventArgs : EventArgs
    {
        //
        // Summary:
        //     Initializes a new instance of the System.ComponentModel.PropertyChangedEventArgs
        //     class.
        //
        // Parameters:
        //   propertyName:
        //     The name of the property that changed.
        public PropertyChangedEventArgs(string propertyName)
        {
            this._propertyName = propertyName;
        }

        //
        // Summary:
        //     Gets the name of the property that changed.
        //
        // Returns:
        //     The name of the property that changed.
        public virtual string PropertyName { get; }
        private string _propertyName;
    }
}