using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
{
    BuoyancyEffector2D pGen;

    public float radius;

    public LayerMask playerLayer;

    // Start is called before the first frame update
    void Start()
    {
        pGen = transform.parent.transform.parent.gameObject.GetComponent<BuoyancyEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics2D.OverlapCircle(transform.position, radius, playerLayer)) 
        {
            GameObject player = Physics2D.OverlapCircle(transform.position, radius, playerLayer).gameObject;

            float jarak = player.transform.position.x - transform.position.x;

            pGen.flowMagnitude = radius * 3;
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
