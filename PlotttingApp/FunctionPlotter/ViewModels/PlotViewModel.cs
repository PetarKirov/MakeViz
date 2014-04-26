using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FunctionPlotter.Models;
using FunctionPlotter.ViewModels;
using Telerik.Windows.Controls;

namespace FunctionPlotter.ViewModels
{
    public class PlotViewModel : ViewModelBase
    {
        public ObservableCollection<FunctionViewModel> FunctionVMs { get; private set; }

        public Plot2DRange PlotRange { get; private set; } 

        public FunctionViewModel FunctionBeingEdited
        {
            get
            {
                return this.functionBeingEdited;
            }
            internal set
            {
                if (this.functionBeingEdited == value)
                    return;

                this.functionBeingEdited = value;

                this.OnPropertyChanged("FunctionBeingEdited");
            }
        }

        private FunctionViewModel functionBeingEdited;        

        internal FunctionViewModel Old { get; set; }

        public PlotViewModel()
        {
            this.FunctionVMs = new ObservableCollection<FunctionViewModel>();
            this.PlotRange = new Plot2DRange(-100, 100, -100, 100);

            this.FunctionVMs.Add(FunctionViewModel.CreateRealFunctionVM("Math.Pow(x, 2)", this.PlotRange));
            this.FunctionVMs.Add(FunctionViewModel.CreateRealFunctionVM("3 * x + 2", this.PlotRange));
        }

        internal void RevertChanges()
        {
            int index = this.FunctionVMs.IndexOf(this.FunctionBeingEdited);
            this.FunctionVMs[index] = this.Old;
            this.ClearEditedFunction();
        }

        internal void ClearEditedFunction()
        {
            this.Old = null;
            this.FunctionBeingEdited.IsBeingEdited = false;
            this.FunctionBeingEdited = null;
        }

        internal void EditFunction(FunctionViewModel func)
        {
            func.IsBeingEdited = true;

            this.FunctionBeingEdited = func;

            this.Old = func.CloneWithoutPlotData();
        }
    }
}