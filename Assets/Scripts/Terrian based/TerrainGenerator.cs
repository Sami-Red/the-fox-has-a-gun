using Unity.AI.Navigation;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    // Terrain Sizes
    public int terrainWidth;
    public int terrainLength;

    // Perlin noise related
    public float perlinFrequencyX;
    public float perlinFrequencyZ;
    public float perlinStrength;

    // Advanced Terrain generation related
    public int octaves;
    public float persistence;
    public float lacunarity;
    public float heightScale;
    public float blue; // Lowest
    public float green; // Mid range
    public float white; //Highest

    public bool isTerrainReady { get; private set; } = false; // allows to be used in objectplacer
    public Vector3[] Vertices { get { return vertices; } } // allows to be used in objectplacer

    // Components
    private Mesh mesh;
    private MeshFilter mFilter;
    private MeshRenderer mRenderer;
    private MeshCollider mCollider;
    private NavMeshSurface navMeshSurface;

    // Mesh related Arrays
    private Vector3[] vertices;
    private int[] tris;
    private Vector2[] uvs;
    private Color[] colors;

    void Start()
    {
        // Gets components mesh related
        mFilter = GetComponent<MeshFilter>();
        mRenderer = GetComponent<MeshRenderer>();
        mCollider = GetComponent<MeshCollider>();
        navMeshSurface = GetComponent<NavMeshSurface>();

        mesh = new Mesh();
        mesh.name = "Procedural Terrain";

        mFilter.mesh = mesh;
        Material terrainMat = new Material(Shader.Find("Custom/StandardWithVertexColor")); // checks for mat
        mRenderer.material = terrainMat;

        MeshDataGenerator();
        InitializeTerrain();

        isTerrainReady = true;

        if (navMeshSurface != null) // Checks if the nav mesh is working, if it is there itll start creating the mesh. If it doesnt exist it wont show anything
        {
            navMeshSurface.BuildNavMesh();
        }
    }
    #region Height Related
    public float CurrentPositionHeight(float x, float z)
    {
        if (vertices == null || vertices.Length == 0)
        {
            return 0;
        }

        int vertexX = Mathf.Clamp(Mathf.FloorToInt(x), 0, terrainWidth);
        int vertexZ = Mathf.Clamp(Mathf.FloorToInt(z), 0, terrainLength);
        int vertexIndex = vertexZ * (terrainWidth + 1) + vertexX;

        return vertices[vertexIndex].y;
    }
    private float HeightGenerator(float x, float z)
    {
        float y = 0;
        float amplitude = 1f;
        float terrainPeak = 1f;

        for (int i = 0; i < octaves; i++)
        {
            y += Mathf.PerlinNoise(x * perlinFrequencyX * terrainPeak, z * perlinFrequencyZ * terrainPeak) * amplitude;
            amplitude *= persistence;
            terrainPeak *= lacunarity;
        }

        return y * perlinStrength * heightScale;
    }
    #endregion
    #region Clears mesh, calcuates normals, set colliders
    private void InitializeTerrain()
    {
        Mesh();
        UpdateMeshCollider();
    }
    private void UpdateMeshCollider()
    {
        if (mCollider != null)
        {
            mCollider.sharedMesh = mesh;
        }
    }
    private void Mesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.colors = colors;
        mesh.RecalculateNormals();
    }
    #endregion
    #region Mesh Generator
    private void MeshDataGenerator()
    {
        // Set arrays
        vertices = new Vector3[(terrainLength + 1) * (terrainWidth + 1)];
        colors = new Color[vertices.Length];
        tris = new int[terrainLength * terrainWidth * 6];
        uvs = new Vector2[vertices.Length];

        int i = 0;

        // loops through the terrain grid
        for (int z = 0; z <= terrainLength; z++)
        {
            for (int x = 0; x <= terrainWidth; x++)
            {
                float y = HeightGenerator(x, z);
                vertices[i] = new Vector3(x, y, z);

                float terHeight = Mathf.InverseLerp(0, perlinStrength * heightScale, y); // Sets the y value of hills

                // Assigns Colours.
                if (terHeight < blue) //lowest level
                { 
                    colors[i] = Color.blue;
                }
                else if (terHeight < green) //Middle level
                { 
                    colors[i] = Color.green;
                }
                else if (terHeight < white) //Mid top level
                {
                    colors[i] = new Color(0.13f, 0.55f, 0.13f); // Dark green & top middle level
                }
                else
                {
                    colors[i] = Color.white; // Top level
                }

                uvs[i] = new Vector2((float)x / terrainWidth, (float)z / terrainLength);

                i++;
            }
        }
        int triIndex = 0;
        int vertexIndex = 0;

        for (int z = 0; z < terrainLength; z++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                tris[triIndex + 0] = vertexIndex + 0;
                tris[triIndex + 1] = vertexIndex + terrainWidth + 1;
                tris[triIndex + 2] = vertexIndex + 1;

                tris[triIndex + 3] = vertexIndex + 1;
                tris[triIndex + 4] = vertexIndex + terrainWidth + 1;
                tris[triIndex + 5] = vertexIndex + terrainWidth + 2;

                triIndex += 6;
                vertexIndex++;
            }
            vertexIndex++;
        }
    }
    #endregion
}
