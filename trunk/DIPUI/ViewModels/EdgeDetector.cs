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
        private bool? _selected = false;

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

        protected EdgeDetector(string name)
        {
            Name = name;
        }

        public EdgeDetector(string name, Func<PgmImage, PgmImage> filterFunction) : this(name)
        {
            FilterFunction = filterFunction;
        }

        public virtual Func<PgmImage, PgmImage> FilterFunction { get; protected set; }
    }
}
