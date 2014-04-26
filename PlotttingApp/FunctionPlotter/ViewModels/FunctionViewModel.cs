using System;
using FunctionPlotter;
using FunctionPlotter.Models;
using Telerik.Windows.Controls;

namespace FunctionPlotter.ViewModels
{
    public class FunctionViewModel : ViewModelBase
    {        
        #region Private Members

        private uint samplesCount;

        private bool isBeingEdited;

        #endregion

        #region Public Properties

        public FunctionBase Function { get; private set; }

        public Plot2DRange PlotRange { get; private set; }
        
        //Last valid PlotData
        public Telerik.Windows.Data.RadObservableCollection<PlotInfo> PlotData { get; private set; }
        
        public bool IsBeingEdited
        {
            get
            {
                return this.isBeingEdited;
            }
            set
            {
                if (this.isBeingEdited == value)
                    return;

                this.isBeingEdited = value;
            
                this.OnPropertyChanged("IsBeingEdited");
            }
        }              
        
        public uint SamplesCount
        {
            get
            {
                return this.samplesCount;
            }
            set
            {
                if (this.samplesCount == value)
                    return;

                this.samplesCount = value;

                this.UpdatePlotData();

                this.OnPropertyChanged("SamplesCount");
            }
        }

        #endregion

        #region Constructors, Factory and Clone Methods
        
        private FunctionViewModel(Plot2DRange plotRange)
            : this(new RealFunction(), plotRange, 1000)
        {
        }

        private FunctionViewModel(FunctionBase function, Plot2DRange plotRange, uint samplesCount)
        {
            this.PlotData = new Telerik.Windows.Data.RadObservableCollection<PlotInfo>();
            this.isBeingEdited = false;
            this.samplesCount = samplesCount;

            this.Function = function;
            this.Function.PropertyChanged += (s,e) => this.UpdatePlotData();

            this.PlotRange = plotRange;
            this.PlotRange.PropertyChanged += (s, e) => this.UpdatePlotData();
        }

        public static FunctionViewModel CreateRealFunctionVM(Plot2DRange plotRange)
        {
            return new FunctionViewModel(plotRange);
        }

        public static FunctionViewModel CreateRealFunctionVM(string functionString, Plot2DRange plotRange)
        {
            var f = new FunctionViewModel(plotRange);
            f.Function.FunctionString = functionString;
            return f;
        }

        public static FunctionViewModel CreateRealFunctionVM(Plot2DRange plotRange, uint samplesCount)
        {
            return new FunctionViewModel(new RealFunction(), plotRange, samplesCount);
        }

        public static FunctionViewModel CreateInverseRealFunctionVM(Plot2DRange plotRange)
        {
            return new FunctionViewModel(
                new InverselRealFunction(),
                plotRange,
                1000);
        }

        public static FunctionViewModel CreateInverseRealFunctionVM(Plot2DRange plotRange, uint samplesCount)
        {
            return new FunctionViewModel(
                new InverselRealFunction(),
                plotRange,
                samplesCount);
        }
        
        public FunctionViewModel CloneWithoutPlotData()
        {
            //this should NOT trigger an update to the PlotData
            return new FunctionViewModel(this.Function.Clone(),
                this.PlotRange, this.samplesCount)
            {
                isBeingEdited = this.IsBeingEdited,
            };
        }
        
        public FunctionViewModel CloneAndUpdatePlotData()
        {
            return new FunctionViewModel(this.Function.Clone(),
                this.PlotRange, this.samplesCount)
            {
                isBeingEdited = this.IsBeingEdited,
                SamplesCount = this.SamplesCount //this SHOULD trigger an update to the PlotData
            };
        }

        #endregion
        
        private void UpdatePlotData()
        {
            this.Function.UpdatePlotData(this.PlotData,
                this.PlotRange,
                this.SamplesCount);
        }
    }
}