using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Face
{
    public List<Vector3> vertices { get; private set; }
    public List<int> triangles { get; private set; }
    public List<Vector2> uvs { get; private set; }
    public List<Vector2> points { get; private set; }

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uvs, List<Vector2> points)
    {
        this.vertices = vertices;
        this.triangles = triangles;
        this.uvs = uvs;
        this.points = points;
    }
}


[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(PolygonCollider2D))]
public class HexRenderer : MonoBehaviour
{

    private Mesh m_mesh;
    private MeshFilter m_meshFilter;
    private MeshRenderer m_meshRenderer;
    private PolygonCollider2D p_collider2D;

    private List<Face> m_faces;

    public Material material;

    [SerializeField] public bool isFlatTopped;
    [SerializeField] public float innerSize;
    [SerializeField] public float outerSize;
    [SerializeField] public Color hoverColor;
    [SerializeField] public Color startColor;

    private void Awake()
    {
        m_meshFilter = GetComponent<MeshFilter>();
        m_meshRenderer = GetComponent<MeshRenderer>();
        p_collider2D = GetComponent<PolygonCollider2D>();

        m_mesh = new Mesh();
        m_mesh.name = "Hex";

        m_meshFilter.mesh = m_mesh;
        m_meshRenderer.material = material;
    }

    private void Start()
    {
        startColor = m_meshRenderer.material.color;
    }

    public void OnMouseEnter()
    {
        hoverColor.a = 1f;
        m_meshRenderer.material.color = hoverColor;
        Debug.Log(gameObject.transform);
    }

    public void OnMouseExit()
    {
        m_meshRenderer.material.color = startColor;
    }

    public void SetMaterial(Material material)
    {
        m_meshRenderer.material = material;
    }

    private void OnEnable()
    {
        DrawMesh();
    }

    public void DrawMesh()
    {
        DrawFace();
        CombineFaces();
    }

    public void DrawFace()
    {
        m_faces = new List<Face>();

        for (int point = 0; point < 6; point++)
        {
            m_faces.Add(CreateFace(innerSize, outerSize, point));
        }
    }

    public void CombineFaces()
    {
        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < m_faces.Count; i++)
        {
            vertices.AddRange(m_faces[i].vertices);
            uvs.AddRange(m_faces[i].uvs);
            points.AddRange(m_faces[i].points);

            int offset = (4 * i);
            foreach (int triangle in m_faces[i].triangles)
            {
                triangles.Add(triangle + offset);
            }
        }

        p_collider2D.points = points.ToArray();
        m_mesh.vertices = vertices.ToArray();
        m_mesh.triangles = triangles.ToArray();
        m_mesh.uv = uvs.ToArray();
        m_mesh.RecalculateNormals();
    }

    private Face CreateFace(float innerRad, float outerRad, int point, bool reverse = false) 
    {
        Vector2 pointA = GetPoint(innerRad, point);
        Vector2 pointB = GetPoint(innerRad, (point < 5) ? point + 1 : 0);
        Vector2 pointC = GetPoint(outerRad, (point < 5) ? point + 1 : 0);
        Vector2 pointD = GetPoint(outerRad, point);

        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD};
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() { new Vector2(0, 0), new Vector2(1, 0), new Vector2(1, 1), new Vector2(0, 1) };
        List<Vector2> points = new List<Vector2>() { pointA, pointB, pointC, pointD };

        if (reverse)
        {
            vertices.Reverse();
        }

        return new Face(vertices, triangles, uvs, points);
    }

    protected Vector2 GetPoint(float size, int index)
    {
        float angle_deg = isFlatTopped ? 60 * index : 60 * index - 30;
        float angle_rad = Mathf.PI / 180f * angle_deg;
        return new Vector2(size * Mathf.Cos(angle_rad), size * Mathf.Sin(angle_rad));
    }

    public void OnValidate()
    {
        if (Application.isPlaying && m_mesh != null)
        {
            DrawMesh();
        }
    }
}
