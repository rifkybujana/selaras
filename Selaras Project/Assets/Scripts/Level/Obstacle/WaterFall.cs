using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaterFall : MonoBehaviour
{
    public TMP_Text tutorial;

    BuoyancyEffector2D pGen;

    public float radius;

    public LayerMask playerLayer;

    private GameManager manager;

    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (manager.firstTimePlay)
        {
            tutorial.gameObject.SetActive(true);
        }
        else
        {
            Destroy(tutorial);
        }

        pGen = transform.parent.transform.parent.gameObject.GetComponent<BuoyancyEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, playerLayer)) 
        {
            GameObject player = Physics2D.OverlapCircle(transform.position, radius, playerLayer).gameObject;

            float jarak = player.transform.position.x - transform.position.x;

            pGen.flowMagnitude = radius * 2;
            pGen.flowAngle = jarak < 0 ? 180 : 0;
        }
        else
        {
            pGen.flowMagnitude = 5;
            pGen.flowAngle = 0;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
