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
    public List<ProceduralGenerator> MeshObjects = new List<ProceduralGenerator>();

    // Start is called before the first frame update
    void Start()
    {
        lastPoint = Vector3.zero;
        PlaceObject();
    }

    /// <summary>
    /// Menempatkan Object Baru / memperpanjang level
    /// </summary>
    public void PlaceObject()
    {
        //Menempatkan object berdasarkan titik terakhir object terakhir
        MeshObjects.Add(Instantiate(MeshObject, lastPoint, Quaternion.identity, transform));

        //hilangkan object ketiga dari belakang
        if(MeshObjects.Count > 3)
        {
            Destroy(MeshObjects[0].gameObject);
            MeshObjects.RemoveAt(0);
        }
    }

    public void ResetLevel()
    {
        lastPoint = Vector3.zero;
        
        for(int i = 0; i < MeshObjects.Count; i++)
        {
            Destroy(MeshObjects[i].gameObject);
        }

        MeshObjects.Clear();

        PlaceObject();
    }
}
