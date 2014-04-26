using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FunctionPlotter
{
    /// <summary>
    /// Represents a 3D point on the Cartesian coordinate system. 
    /// The data item of the plotting engine.
    /// </summary>
    public class PlotInfo
    {
        public PlotInfo()
        {
            
        }

        public PlotInfo(double x, double y, double z = 0.0)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
    }
}
