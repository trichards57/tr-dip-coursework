using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ManagedDigitalImageProcessing.Filters;
using ManagedDigitalImageProcessing.PGM;

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

        public EdgeDetector(string name, Func<PgmImage, PgmImage> filterFunction)
        {
            Name = name;
            Selected = false;

            FilterFunction = filterFunction;
        }

        public Func<PgmImage, PgmImage> FilterFunction { get; private set; }
    }
}
