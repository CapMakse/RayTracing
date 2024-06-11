using System.Numerics;
using System.Runtime.InteropServices;

namespace RayTracing
{

    public class Polygon
    {
        public Point A {  get; set; }
        public Point B {  get; set; }
        public Point C {  get; set; }
        public Polygon(Vector3 a, Vector3 b, Vector3 c)
        {
            A = new Point(a);
            B = new Point(b);
            C = new Point(c);
        }
        public float Intersect(Vector3 Ray)
        {
            Vector3 e1 = B.P - A.P;
            Vector3 e2 = C.P - A.P;
            // Вычисление вектора нормали к плоскости
            Vector3 pvec = Vector3.Cross(Ray, e2);
            float det = Vector3.Dot(e1, pvec);

            // Луч параллелен плоскости
            if (det < 1e-8 && det > -1e-8)
            {
                return -1;
            }

            float inv_det = 1 / det;
            Vector3 tvec = -A.P;
            float u = Vector3.Dot(tvec, pvec) * inv_det;
            if (u < 0 || u > 1)
            {
                return -1;
            }

            Vector3 qvec = Vector3.Cross(tvec, e1);
            float v = Vector3.Dot(Ray, qvec) * inv_det;
            if (v < 0 || u + v > 1)
            {
                return -1;
            }
            return Vector3.Dot(e2, qvec) * inv_det;
        }
    }
}
