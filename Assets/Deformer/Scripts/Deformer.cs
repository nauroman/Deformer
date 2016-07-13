using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Flashunity.Deformer
{
    public class Deformer
    {
        static public bool Deform(Mesh mesh, Vector3 point, Vector3 power, DeformSettings settings)
        {
            var vertices = mesh.vertices;

            var magnitude = power.magnitude * settings.softness;// * 4;

            //            power = transform.InverseTransformVector(power);

            var min = new Vector3(point.x - magnitude, point.y - magnitude, point.z - magnitude);
            var max = new Vector3(point.x + magnitude, point.y + magnitude, point.z + magnitude);

            var indices = GetIndicesInTheCube(vertices, min, max);

            if (ShiftVertices(indices, vertices, point, power * settings.softness, settings))
            {
                mesh.vertices = vertices;

                mesh.RecalculateNormals();

                return true;
            }

            return false;
        }

        static bool ShiftVertices(List<int> indices, Vector3[] vertices, Vector3 point, Vector3 power, DeformSettings settings)
        {
            bool shifted = false;

            for (int i = 0; i < indices.Count; i++)
            {
                var index = indices [i];
                var vertex = vertices [index];

                //                var distance = Mathf.Sqrt(Mathf.Sqrt(Vector3.Distance(vertex, point)));
                var distance = Vector3.Distance(vertex, point);

                //                var moveTo = power * hardness * (Mathf.Sin(distance / 10) * 10);
                //       var moveTo = power * (Mathf.Sin(distance / 100) * 100);

                //                distance = Mathf.Sin(distance / 0.1f) * 0.05f / (distance * distance);

                //                var distanceDecrease = (0.1f - (distance * distance) / 20);
                var distanceDecrease = (0.1f - distance / 20);

                if (distanceDecrease > 0)
                {

                    distance = Mathf.Cos(distance / 0.2f) * distanceDecrease;

                    var moveTo = power * settings.softness * distance;


                    //                var moveTo = power * (hardness - distance / 1000);

                    //        var shiftDistance = moveTo.magnitude;

                    //                if (shiftDistance >= minVertexShift)
                    //              {

                    //                if (shiftDistance > maxVertexShift)
                    //                  moveTo = maxVertexShift;

                    vertex += moveTo;

                    vertices [index] = vertex;
                    //                vertex = new Vector3(0, 0, 0);

                    shifted = true;
                }
                //                ShiftVertex(vertices [indices [i]], point, magnitude);
            }

            return shifted;
        }

        static List<int> GetIndicesInTheCube(Vector3[] meshVertices, Vector3 min, Vector3 max)
        {
            var indices = new List<int>();

            //            var meshVertices = mesh.vertices;

            for (int i = 0; i < meshVertices.Length; i++)
            {
                var v = meshVertices [i];

                if (v.x >= min.x && v.x <= max.x && v.y >= min.y && v.y <= max.y && v.z >= min.z && v.z <= max.z)
                {
                    //                    vertices.Add(v);
                    indices.Add(i);
                }
            }

            return indices;
        }
    }

    public class DeformSettings
    {
        public float softness = 0.5f;

        public float spread = 0.5f;
        public float wave = 0.5f;
        public float noise = 0.1f;

        public float minVertexShift = 0.0001f;
        public float maxVertexShift = 1;

        //        public float
    }


}
