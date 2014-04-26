using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Telerik.Windows.Controls;

namespace PlotttingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
    }

    public class MainViewModel : ViewModelBase
    {
        public ObservableCollection<PlotInfo> Data { get; private set; }

        public ObservableCollection<LineInfo> Lines { get; private set; }

        public double TargetFunctionValue
        {
            get
            {
                return this.targetFunctionValue;
            }
            set
            {
                this.targetFunctionValue = value;
                this.CalculateLine();
                this.OnPropertyChanged("TargetFunctionValue");
            }
        }

        private double targetFunctionValue;

        private Func<double, double, double> targetFunction;

        private List<Restriction> restrictions;

        private ValueRange rangeX1;

        private ValueRange rangeX2;

        private double step;

        public MainViewModel()
        {
            this.targetFunction = (x, a) => (3.0 * x - a) / 2.0;
            this.restrictions = GetRestrictions();

            this.rangeX1 = new ValueRange(-30, 30);
            this.rangeX2 = new ValueRange(-30, 30);
            this.step = 0.5;

            this.Data = GenerateData();
            this.Lines = new ObservableCollection<LineInfo>() { new LineInfo() };
            GenerateRestrictionLines();
            this.TargetFunctionValue = 0;
        }

        private List<Restriction> GetRestrictions()
        {
            return new List<Restriction>()
            {
                new Restriction((a,b) => a >= b, (x1) => (x1 - 1) / 2.0),
                new Restriction((a,b) => a <= b, (x1) => (15 - 4 * x1) / 3.0),
                new Restriction((a,b) => a >= b, (x1) => (-x1 - 1) / 2.0),
                new Restriction((a,b) => a >= b, (x1) => 0.0),
            };
        }

        private void CalculateLine()
        {
            double xStart = rangeX1.Min;
            double xEnd = rangeX1.Max;

            double yStart = this.targetFunction(xStart, TargetFunctionValue);
            double yEnd = this.targetFunction(xEnd, TargetFunctionValue);

            this.Lines[0] = new LineInfo(xStart, yStart, xEnd, yEnd);
        }

        private void GenerateRestrictionLines()
        {
            double xStart = rangeX1.Min;
            double xEnd = rangeX1.Max;

            foreach (var restriction in this.restrictions)
            {
                double yStart = restriction.Function(xStart);
                double yEnd = restriction.Function(xEnd);

                this.Lines.Add(new LineInfo(xStart, yStart, xEnd, yEnd));
            }
        }

        private ObservableCollection<PlotInfo> GenerateData()
        {
            var result = new ObservableCollection<PlotInfo>();

            for (double x1 = rangeX1.Min; x1 < rangeX1.Max; x1 += step)
            {
                for (double x2 = rangeX2.Min; x2 < rangeX2.Max; x2 += step)
                {
                    if (Check(x1, x2, restrictions))
                    {
                        result.Add(new PlotInfo { X = x1, Y = x2 });
                    }
                }
            }

            return result;
        }

        private bool Check(double x1, double x2, List<Restriction> restrictions)
        {
            bool areSatisfied = true;

            foreach (var restriction in restrictions)
            {
                if (!restriction.Check(x1, x2))
                {
                    areSatisfied = false;
                    break;
                }
            }

            return areSatisfied;
        }
    }

    public class PlotInfo
    {
        public PlotInfo()
        {
        }

        public PlotInfo(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }

        public double X { get; set; }

        public double Y { get; set; }
    }

    public class LineInfo : ViewModelBase
    {
        public LineInfo()
        { 
            this.Data = new List<PlotInfo>()
            {
                new PlotInfo(),
                new PlotInfo()
            };
        }

        public LineInfo(double startX, double startY, double endX, double endY) : this()
        {
            this.StartX = startX;
            this.StartY = startY;
            this.EndX = endX;
            this.EndY = endY;
        }

        public List<PlotInfo> Data { get; private set; }

        public double StartX
        {
            get
            {
                return this.Data[0].X;
            }
            set
            {
                this.Data[0].X = value;
            }
        }

        public double StartY
        {
            get
            {
                return this.Data[0].Y;
            }
            set
            {
                this.Data[0].Y = value;
            }
        }

        public double EndX
        {
            get
            {
                return this.Data[1].X;
            }
            set
            {
                this.Data[1].X = value;
            }
        }

        public double EndY
        {
            get
            {
                return this.Data[1].Y;
            }
            set
            {
                this.Data[1].Y = value;
            }
        }
    }

    public class ValueRange
    {
        public ValueRange()
        {
        }

        public ValueRange(double min, double max)
        {
            this.Min = min;
            this.Max = max;
        }

        public double Min { get; set; }

        public double Max { get; set; }
    }

    public class Restriction
    {
        public Restriction()
        {
        }

        public Restriction(Func<double, double, bool> operation, Func<double, double> function)
        {
            this.Operation = operation;
            this.Function = function;
        }

        public Func<double, double, bool> Operation { get; set; }

        public Func<double, double> Function { get; set; }

        public bool Check(double x1, double x2)
        {
            return Operation(x2, Function(x1));
        }
    }
}