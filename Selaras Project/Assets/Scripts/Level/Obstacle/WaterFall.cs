using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
{
    [SerializeField] private float radius;

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private float flowPower = 5;


    // Update is called once per frame
    void Update()
    {
        if (!Physics2D.OverlapCircle(transform.position, radius, playerLayerMask)) return;

        PlayerController p = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask).GetComponent<PlayerController>();

        if (p != null && p.isGrounded())
        {
            //y = n - x^2
            //y >= 0
            Debug.Log(Mathf.Clamp(flowPower - Mathf.Pow(p.transform.position.x - transform.position.x, 2), -flowPower, flowPower));
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
