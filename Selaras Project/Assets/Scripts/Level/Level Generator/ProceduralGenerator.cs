using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    [Tooltip("Seberapa mulus kurvanya")]
    [SerializeField] private int resolution = 20;

    [Tooltip("Player Layer Mask")]
    [SerializeField] private LayerMask playerLayerMask;

    Mesh mesh;

    //List of curve
    List<Vector3[]> curves = new List<Vector3[]>();

    //Vertices dan Tris dari mesh
    List<Vector3> vertices = new List<Vector3>();
    List<int> triangles = new List<int>();

    float xPos, yBefore;

    // Start is called before the first frame update
    void Start()
    {
        var filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;
        mesh.Clear();

        xPos = 0; yBefore = 0;

        for(int i = 0; i < 4; i++)
        {
            //Buat mesh permulaan
            GeneratePoint();
        }
        GenerateMesh();
    }

    /// <summary>
    /// Membuat Mesh Berdasarkan Point
    /// </summary>
    private void GenerateMesh()
    {
        // Same drawing code as before but now in a loop
        foreach (var curve in curves)
        {
            for (int i = 0; i < resolution; i++)
            {
                float t = (float)i / (float)(resolution - 1);
                Vector3 p = CalculateBezierPoint(t, curve[0], curve[1], curve[2], curve[3]);
                AddTerrainPoint(p);
            }
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        AddCollider();
    }

    /// <summary>
    /// Membuat Point atau vertices
    /// </summary>
    private void GeneratePoint()
    {
        //satu quad memiliki 4 titik
        var curve = new Vector3[4];

        for (int i = 0; i < curve.Length; i++)
        {
            //curve sebelumnya
            Vector3[] prev = null;

            //jika bukan di awal generasi set curva sebelumnya
            if (curves.Count > 0)
            {
                prev = curves[curves.Count - 1];
            }

            if (prev != null && i == 0) //jika ini bukan quad pertama, dan ini adalah titik curva awal
            {
                // buat kurva baru dan set posisi ke kurva sebelumnya agar nyambung
                curve[i] = prev[curve.Length - 1];
            }
            else if (prev != null && i == 1) //jika ini bukan quad pertama, dan ini adalah titik kurva ke 2
            {
                // buat kurva akhir
                curve[i] = 2 * prev[curve.Length - 1] - prev[curve.Length - 2];
            }
            else if (prev != null)
            {
                if (i % 2 != 0)
                {
                    curve[i] = new Vector3(xPos, yBefore, 0f);
                    Instantiate(new GameObject(), curve[i], Quaternion.identity, transform);
                }
                else
                {
                    float yPos = prev[curve.Length - 1].y - (Random.Range(0f, 0.5f) * transform.localScale.y);
                    yBefore = yPos;

                    curve[i] = new Vector3(xPos, yPos, 0f);

                    //Debug.Log(i + " " + yBefore);
                }

                //Debug.Log(curve[i]);
            }

            xPos += 0.5f * transform.localScale.x;
        }

        curves.Add(curve);
    }


    /// <summary>
    /// Membuat Colli
    /// </summary>
    private void AddCollider()
    {
        EdgeCollider2D col = gameObject.AddComponent<EdgeCollider2D>();

        List<Vector2> p = new List<Vector2>();

        for(int i = 1; i < vertices.Count; i += 2)
        {
            p.Add(new Vector2(vertices[i].x, vertices[i].y));
        }

        col.points = p.ToArray();

        col.isTrigger = true;
        col.usedByEffector = true;

        BuoyancyEffector2D buoyancy = gameObject.AddComponent<BuoyancyEffector2D>();
    }

    private void AddTerrainPoint(Vector3 point)
    {
        //Membuat bottom vertices yang sejajar dengan top vertices
        vertices.Add(new Vector3(point.x, point.y - transform.localScale.y, 0f)); 
        
        //membuat top vertices yang sejajar dengan bottom vertices
        vertices.Add(point);

        if(vertices.Count >= 4)
        {
            //menyelesaikan mesh quad baru dan membuat 2 segitiga
            int start = vertices.Count - 4;

            triangles.Add(start + 0);
            triangles.Add(start + 1);
            triangles.Add(start + 2);
            triangles.Add(start + 1);
            triangles.Add(start + 3);
            triangles.Add(start + 2);
        }
    }

    //bezier curve go brrrr
    Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;

        return p;
    }
}
