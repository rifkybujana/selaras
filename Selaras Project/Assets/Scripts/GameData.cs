using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Random Object Generator/Generator Data")]
public class GameData : ScriptableObject
{
    [Header("Background")]
    public GameObject[] Grass;
    public GameObject[] Stones;
    public GameObject[] Trees;

    public GameObject[] PhotoSpot;
    public GameObject[] Background;

    [Header("Obstacle")]
    public GameObject[] StoneObstacles;
    public GameObject[] WaterfallObstacles;

    [System.Serializable]
    public class Boat
    {
        public string name;
        public int Index;
        public Sprite Model;
    }

    [System.Serializable]
    public class Char
    {
        public string name;
        public int Index;
        public Sprite Model;
        public GameObject Character;
    }

    [Header("Player Data")]
    public Boat[] Boats;
    public Char[] Character;

    public GameObject[] animals;

    public GameObject RandomizedGrass() => Grass[Random.Range(0, Grass.Length)];
    public GameObject RandomizedStones(bool obstacle = false) => obstacle ? StoneObstacles[Random.Range(0, Stones.Length)] : Stones[Random.Range(0, Stones.Length)];
    public GameObject RandomizedTrees() => Trees[Random.Range(0, Trees.Length)];
    public GameObject RandomizedWaterfall() => WaterfallObstacles[Random.Range(0, WaterfallObstacles.Length)];
    public GameObject RandomizedBackground() => PhotoSpot[Random.Range(0, PhotoSpot.Length)];

    public GameObject GetRandom(Transform parent)
    {
        int index = Random.Range(0, 4);
        int[] order = { -5, 1 };
        GameObject s;

        switch (index)
        {
            case 0:
                s = Instantiate(RandomizedGrass(), parent);
                s.GetComponentInChildren<SpriteRenderer>().sortingOrder = order[Random.Range(0, 2)];
                return s;

            case 1:
                s = Instantiate(RandomizedStones(), parent);
                s.GetComponentInChildren<SpriteRenderer>().sortingOrder = order[Random.Range(0, 2)];
                return s;

            case 2:
                s = Instantiate(RandomizedTrees(), parent);
                s.GetComponentInChildren<SpriteRenderer>().sortingOrder = order[Random.Range(0, 2)];
                return s;

            default:
                s = Instantiate(RandomizedGrass(), parent);
                s.GetComponentInChildren<SpriteRenderer>().sortingOrder = order[Random.Range(0, 2)];
                return s;
        }
    }
}
