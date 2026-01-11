using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class EnemyFOVMesh : MonoBehaviour
    {
        private struct ViewCast
        {
            public readonly bool Hit;
            public readonly Vector3 Point;
            public readonly float Dist;
            public readonly float Angle;

            public ViewCast(bool hit, Vector3 point, float dist, float angle)
            {
                Hit = hit;
                Point = point;
                Dist = dist;
                Angle = angle;
            }
        }

        private struct EdgeInfo
        {
            public readonly Vector3 PointA;
            public readonly Vector3 PointB;

            public EdgeInfo(Vector3 a, Vector3 b)
            {
                PointA = a;
                PointB = b;
            }
        }
        
        [Header("Shape")]
        [SerializeField][Range(1f, 360f)] private float angle = 90f;
        public float Angle => angle;
        
        [SerializeField] private float radius = 8f;
        public float Radius => radius;
        
        [SerializeField][Range(3, 256)] private int segments = 20;
        [SerializeField] private Material fovMaterial;

        [Header("Raycast / Clipping")]
        [SerializeField] private LayerMask obstacleMask;
        [SerializeField] private float originYOffset = 0.1f;
        [SerializeField] private bool refineEdges = true;
        [SerializeField][Range(0, 6)] private int refineIterations = 4;

        [Header("Update")]
        [SerializeField] private bool updateEveryFrame = true;
        [SerializeField][Range(0.02f, 1f)] private float updateInterval = 0.07f;

        private Mesh _mesh;
        private float _t;

        private bool _isVisible = true;

        private void Awake()
        {
            _mesh = new Mesh { name = "Enemy_FOV_Pie" };
            GetComponent<MeshFilter>().sharedMesh = _mesh;
            
            if (fovMaterial != null)
                GetComponent<MeshRenderer>().sharedMaterial = fovMaterial;
        }

        private void LateUpdate()
        {
            if (!_isVisible)
                return;
            
            if (updateEveryFrame)
            {
                Rebuild();
                return;
            }
            
            _t += Time.deltaTime;
            if (_t >= updateInterval)
            {
                _t = 0f;
                Rebuild();
            }
        }

        private void Rebuild()
        {
            Vector3 originWorld = transform.position + Vector3.up * originYOffset;

            var pointsWorld = new List<Vector3>(segments + 8);

            float half = angle * 0.5f;
            float step = angle / segments;

            // This is the *global* yaw center of the enemy forward direction
            float centerYaw = transform.eulerAngles.y;

            ViewCast previousCast = default;
            bool hasPrev = false;

            for (int i = 0; i <= segments; i++)
            {
                float localA = -half + step * i;
                float globalA = centerYaw + localA;

                ViewCast currentCast = CastView(originWorld, globalA);

                if (refineEdges && hasPrev)
                {
                    bool edgeBoundary = previousCast.Hit != currentCast.Hit;

                    bool distanceJump = previousCast.Hit && currentCast.Hit &&
                                        Mathf.Abs(previousCast.Dist - currentCast.Dist) > 0.5f;

                    if (edgeBoundary || distanceJump)
                    {
                        EdgeInfo edge = FindEdge(originWorld, previousCast, currentCast);

                        pointsWorld.Add(edge.PointA);
                        pointsWorld.Add(edge.PointB);
                    }
                }

                pointsWorld.Add(currentCast.Point);

                previousCast = currentCast;
                hasPrev = true;
            }

            int vertCount = pointsWorld.Count + 1;
            var verts = new Vector3[vertCount];
            var tris = new int[(vertCount - 2) * 3];

            // Center
            verts[0] = transform.InverseTransformPoint(originWorld);

            // Perimeter
            for (int i = 0; i < pointsWorld.Count; i++)
                verts[i + 1] = transform.InverseTransformPoint(pointsWorld[i]);

            // Triangles fan
            for (int i = 0; i < vertCount - 2; i++)
            {
                int t = i * 3;
                tris[t + 0] = 0;
                tris[t + 1] = i + 1;
                tris[t + 2] = i + 2;
            }

            _mesh.Clear();
            _mesh.vertices = verts;
            _mesh.triangles = tris;
            _mesh.RecalculateBounds();
            _mesh.RecalculateNormals();
        }

        private ViewCast CastView(Vector3 originWorld, float globalAngleDeg)
        {
            Vector3 dir = DirFromGlobalAngle(globalAngleDeg);
            if (Physics.Raycast(originWorld, dir, out RaycastHit hit, radius, obstacleMask, QueryTriggerInteraction.Ignore))
                return new ViewCast(true, hit.point, hit.distance, globalAngleDeg);

            return new ViewCast(false, originWorld + dir * radius, radius, globalAngleDeg);
        }

        private EdgeInfo FindEdge(Vector3 originWorld, ViewCast minCast, ViewCast maxCast)
        {
            float minAngle = minCast.Angle;
            float maxAngle = maxCast.Angle;

            Vector3 minPoint = minCast.Point;
            Vector3 maxPoint = maxCast.Point;

            for (int i = 0; i < refineIterations; i++)
            {
                float midAngle = (minAngle + maxAngle) * 0.5f;
                ViewCast midCast = CastView(originWorld, midAngle);

                if (midCast.Hit == minCast.Hit)
                {
                    minAngle = midAngle;
                    minPoint = midCast.Point;
                }
                else
                {
                    maxAngle = midAngle;
                    maxPoint = midCast.Point;
                }
            }

            return new EdgeInfo(minPoint, maxPoint);
        }

        private Vector3 DirFromGlobalAngle(float globalAngleDeg)
        {
            float rad = globalAngleDeg * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(rad), 0f, Mathf.Cos(rad));
        }
        
        public void SetVisible(bool visible)
        {
            _isVisible = visible;

            if (visible)
            {
                Rebuild();
            }
            else
            {
                _mesh.Clear();
            }
        }
    }
}
