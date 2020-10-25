using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Background : MonoBehaviour
{
    public bool isStartBackground;

    public int index;
    public GameData data;

    [SerializeField]
    private Transform SpawnPos = null;
    private Transform cam;

    private Renderer m_renderer;
    private GameManager manager;

    private float selisihJarak;

    [HideInInspector]
    public bool isPlaced;

    // Start is called before the first frame update
    void Start()
    {
        m_renderer = GetComponent<Renderer>();
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        cam = Camera.main.GetComponent<Transform>();

        selisihJarak = transform.position.y - cam.transform.position.y;

        isPlaced = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!manager.isStart) return;

        transform.position = new Vector3(transform.position.x, cam.transform.position.y + selisihJarak, transform.position.z);

        if (m_renderer.isVisible && !isPlaced)
        {
            Debug.Log("test");
            place();
        }

        if (isPlaced && !m_renderer.isVisible && !isStartBackground) Destroy(gameObject); 
    }

    public void place()
    {
        //place
        Instantiate(data.Background[index], SpawnPos.position, Quaternion.identity);
        isPlaced = true;

    }
}
