using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    [Tooltip("Seberapa mulus kurvanya")]
    [SerializeField] private int resolution = 20;


    Mesh mesh;

    //apakah object ini sudah menggenerasikan object baru
    bool isPlaced = false;

    //Jenis Procedural Mesh
    public enum MeshType
    {
        Flat,
        StreamDown
    }

    [HideInInspector] public MeshType meshType = MeshType.StreamDown;

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

        int RandomizeType = Random.Range(1, 5);
        if (transform.parent.GetComponent<LevelGenerator>().MeshObjects.Count > 2 && RandomizeType > 3)
        {
            meshType = MeshType.Flat;
        }
        else
        {
            meshType = MeshType.StreamDown;
        }

        for(int i = 0; i < 5; i++)
        {
            GeneratePoint();
        }

        GenerateMesh();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //jika menyentuh player, buat generasi baru
        if(collision.gameObject.CompareTag("Player") && !isPlaced)
        {
            transform.parent.GetComponent<LevelGenerator>().PlaceObject();
            isPlaced = true;
        }
    }

    /// <summary>
    /// Membuat Mesh Berdasarkan Point
    /// </summary>
    private void GenerateMesh()
    {
        //hubungkan setiap titik kurva menggunakan bezier curve
        foreach (var curve in curves)
        {
            for (int i = 0; i < resolution; i++)
            {
                float t = (float)i / (float)(resolution - 1);
                Vector3 p = CalculateBezierPoint(t, curve[0], curve[1], curve[2], curve[3]);
                AddTerrainPoint(p);
            }
        }

        //membuat mesh
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        //set titik terakhir dari level generator menjadi vertices terakhir Procedural Generator ini
        LevelGenerator levelGen = transform.parent.GetComponent<LevelGenerator>();
        levelGen.lastPoint += vertices[vertices.Count - 1] * transform.localScale.x;

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
            //titik sebelumnya
            Vector3[] prev = null;

            //jika bukan di awal generasi set curva sebelumnya
            if (curves.Count > 0)
            {
                prev = curves[curves.Count - 1];
            }

            switch (meshType)
            {
                //jika tipe meshnya flat
                case MeshType.Flat:

                    //untuk kurva pertama, buat mendatar
                    curve[i] = new Vector3(xPos, -0.5f, 0);

                    break;

                //jika tipe meshnya menurun kebawah
                case MeshType.StreamDown:

                    //jika ada kurva sebelumnya
                    if (prev != null)
                    {
                        switch (i)
                        {
                            case 0:
                                //memposisikan titik pertama sama dengan titik terakhir kurva sebelumnya
                                curve[i] = prev[curve.Length - 1];
                                break;

                            case 1:
                                //membuat kurva kedua menyesuaikan kruva pertama dan sebelumnya
                                curve[i] = 2 * prev[curve.Length - 1] - prev[curve.Length - 2];
                                break;

                            default:
                                //jika angka ganjil
                                if (i % 2 != 0)
                                {
                                    //posisikan tinggi titik ini sama seperti titik sebelumnya
                                    curve[i] = new Vector3(xPos, yBefore, 0f);
                                }
                                //jika angka genap
                                else
                                {
                                    //membuat jalur turun atau naik secara random
                                    float yPos = prev[curve.Length - 1].y - (Random.Range(0f, 1f) * transform.localScale.y);
                                    yBefore = yPos;

                                    curve[i] = new Vector3(xPos, yPos, 0f);
                                }
                                break;
                        }
                    }
                    else
                    {
                        //untuk kurva pertama, buat mendatar
                        curve[i] = new Vector3(xPos, 0, 0);
                    }

                    break;
            }

            //posisi setiap titik menambah setengah
            xPos += 0.5f * transform.localScale.x;
        }

        curves.Add(curve);
    }


    /// <summary>
    /// Membuat Collider
    /// </summary>
    private void AddCollider()
    {
        //menambahkan komponen edge collider ke gameObject
        EdgeCollider2D col = gameObject.AddComponent<EdgeCollider2D>();

        //mengambil semua titik vektor bagian atas dari array vertices
        List<Vector2> p = new List<Vector2>();

        for(int i = 1; i < vertices.Count; i += 2)
        {
            p.Add(vertices[i]);
        }

        p.Add(vertices[vertices.Count - 2]);

        //menempatkan sudut colliders sesuai dengan titik vektor dari array vertices
        col.points = p.ToArray();
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
