using System;
using ManagedDigitalImageProcessing.PGM;

namespace DIPUI.ViewModels
{
    class Filter : ViewModelBase
    {
        private bool? _selected = false;
        private int _priority;
        private int _parameter;

        public string Name { get; private set; }
        public string ParameterName { get; private set; }
        public Func<PgmImage, PgmImage> FilterFunction { get; protected set; }

        protected Filter(string name, string parameterName)
        {
            Name = name;
            ParameterName = parameterName;
        }

        public Filter(string name, string parameterName, Func<PgmImage, PgmImage> filterFunction)
            : this(name, parameterName)
        {
            FilterFunction = filterFunction;
        }

        public int Priority
        {
            get { return _priority; }
            set
            {
                _priority = value;
                OnPropertyChanged("Priority");
            }
        }

        public bool? Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public int Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged("Parameter");
            }
        }
    }
}
