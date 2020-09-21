using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    [Tooltip("Seberapa mulus kurvanya")]
    [SerializeField] private int resolution = 20;

    [Space(10)]
    [Header("Obstacle")]
    public GameObject WaterFall;
    public GameObject Stone;


    #region private/hidden variable

    Mesh mesh;
    LevelGenerator levelGenerator;

    //apakah object ini sudah menggenerasikan object baru
    bool isPlaced = false;

    public enum MeshType
    {
        Flat,
        StreamDown
    }

    [HideInInspector] public MeshType meshType = MeshType.StreamDown;

    public enum FlatType
    {
        WaterFall,
        PhotoSpot
    }

    [HideInInspector] public FlatType flatType = FlatType.WaterFall;

    //List of curve
    List<Vector3[]> curves = new List<Vector3[]>();

    //Vertices dan Tris dari mesh
    List<Vector3> vertices = new List<Vector3>();

    List<int> triangles = new List<int>();

    float xPos, yBefore;

    float flatHeight = -0.4f;

    #endregion


    // Start is called before the first frame update
    void Start()
    {
        //mendapatkan level generator komponen dari parent
        levelGenerator = transform.parent.GetComponent<LevelGenerator>();

        //setup mesh filter
        var filter = GetComponent<MeshFilter>();
        mesh = filter.mesh;
        mesh.Clear();

        //reset x dan y position dari vertices
        xPos = 0; yBefore = 0;

        //merandomize tipe mesh
        int RandomizeType = Random.Range(1, 3);

        //jika bukan di awal generasi dan hasil dari random itu = 2
        if (levelGenerator.MeshObjects.Count > 2 && RandomizeType == 2 && levelGenerator.MeshObjects[levelGenerator.MeshObjects.Count - 1].meshType != MeshType.Flat)
        {
            meshType = MeshType.Flat;

            Instantiate(Stone, transform);

            int RandomType = Random.Range(1, 3);

            Debug.Log(RandomType);

            if(RandomType == 1)
            {
                flatType = FlatType.PhotoSpot;
            }
            else
            {
                flatType = FlatType.WaterFall;
            }
        }
        else
        {
            meshType = MeshType.StreamDown;
        }

        //Buat Mesh
        for(int i = 0; i < 5; i++)
        {
            GeneratePoint();
        }

        GenerateMesh();

        GenerateObstacle();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //jika menyentuh player, buat generasi baru
        if(collision.gameObject.CompareTag("Player") && !isPlaced)
        {
            levelGenerator.PlaceObject();
            isPlaced = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //jika menyentuh player, buat generasi baru
        if (collision.gameObject.CompareTag("Player") && !isPlaced)
        {
            levelGenerator.PlaceObject();
            isPlaced = true;
        }
    }

    private void GenerateObstacle()
    {
        List<Vector2> v = GetTopVertices();

        if(meshType == MeshType.Flat )
        {
            if(flatType == FlatType.WaterFall)
            {
                int random = Random.Range(v.Count - (v.Count * 3 / 4), v.Count - (v.Count / 4));

                WaterFall = Instantiate(WaterFall, Vector3.zero, Quaternion.identity, transform);

                WaterFall.transform.localPosition = v[random];
                WaterFall.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            }
            else
            {
                //Spawn Photo Spot
            }
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
        levelGenerator.lastPoint += vertices[vertices.Count - 1] * transform.localScale.x;

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
                    curve[i] = new Vector3(xPos, flatHeight, 0);

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
        EdgeCollider2D col = gameObject.AddComponent<EdgeCollider2D>();
        BuoyancyEffector2D bEffector = gameObject.AddComponent<BuoyancyEffector2D>();
        List<Vector2> v = new List<Vector2>();

        switch (meshType)
        {
            case MeshType.StreamDown:
                BoxCollider2D bCol = gameObject.AddComponent<BoxCollider2D>();
                bCol.offset = new Vector2(2.25f, -1f);
                bCol.size = new Vector2(4.5f, 2f);
                bCol.isTrigger = true;
                bCol.usedByEffector = true;

                bEffector.flowMagnitude = 15;

                //mengambil semua titik vektor bagian atas dari array vertices
                v = GetTopVertices(41);
                v.Add(vertices[vertices.Count - 2]);

                //menempatkan sudut colliders sesuai dengan titik vektor dari array vertices
                col.points = v.ToArray();
                break;

            case MeshType.Flat:
                v = GetTopVertices();
                v.Add(vertices[vertices.Count - 2]);

                col.points = v.ToArray();
                col.isTrigger = true;
                col.usedByEffector = true;

                bEffector.surfaceLevel += flatHeight;
                bEffector.flowMagnitude = 10;
                break;
        }
    }

    /// <summary>
    /// Menetapkan titik titik vertices atas dan bawah
    /// Dan membuat segitiga mesh
    /// </summary>
    /// <param name="point">Titik atau point vertices</param>
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

    /// <summary>
    /// Mendapatkan Hanya Posisi Top Vertices Saja
    /// </summary>
    /// <returns></returns>
    List<Vector2> GetTopVertices(int s = 1)
    {
        //mengambil semua titik vektor bagian atas dari array vertices
        List<Vector2> p = new List<Vector2>();

        p.Add(new Vector2(vertices[s - 1].x, vertices[s - 1].y));
        for (int i = s; i < vertices.Count; i += 2)
        {
            p.Add(new Vector2(vertices[i].x, vertices[i].y - 0.1f));
        }

        return p;
    }
}
