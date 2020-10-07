using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatList : MonoBehaviour
{
    public GameData data;
    public GameObject boatSelect;

    private void Awake()
    {
        if (data.Boats.Length <= 0) return;

        foreach(GameData.Boat b in data.Boats)
        {
            BoatSelect bs = Instantiate(boatSelect, transform).GetComponent<BoatSelect>();
            bs.boat = b;
        }
    }
}
