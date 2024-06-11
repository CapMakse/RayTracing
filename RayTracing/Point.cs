using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracing
{
    public class Point
    {
        public Vector3 P {  get; set; }
        public Point(Vector3 p)
        {
            P = p;
        }
    }
    public class PointComparer : IComparer<Point>
    {
        public int Compare(Point x, Point y)
        {
            if (x.P.X > y.P.X) { return 1; }
            if (x.P.X < y.P.X) { return -1; }
            if (x.P.Y > y.P.Y) { return 1; }
            if (x.P.Y < y.P.Y) { return -1; }
            if (x.P.Z > y.P.Z) { return 1; }
            if (x.P.Z < y.P.Z) { return -1; }
            return 0;
        }
    }
}
