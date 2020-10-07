using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharSelect : MonoBehaviour
{
    public GameData.Char character;

    public TMP_Text nameTxt;
    public Image img;

    private PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        img = GetComponent<Image>();

        nameTxt.text = character.name;
        img.sprite = character.Model;
    }

    public void ChangeCharacter()
    {
        player.anim[player.character.Index].gameObject.SetActive(false);
        player.anim[character.Index].gameObject.SetActive(true);
        player.character = character;
    }
}
