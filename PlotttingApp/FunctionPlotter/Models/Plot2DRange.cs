using System;
using System.ComponentModel;

namespace FunctionPlotter.Models
{
    public class Plot2DRange : INotifyPropertyChanged
    {
        private double xMin, xMax, yMin, yMax;
        
        public Plot2DRange(double xMin, double xMax, double yMin, double yMax)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;
        }

        public double XMin
        {
            get { return this.xMin; }
            set
            {
                if (xMin != value)
                {
                    xMin = value;
                    this.OnPropertyChanged("XMin");
                }
            }
        }
 
        public double XMax
        {
            get
            {
                return this.xMax;
            }
            set
            {
                if (this.xMax != value)
                {
                    this.xMax = value;
                    this.OnPropertyChanged("XMax");
                }
            }
        } 

        public double YMin
        {
            get
            {
                return this.yMin;
            }
            set
            {
                if (this.yMin != value)
                {
                    this.yMin = value;
                    this.OnPropertyChanged("YMin");
                }
            }
        }

        public double YMax
        {
            get
            {
                return this.yMax;
            }
            set
            {
                if (this.yMax != value)
                {
                    this.yMax = value;
                    this.OnPropertyChanged("YMax");
                }
            }
        }

        #region INotifyPropertyChanged
        
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
