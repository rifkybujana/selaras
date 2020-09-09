using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFall : MonoBehaviour
{
    [SerializeField] private float radius;

    [SerializeField] private LayerMask playerLayerMask;

    [SerializeField] private float flowMultiplier = 1;

    private PlayerController player;

    private bool isOnRadius;

    // Update is called once per frame
    void Update()
    {
        if (!Physics2D.OverlapCircle(transform.position, radius, playerLayerMask))
        {
            if (isOnRadius)
            {
                isOnRadius = false;
            }

            return;
        }

        player = Physics2D.OverlapCircle(transform.position, radius, playerLayerMask).GetComponent<PlayerController>();

        if (player != null && player.isGrounded())
        {
            float flowCalculate = Map(player.transform.position.x - transform.position.x, -radius, radius, -1, 1);

            player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x - flow(flowCalculate), player.GetComponent<Rigidbody2D>().velocity.y);

            isOnRadius = true;
        }
    }

    private float flow(float f)
    {
        return f < 0 ? -(1 + f) : 1 - f;
    }

    private float Map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return Mathf.Clamp((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min, out_min, out_max);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
