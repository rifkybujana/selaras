using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharList : MonoBehaviour
{
    public GameData data;
    public GameObject charSelect;

    private void Awake()
    {
        if (data.Character.Length <= 0) return;

        foreach(GameData.Char c in data.Character)
        {
            CharSelect cs = Instantiate(charSelect, transform).GetComponent<CharSelect>();
            cs.character = c;
        }
    }
}
