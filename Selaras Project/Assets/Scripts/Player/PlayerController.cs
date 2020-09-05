using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = new PlayerInput();

    [Space(10)] [Header("Player Movement and Controll")]
    
    [Tooltip("Kecepatan dasar dari player jika tidak dilakukan interaksi apapun oleh player")]
    [Range(1, 10)] [SerializeField] private float defaultSpeed = 3;

    [Tooltip("Kecepatan perubahan dari kecepatan player")]
    [Range(1, 10)] [SerializeField] private float accelleration;

    [Tooltip("Batas Kecepatan Player")]
    [Range(5, 100)] [SerializeField] private float maxSpeed = 20;

    #region Private Variable

    private Rigidbody2D rb;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }


    /// <summary>
    /// Mendapatkan Input dari player, dan mengendalikan player berdasarkan input
    /// </summary>
    private void GetInput()
    {
        //Jika player menekan mouse kiri
        if (Input.GetMouseButton(0))
        {
            if (playerInput.buttonHoldTime >= playerInput.buttonHoldMin)
            {
                float s = Mathf.Clamp(rb.velocity.x - (2 * accelleration * Time.deltaTime), 0, maxSpeed);

                rb.velocity = new Vector2(s, rb.velocity.y);
            }

            playerInput.buttonHoldTime += Time.deltaTime;
        }

        //jika player berhenti menekan mouse kiri
        if (Input.GetMouseButtonUp(0))
        {
            if(playerInput.buttonHoldTime < playerInput.buttonHoldMin)
            {
                float s = Mathf.Clamp(rb.velocity.x + (accelleration * Time.deltaTime), 0, maxSpeed);

                rb.velocity = new Vector2(s, rb.velocity.y);
            }

            playerInput.buttonHoldTime = 0;
        }
    }
}
