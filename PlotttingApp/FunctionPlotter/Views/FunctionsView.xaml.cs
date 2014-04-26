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
    /// Interaction logic for FunctionsView.xaml
    /// </summary>
    public partial class FunctionsView : UserControl
    {
        public FunctionsView()
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

        private void EditFunctionButton_Click(object sender, RoutedEventArgs e)
        {
            var func = (sender as Button).DataContext as FunctionViewModel;

            this.ViewModel.EditFunction(func);            

            this.functionPanel.EditFunction();
        }
    }
}
