using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FunctionPlotter;
using FunctionPlotter.Models;
using FunctionPlotter.ViewModels;

namespace FunctionPlotter.Views
{
    /// <summary>
    /// Interaction logic for FunctionPanelControl.xaml
    /// </summary>
    public partial class FunctionPanelControl : UserControl
    {
        public FunctionPanelControl()
        {
            InitializeComponent();
        }

        private PlotViewModel ViewModel
        {
            get
            {
                return this.DataContext as PlotViewModel;
            }
        }

        private FunctionViewModel FunctionBeingEdited
        {
            get
            {
                return this.ViewModel.FunctionBeingEdited;
            }
        }

        public void EditFunction()
        {
            this.functionPanel.Visibility = Visibility.Visible;
            this.addFunctionButtonPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void AddFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            var functionType = (string)(sender as Button).Tag;

            switch (functionType)
            {
                case "RF":
                    this.ViewModel.FunctionBeingEdited = FunctionViewModel.CreateRealFunctionVM((this.DataContext as PlotViewModel).PlotRange);
                    break;
                case "IRF":
                    this.ViewModel.FunctionBeingEdited = FunctionViewModel.CreateInverseRealFunctionVM((this.DataContext as PlotViewModel).PlotRange);
                    break;
                default:
                    break;
            }

            this.functionPanel.Visibility = Visibility.Visible;
            this.addFunctionButtonPanel.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void DiscardButton_Click(object sender, RoutedEventArgs e)
        {
            this.functionPanel.Visibility = Visibility.Collapsed;
            this.addFunctionButtonPanel.Visibility = System.Windows.Visibility.Visible;

            if (this.FunctionBeingEdited.IsBeingEdited)
                (this.DataContext as PlotViewModel).RevertChanges();
            else
                (this.DataContext as PlotViewModel).ClearEditedFunction();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (!this.FunctionBeingEdited.IsBeingEdited)
            {
                this.ViewModel.FunctionVMs.Add(this.FunctionBeingEdited);
            }

            this.ViewModel.ClearEditedFunction();

            this.functionPanel.Visibility = Visibility.Collapsed;
            this.addFunctionButtonPanel.Visibility = System.Windows.Visibility.Visible;
        }
    }
}