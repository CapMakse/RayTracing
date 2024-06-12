using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace RayTracing
{
    public class Render
    {
        private Point _camera = new Point(new Vector3(0, 0, 0));
        private int _width;
        private int _height;
        private Color[][] _screen;
        private Point[] _screenCorners = new Point[4];
        public Render (int width, int height, int FOV)
        {
            if (FOV >= 180 || FOV <= 0) return;
            _width = width;
            _height = height;
            float x = width / 2;
            float y = height / 2;
            float z = (float)(width / 2 * Math.Tan(FOV / 2 * Math.PI / 180));
            _screenCorners[0] = new Point(new Vector3(x,y,z));
            _screenCorners[1] = new Point(new Vector3(x,-y,z));
            _screenCorners[2] = new Point(new Vector3(-x,y,z));
            _screenCorners[3] = new Point(new Vector3(-x,-y,z));
            _screen = new Color[width][];
            for (int i = 0; i < width; i++)
            {
                _screen[i] = new Color[height];
                for (int j = 0; j < height; j++)
                {
                    _screen[i][j] = new Color(0, 0, 0);
                }
            }
        }
        public void RotateCamera(Vector3 axisStart, Vector3 axisEnd, double angle)
        {
            List<Point> points =
            [
                _camera,
                _screenCorners[0],
                _screenCorners[1],
                _screenCorners[2],
                _screenCorners[3],
            ];
            Tranform.Rotate(points, axisStart, axisEnd, angle);
        }
        public void ShiftCamera(Vector3 transposition)
        {
            List<Point> points =
            [
                _camera,
                _screenCorners[0],
                _screenCorners[1],
                _screenCorners[2],
                _screenCorners[3],
            ];
            Tranform.Shift(points, transposition);
        }
        public void RenderImage(List<Polygon> polygons)
        {
            for (int i = 0; i < _width; i++)
            {
                float hRatio = (float)i / ((float)_width - 1 - i);
                Vector3 up = GetPointRatio(_screenCorners[2].P, _screenCorners[0].P, hRatio);
                Vector3 down = GetPointRatio(_screenCorners[3].P, _screenCorners[1].P, hRatio);
                for (int j = 0; j < _height; j++)
                {
                    float vRatio = (float)j / ((float)_height - 1 - j);
                    Vector3 screenPoint = GetPointRatio(up, down, vRatio);
                    Vector3 ray = screenPoint - _camera.P;
                    //
                    Polygon polygon = RenderRay(polygons, ray);
                    if (polygon != null) _screen[i][j] = polygon.Color;
                    //
                }
            }
        }
        private Polygon RenderRay(List<Polygon> polygons, Vector3 ray)
        {
            float distance = float.MaxValue;
            Polygon res = null;
            foreach (Polygon polygon in polygons)
            {
                float newDistance = polygon.Intersect(ray);
                if (newDistance == -1) continue;
                if (distance > newDistance)
                {
                    distance = newDistance;
                    res = polygon;
                }
            }
            return res;
        } 
        private Vector3 GetPointRatio(Vector3 a, Vector3 b, float ratio)
        {
            float x = (a.X + ratio * b.X) / (1 + ratio);
            float y = (a.Y + ratio * b.Y) / (1 + ratio);
            float z = (a.Z + ratio * b.Z) / (1 + ratio);
            return new Vector3(x, y, z);
        }
    }
}
