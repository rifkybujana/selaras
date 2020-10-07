using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoatSelect : MonoBehaviour
{
    public GameData.Boat boat;

    public Image img;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        img = GetComponent<Image>();

        img.sprite = boat.Model;
        img.SetNativeSize();
    }

    public void ChangeBoat()
    {
        player.boats[player.boat.Index].SetActive(false);
        player.boats[boat.Index].SetActive(true);
        player.boat = boat;
    }
}
