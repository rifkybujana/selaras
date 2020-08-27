﻿using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput = new PlayerInput();

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
        GetInput();

        SetVelocity();
    }


    /// <summary>
    /// Mendapatkan Input dari player
    /// Simultaneous Tap = Tambah Kecepatan
    /// Hold Mouse button = Rem
    /// </summary>
    private void GetInput()
    {
        //Jika player menekan mouse kiri
        if (Input.GetMouseButton(0))
        {
            //jika player menekannya dalam waktu yang cukup lama maka rem
            //waktu meneakan > waktu minimal untuk dikatakan sedang ditahan
            if (playerInput.buttonHoldTime >= playerInput.buttonHoldMin) Brake();

            playerInput.buttonHoldTime += Time.deltaTime;
        }

        //jika player berhenti menekan mouse kiri
        if (Input.GetMouseButtonUp(0))
        {
            //Jika player menekannya hanya sebentar maka tambah kecepatan
            //waktu menekan < waktu minimal untuk dikatakan sedang ditahan
            if(playerInput.buttonHoldTime < playerInput.buttonHoldMin) AddSpeed();

            playerInput.buttonHoldTime = 0;
        }
    }

    /// <summary>
    /// Mengatur kecepatan player, dan mengembalikan kecepatan kembali ke normal
    /// jika tidak terjadi interaksi
    /// </summary>
    private void SetVelocity()
    {
        //Mengembalikan kecepatan player ke normal / kecepatan default
        //jika sedang tidak ada interaksi dan juga kecepatannya tidak sama dengan kecepatan normal
        if(currentSpeed != defaultSpeed && playerInput.buttonHoldTime == 0)
        {
            currentSpeed = currentSpeed < defaultSpeed ?
                           Mathf.Clamp(currentSpeed + Time.deltaTime * 2, 0, defaultSpeed) :
                           Mathf.Clamp(currentSpeed - Time.deltaTime * 2, defaultSpeed, speedThreshold);
        }

        //set kecepatan player
        rb.velocity = new Vector2(currentSpeed, rb.velocity.y);
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
        currentSpeed = Mathf.Clamp(currentSpeed - (increasedSpeed * playerInput.buttonHoldTime / 10), 0, speedThreshold);
    }
}
