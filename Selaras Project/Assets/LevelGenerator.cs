using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Tooltip("Mesh Generator Prefab")]
    [SerializeField] private ProceduralGenerator MeshObject = null;

    //titik terakhir sebagai acuan untuk menempatkan object
    public Vector3 lastPoint;

    //list semua mesh object
    [HideInInspector] public List<ProceduralGenerator> MeshObjects = new List<ProceduralGenerator>();

    // Start is called before the first frame update
    void Start()
    {
        lastPoint = Vector3.zero;
        InvokeRepeating("PlaceObject", 0.5f, 1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Menempatkan Object Baru / memperpanjang level
    /// </summary>
    private void PlaceObject()
    {
        //Menempatkan object berdasarkan titik terakhir object terakhir
        MeshObjects.Add(Instantiate(MeshObject, lastPoint, Quaternion.identity, transform));

        if(MeshObjects.Count > 2)
        {
            Destroy(MeshObjects[0].gameObject);
            MeshObjects.RemoveAt(0);
        }
    }
}
