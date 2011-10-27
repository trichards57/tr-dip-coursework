using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.Filters;

namespace DIPUI.ViewModels
{
    class EdgeDetector : ViewModelBase
    {
        private bool? _selected;

        public string Name { get; private set; }
        public bool? Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public EdgeDetector(string name)
        {
            Name = name;
            Selected = false;
        }

        private Type _filter;
        public Type Filter
        {
            get { return _filter; }
            set { _filter = value; }
        }
    }
}
