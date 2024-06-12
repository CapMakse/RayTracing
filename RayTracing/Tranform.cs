using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace RayTracing
{
    static class Tranform
    {
        static public void Shift(Point point, Vector3 transposition)
        {
             point.P += transposition;
        }
        static public void Shift(List<Point> points, Vector3 transposition)
        {
            foreach (Point point in points)
            {
               Shift(point, transposition);
            }
        }
        static public void Rotate(Point point, Vector3 axisStart, Vector3 axisEnd, double angle)
        {
            angle = (angle * Math.PI / 180) / 2;
            Vector3 rotateVector = Vector3.Normalize(axisEnd - axisStart);

            Vector4 quat = GetQuat(rotateVector, angle);
            Vector4 negQuat = new Vector4(-quat.X, -quat.Y, -quat.Z, quat.W);

            point.P += axisStart;
            point.P = Rotate(point.P, quat, negQuat);
            point.P -= axisStart;
        }
        static public void Rotate(List<Point> points, Vector3 axisStart, Vector3 axisEnd, double angle)
        {
            points.Sort(new PointComparer());
            angle = (angle * Math.PI / 180) / 2;
            Vector3 rotateVector = Vector3.Normalize(axisEnd - axisStart);

            Vector4 quat = GetQuat(rotateVector, angle);
            Vector4 negQuat = new Vector4(-quat.X, -quat.Y, -quat.Z, quat.W);


            for (int i = 0; i < points.Count; i++)
            {
                int copies = 0;
                Vector3 point = points[i].P;
                point += axisStart;
                point = Rotate(point, quat, negQuat);
                point -= axisStart;
                for (int j = 1; ; j++)
                {
                    if (i + j >= points.Count) break;
                    if (points[i + j].P == points[i].P)
                    {
                        points[i + j].P = point;
                        copies++;
                    }
                    else break;
                }
                points[i].P = point;
                i += copies;
            }

        }
        static private Vector3 Rotate(Vector3 point, Vector4 quat, Vector4 negQuat)
        {
            Vector4 t;
            t = QuatMulVector(point, quat);
            t = QuatMulQuat(t, negQuat);
            point = new Vector3(t.X, t.Y, t.Z);
            return point;
        }
        static private Vector4 GetQuat(Vector3 rotateVector, double angle)
        {
            Vector4 quat = new Vector4();
            quat.W = (float)Math.Cos(angle);
            quat.X = (float)(rotateVector.X * Math.Sin(angle));
            quat.Y = (float)(rotateVector.Y * Math.Sin(angle));
            quat.Z = (float)(rotateVector.Z * Math.Sin(angle));
            return quat;
        }
        static private Vector4 QuatMulQuat(Vector4 a, Vector4 b)
        {
            float W = a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z;
            float X = a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y;
            float Y = a.W * b.Y - a.X * b.Z + a.Y * b.W + a.Z * b.X;
            float Z = a.W * b.Z + a.X * b.Y - a.Y * b.X + a.Z * b.W;
            Vector4 res = new Vector4(X, Y, Z, W);
            return res;

        }
        static private Vector4 QuatMulVector(Vector3 b, Vector4 a)
        {
            float W = -a.X * b.X - a.Y * b.Y - a.Z * b.Z;
            float X = a.W * b.X + a.Y * b.Z - a.Z * b.Y;
            float Y = a.W * b.Y - a.X * b.Z + a.Z * b.X;
            float Z = a.W * b.Z + a.X * b.Y - a.Y * b.X;
            Vector4 res = new Vector4(X, Y, Z, W);
            return res;

        }
    }
}
