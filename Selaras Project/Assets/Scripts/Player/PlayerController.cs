using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input Settings")]

    [Tooltip("Batas waktu minimal untuk player dikatakan sedang menahan mousenya")]
    [Range(0, 1)] [SerializeField] private float buttonHoldMin = 0.3f;

    [Space(10)] [Header("Player Movement and Controll")]
    
    [Tooltip("Kecepatan dasar dari player jika tidak dilakukan interaksi apapun oleh player")]
    [Range(1, 10)] [SerializeField] private float defaultSpeed = 3;

    [Tooltip("Pertambahan kecepatan setiap kali tap")]
    [Range(0.1f, 10)] [SerializeField] private float increasedSpeed = 1;

    [Tooltip("Batas Kecepatan Player")]
    [Range(5, 100)] [SerializeField] private float speedThreshold = 10;

    #region Private Variable

    private Rigidbody2D rb;

    //Kecepatan player saat ini
    //atur ini untuk mengkontrol kecepatan player
    private float currentSpeed;

    //Berapa lama player menekan mousenya
    private float buttonHoldTime;

    //sedang menekan mouse
    private bool isTapping;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        currentSpeed = defaultSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInput();

        SetVelocity();
    }


    /// <summary>
    /// Mengatur kecepatan player, dan mengembalikan kecepatan kembali ke normal
    /// jika tidak terjadi interaksi
    /// </summary>
    private void SetVelocity()
    {
        if(currentSpeed != defaultSpeed && !isTapping)
        {
            //mengembalikan kecepatan ke normal
            //saat kecepatan lebih kecil dari kecepatan normal maupun lebih besar dari kecepatan normal
            currentSpeed = currentSpeed < defaultSpeed ?
                           Mathf.Clamp(currentSpeed + Time.deltaTime * 2, 0, defaultSpeed) :
                           Mathf.Clamp(currentSpeed - Time.deltaTime * 2, defaultSpeed, speedThreshold);
        }


        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
    }

    /// <summary>
    /// mendapatkan input dari player lalu mengkontrol player berdasarkan input tersebut
    /// </summary>
    private void PlayerInput()
    {
        //cek jika player sedang menekan mouse
        if (Input.GetMouseButtonDown(0)) isTapping = true;

        //jika mouse dilepas
        //dan jika player tidak menahan mousenya, maka kecepatan akan bertambah
        if (Input.GetMouseButtonUp(0))
        {
            if(buttonHoldTime < buttonHoldMin) AddSpeed(); 

            //reset kembali isTapping dan waktu menekan mouse
            buttonHoldTime = 0;
            isTapping = false;
        }


        //jika player masih / sedang menekan mousenya
        if (isTapping)
        {
            //jika player menahan mousenya
            //maka kecepatan akan berkurang
            if (buttonHoldTime >= buttonHoldMin) Brake();

            buttonHoldTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Menambahkan kecepatan player selama 0 <= kecepatan player <= batas kecepatan
    /// </summary>
    private void AddSpeed()
    {
        currentSpeed = Mathf.Clamp(currentSpeed + increasedSpeed, defaultSpeed, speedThreshold);
    }

    /// <summary>
    /// Mengurangi kecepatan player sampai 0
    /// </summary>
    private void Brake()
    {
        currentSpeed = Mathf.Clamp(currentSpeed - (increasedSpeed * buttonHoldTime / 10), 0, speedThreshold);
    }
}
