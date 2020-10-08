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

        img.sprite = boat.Model;
        img.SetNativeSize();
        img.rectTransform.sizeDelta *= 0.5f;
    }

    public void ChangeBoat()
    {
        player.boats[player.boat.Index].SetActive(false);
        player.boats[boat.Index].SetActive(true);
        player.boat = boat;

        SavedData.BoatIndex = boat.Index;
        SavedData.SaveData();
    }
}
