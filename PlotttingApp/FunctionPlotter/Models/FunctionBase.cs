using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using Telerik.Windows.Controls;

namespace FunctionPlotter.Models
{
    public abstract class FunctionBase : INotifyPropertyChanged
    {
        #region Private Members

        protected string functionString;

        protected bool isValid;

        #endregion

        #region Public Properties

        /// <summary>
        /// The string representation (source) of the function.
        /// Validated on every update.
        /// </summary>
        public string FunctionString
        {
            get
            {
                return this.functionString;
            }
            set
            {
                if (this.functionString == value)
                    return;

                this.functionString = value;

                this.Validate();

                this.OnPropertyChanged("FunctionString");
            }
        }

        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
            protected set
            {
                if (this.isValid == value)
                    return;

                this.isValid = value;

                this.OnPropertyChanged("IsValid");
            }
        }

        //Last Valid
        //public Func<double, double> F { get; protected set; }

        #endregion

        #region Constructor

        protected FunctionBase()
        {
            this.functionString = string.Empty;
            this.isValid = false;
        }

        #endregion

        protected abstract void Validate();

        internal abstract void UpdatePlotData(Telerik.Windows.Data.RadObservableCollection<PlotInfo> dataCollection,
            Plot2DRange plotRange, uint samplesCount);

        public abstract FunctionBase Clone();

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }

    public class RealFunction : FunctionBase
    {
        public Func<double, double> F { get; protected set; }

        protected override void Validate()
        {
            try
            {
                var paramExpressions = new System.Linq.Expressions.ParameterExpression[] { System.Linq.Expressions.Expression.Parameter(typeof(double), "x") };

                var lambda = DynamicExpression.ParseLambda(paramExpressions, typeof(double), this.functionString);
                this.F = (Func<double, double>)lambda.Compile();
                IsValid = true;
            }
            catch
            {
                IsValid = false;
            }
        }

        internal override void UpdatePlotData(Telerik.Windows.Data.RadObservableCollection<PlotInfo> dataCollection,
            Plot2DRange plotRange, uint samplesCount)
        {
            if (!this.IsValid)
                return;

            dataCollection.SuspendNotifications();

            dataCollection.Clear();

            double step = (plotRange.XMax - plotRange.XMin) / samplesCount;

            for (double x = plotRange.XMin; x <= plotRange.XMax; x += step)
            {
                double y = this.F(x);
                if (Double.IsInfinity(y) || Double.IsNaN(y))
                    dataCollection.Add(new PlotInfo(x, double.NaN));

                dataCollection.Add(new PlotInfo(x, y));
            }

            dataCollection.ResumeNotifications();
        }

        public override FunctionBase Clone()
        {
            return new RealFunction()
            {
                FunctionString = string.Copy(this.functionString)
            };
        }
    }

    public class InverselRealFunction : FunctionBase
    {
        public Func<double, double> F { get; protected set; }

        protected override void Validate()
        {
            try
            {
                var paramExpressions = new System.Linq.Expressions.ParameterExpression[] { System.Linq.Expressions.Expression.Parameter(typeof(double), "y") };

                var lambda = DynamicExpression.ParseLambda(paramExpressions, typeof(double), this.functionString);
                this.F = (Func<double, double>)lambda.Compile();
                IsValid = true;
            }
            catch
            {
                IsValid = false;
            }
        }

        internal override void UpdatePlotData(Telerik.Windows.Data.RadObservableCollection<PlotInfo> dataCollection,
            Plot2DRange plotRange, uint samplesCount)
        {
            if (!this.IsValid)
                return;

            dataCollection.SuspendNotifications();

            dataCollection.Clear();

            double step = (plotRange.YMax - plotRange.YMin) / samplesCount;

            for (double y = plotRange.YMin; y <= plotRange.YMax; y += step)
            {
                double x = this.F(y);
                if (Double.IsInfinity(x) || Double.IsNaN(x))
                    continue;

                dataCollection.Add(new PlotInfo(x, y));
            }

            dataCollection.ResumeNotifications();
        }

        public override FunctionBase Clone()
        {
            return new InverselRealFunction()
            {
                FunctionString = string.Copy(this.functionString)
            };
        }
    }
}
