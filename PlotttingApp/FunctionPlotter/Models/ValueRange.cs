using System;

namespace FunctionPlotter
{
    public struct ValueRange
    {
        public ValueRange(double min, double max) : this()
        {
            this.Min = min;
            this.Max = max;
        }

        public double Min { get; set; }

        public double Max { get; set; }
    }
}