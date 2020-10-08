using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SavedData
{
    public static int BestDistance;
    public static int BestSpeed;

    public static int CharIndex;
    public static int BoatIndex;

    public static void SetBestSpeed(int val)
    {
        if(val > BestSpeed)
        {
            BestSpeed = val;
        }
    }

    public static void SetBestDistance(int val)
    {
        if(val > BestDistance)
        {
            BestDistance = val;
        }
    }

    public static void SaveData(bool makeNewFile = false)
    {
        if (makeNewFile)
        {
            PlayerPrefs.SetInt("Char Index", 0);
            PlayerPrefs.SetInt("Boat Index", 0);
            PlayerPrefs.SetInt("Best Distance", 0);
            PlayerPrefs.SetInt("Best Speed", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Char Index", CharIndex);
            PlayerPrefs.SetInt("Boat Index", BoatIndex);
            PlayerPrefs.SetInt("Best Distance", BestDistance);
            PlayerPrefs.SetInt("Best Speed", BestSpeed);
        }
    }

    public static void GetData()
    {
        BestDistance = PlayerPrefs.GetInt("Best Distance");
        BestSpeed = PlayerPrefs.GetInt("Best Speed");
        CharIndex = PlayerPrefs.GetInt("Char Index");
        BoatIndex = PlayerPrefs.GetInt("Boat Index");
    }
}
