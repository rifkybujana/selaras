using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private Vector3 jarak;

    // Start is called before the first frame update
    void Start()
    {
        jarak = transform.position - Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Camera.main.transform.position + jarak;
    }
}
