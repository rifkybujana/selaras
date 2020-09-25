using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Rendering;

public class background : MonoBehaviour
{
    public GameObject[] bg;

    private Camera mainCamera;
    private Vector2 screenBounds;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = gameObject.GetComponent<Camera>();
        screenBounds = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCamera.transform.position.z));

        Debug.Log(screenBounds);

        foreach(GameObject o in bg)
        {
            LoadChildObject(o);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }


    void LoadChildObject(GameObject obj)
    {
        float objectWidth = obj.GetComponent<SpriteRenderer>().bounds.size.x;
        int childsNeeded = (int)Mathf.Abs(screenBounds.x / objectWidth);
        Debug.Log(childsNeeded);
        GameObject clone = Instantiate(obj) as GameObject;

        for(int i = 0; i <= childsNeeded; i++)
        {
            Debug.Log("work");
            GameObject c = Instantiate(clone) as GameObject;

            c.transform.SetParent(obj.transform);
            c.transform.position = new Vector3(objectWidth * i, obj.transform.position.y, obj.transform.position.z);
            c.name = obj.name + i;
        }

        Destroy(clone);
        Destroy(obj.GetComponent<SpriteRenderer>());
    }
}
