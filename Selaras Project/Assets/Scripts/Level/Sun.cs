using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public GameManager manager;

    private Vector3 jarak;

    // Start is called before the first frame update
    void Start()
    {
        jarak = transform.position - Camera.main.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isStart) return;

        transform.position = Camera.main.transform.position + jarak;
    }
}
