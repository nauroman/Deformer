using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Flashunity.Deformer
{

    public class BDeform : MonoBehaviour
    {
        public MeshFilter meshFilter;
        public MeshFilter latticedMeshFilter;

        Mesh mesh;
        //        Mesh latticedMesh;

        //        public Rigidbody rigidBody;
        //      public Rigidbody latticedRigidBody;

        //        [SerializeField]
        //      GameObject gameObjectBeforeDeform;

        //    [SerializeField]
        //        GameObject gameObjectAfterDeform;

        //        [SerializeField]
        //      GameObject rigid;

        //    [SerializeField]
        //        Rigidbody rigidBodyBeforeDeform;

        //        [SerializeField]
        //        Rigidbody rigidBodyAfterDeform;

        public bool deformRigidBody = true;

        Mesh meshRigidBody;

        //        Rigidbody rigidBody;

        //      MeshCollider meshCollider;

        //    Mesh highResMesh;

        [SerializeField]
        AudioSource audioSource;

        float[] verticesShifts = new float[1];

        public float minForce = 0.01f;
        public float maxForce = 1000f;

        public float softness = 0.5f;

        public float minVertexShift = 0.0001f;
        public float maxVertexShift = 1;

        Vector3 startPos;

        bool deformed;

        bool coroutineIsRunning;

        public bool moveUp = true;

        public bool Deformed{ get { return deformed; } }

        void Start()
        {
            audioSource = GetComponent<AudioSource>();

            startPos = transform.position;



//            meshCollider = GetComponent<MeshCollider>();

            //          meshCollider.sharedMesh = rigid.GetComponent<MeshFilter>().mesh;
            //          rigidBody = GetComponent<Rigidbody>();

//            highResMesh = gameObjectAfterDeform.GetComponent<MeshFilter>().mesh;

//            verticesMoveDistances = new float[highResMesh.vertexCount];

            /*
            if (meshFilter != null)
                mesh = meshFilter.mesh;

            if (latticedMeshFilter != null)
                mesh = latticedMeshFilter.mesh;
            else if (meshFilter != null)
                mesh = meshFilter.mesh;

//            verticesMoveDistances = Enumerable.Repeat(0.0f, highResMesh.vertexCount).ToArray();

            if (latticedMesh != null)
                verticesShifts = Enumerable.Repeat<float>(0.0f, latticedMesh.vertexCount).ToArray();
            else if (mesh != null)
                verticesShifts = Enumerable.Repeat<float>(0.0f, mesh.vertexCount).ToArray();
*/



            if (latticedMeshFilter != null && meshFilter != null && latticedMeshFilter != meshFilter)
                latticedMeshFilter.gameObject.SetActive(false);



//            if (rigidBody != null && latticedRigidBody != null && rigidBody != latticedRigidBody)
            //              latticedRigidBody.gameObject.SetActive(false);


            //meshFilter.
//            rigidBody.co

//            gameObjectAfterDeform.SetActive(false);

//            var rigidMeshRenderer = rigidBody.GetComponent<MeshRenderer>();

            //          if (rigidMeshRenderer != null)
            //            Destroy(rigidMeshRenderer);
        }

        void OnCollisionEnter(Collision collision)
        {
            //          Debug.Log(collision.relativeVelocity.magnitude);

//            rigidBody.MovePosition(new Vector3(rigid.transform.position.x, rigid.transform.position.y + 5, rigid.transform.position.z));

//            var magnitude = collision.relativeVelocity.magnitude;
//            rb.transform.localPosition

            var relativeVelocity = collision.relativeVelocity;
            var mass = collision.rigidbody.mass;

            var force = new Vector3(relativeVelocity.x * relativeVelocity.x * mass, relativeVelocity.y * relativeVelocity.y * mass, relativeVelocity.z * relativeVelocity.z * mass); // v*v*mass
            var magnitude = force.magnitude;

            if (minForce < 0)
                minForce = 0;

            if (maxForce < minForce)
                maxForce = minForce;

            if (magnitude >= minForce)
            {
                if (magnitude > maxForce)
                {
                    force = force * (maxForce / magnitude);
                }

                if (Deform(collision.contacts [0].point, force))
                {
                    if (audioSource != null)
                        audioSource.Play();
                    
                }


                if (moveUp && !coroutineIsRunning)
                    StartCoroutine(MoveUp(3));
                
                //rigid.GetPointVelocity(transform.TransformPoint(localWheelPos));
            }
        }

        IEnumerator MoveUp(float time)
        {
            coroutineIsRunning = true;
            //yield return new WaitForSeconds(time);

            yield return new WaitForSeconds(time);
//            rigidBody.MovePosition(new Vector3(5, rigid.transform.position.y + 5, -5));

            //           rigidBody.isKinematic = true;

            var rigid = GetComponent<Rigidbody>();

            rigid.isKinematic = true;

//            rigidBody.MovePosition(new Vector3(5, rigid.transform.position.y + 5, -5));

            rigid.transform.position = new Vector3(startPos.x, startPos.y + Random.Range(2, 6), startPos.z);
            rigid.transform.Rotate(Random.Range(-180, 180), Random.Range(-180, 180), Random.Range(-180, 180));

//            meshCollider.sharedMesh = highResMesh;

            rigid.isKinematic = false;

            coroutineIsRunning = false;
//            rigidBody.MoveRotation(new Quaternion(Random.Range(0, 90), Random.Range(0, 90), Random.Range(0, 90), 1));

        }

        public bool Deform(Vector3 point, Vector3 power)
        {

            bool twoMeshes = false;
//            Mesh mesh;

            if (!deformed)
            {
                if (latticedMeshFilter != null)
                    mesh = latticedMeshFilter.mesh;
                else if (meshFilter != null)
                    mesh = meshFilter.mesh;

                if (mesh != null)
                {
                    if (latticedMeshFilter != null && meshFilter != null && meshFilter != latticedMeshFilter)
                    {
                        twoMeshes = true;
                        latticedMeshFilter.gameObject.SetActive(true);
                        meshFilter.gameObject.SetActive(false);
                    }
//                    deformed = true;
                }
            }

            //          var position = transform.position;

//            var magnitude = power.magnitude * hardness;// * 4;

            var settings = new DeformSettings
            {
                softness = softness,
                minVertexShift = minVertexShift,
                maxVertexShift = maxVertexShift,

            };

            point = transform.InverseTransformPoint(point);
            power = transform.InverseTransformPoint(power);

            var d = Deformer.Deform(mesh, point, power, settings);

            /*
            if (d)
            {
                deformed = true;

                if (deformRigidBody)
                {
                    if (meshRigidBody == null)
                    {
                        if (latticedMeshFilter != null)
                        {
                            var meshCollider = latticedMeshFilter.GetComponent<MeshCollider>();

                            if (meshCollider != null)
                                meshRigidBody = meshCollider.sharedMesh;
                        } else if (meshFilter != null)
                        {
                            var meshCollider = meshFilter.GetComponent<MeshCollider>();

                            if (meshCollider != null)
                                meshRigidBody = meshCollider.sharedMesh;
                        }
                    }

                    if (meshRigidBody != null)
                    {
                        Deformer.Deform(meshRigidBody, point, power, settings);
                    }
                }
            }
            */

            if (twoMeshes && !deformed && !d)
            {
                latticedMeshFilter.gameObject.SetActive(false);
                meshFilter.gameObject.SetActive(true);
            }

            return d;

            /*
            point = transform.InverseTransformPoint(point);

            var vertices = mesh.vertices;

//            power = transform.InverseTransformVector(power);

            var min = new Vector3(point.x - magnitude, point.y - magnitude, point.z - magnitude);
            var max = new Vector3(point.x + magnitude, point.y + magnitude, point.z + magnitude);

            var indices = GetIndicesInTheCube(vertices, min, max);

            if (ShiftVertices(indices, vertices, point, transform.InverseTransformVector(power * hardness)))
            {

                mesh.vertices = vertices;

                mesh.RecalculateNormals();

                return true;
            }

            return false;
            */
        }

        /*
        bool ShiftVertices(List<int> indices, Vector3[] vertices, Vector3 point, Vector3 power)
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

                    var moveTo = power * hardness * distance;


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
        */

        /*
        void ShiftVertex(Vector3 vertex, Vector3 point, float magnitude)
        {
            vertex = new Vector3(0, 0, 0);

//            vertex += new Vector3(0, 1, 0);
        }
        */

        /*
        List<int> GetIndicesInTheCube(Vector3[] meshVertices, Vector3 min, Vector3 max)
        {
//            vertices = new List<Vector3>();
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
        */

    }
}