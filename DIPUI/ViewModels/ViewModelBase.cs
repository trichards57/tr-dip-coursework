using System.ComponentModel;
using System.Diagnostics;

namespace DIPUI.ViewModels
{
    abstract class ViewModelBase : INotifyPropertyChanged
    {
        public bool ThrowOnInvalidPropertyName { get; set; }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        private void VerifyPropertyName(string property)
        {
            if (TypeDescriptor.GetProperties(this)[property] != null) return;
            
            var message = string.Format("Invalid property name : {0}", property);
            if (ThrowOnInvalidPropertyName)
                throw new InvalidPropertyException(message);
            
            Debug.Fail(message);
        }

        protected void OnPropertyChanged(string property)
        {
            VerifyPropertyName(property);

            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
