using UnityEngine;

[ExecuteInEditMode] 
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class Pyramid : MonoBehaviour
{
    public float baseSize = 1f;
    public float height = 1f;
    public bool generateCollider = false;

    private float lastBaseSize;
    private float lastHeight;

    void Update()
    {
        
        if (baseSize != lastBaseSize || height != lastHeight)
        {
            GeneratePyramid();
            lastBaseSize = baseSize;
            lastHeight = height;
        }
    }

    void GeneratePyramid()
    {
        Mesh mesh = new Mesh();
        mesh.name = "SquarePyramid";

        float half = baseSize * 0.5f;

        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-half, 0f, -half),
            new Vector3( half, 0f, -half),
            new Vector3( half, 0f,  half),
            new Vector3(-half, 0f,  half),
            new Vector3( 0f,    height, 0f)
        };

        int[] triangles = new int[]
        {
            0, 2, 1,
            0, 3, 2,

            4, 0, 1,
            4, 1, 2,
            4, 2, 3,
            4, 3, 0
        };

        Vector2[] uvs = new Vector2[vertices.Length];
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(0, 1);
        uvs[4] = new Vector2(0.5f, 0.5f);

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        GetComponent<MeshFilter>().sharedMesh = mesh;

        if (generateCollider)
        {
            var mc = GetComponent<MeshCollider>();
            if (mc == null) mc = gameObject.AddComponent<MeshCollider>();
            mc.sharedMesh = mesh;
        }
    }
}
