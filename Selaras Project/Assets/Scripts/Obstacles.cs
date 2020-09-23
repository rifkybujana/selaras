using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "data", menuName = "Random Object Generator/Generator Data")]
public class Obstacles : ScriptableObject
{
    [Header("Background")]
    public GameObject[] Grass;
    public GameObject[] Stones;
    public GameObject[] Trees;

    public GameObject[] Background;

    [Header("Obstacle")]
    public GameObject[] StoneObstacles;
    public GameObject[] WaterfallObstacles;

    [Header("Player Data")]
    public Sprite[] Board;
    public GameObject[] Character;
    public GameObject[] animals;

    public GameObject RandomizedGrass() => Grass[Random.Range(0, Grass.Length)];
    public GameObject RandomizedStones(bool obstacle = false) => obstacle ? StoneObstacles[Random.Range(0, Stones.Length)] : Stones[Random.Range(0, Stones.Length)];
    public GameObject RandomizedTrees() => Trees[Random.Range(0, Trees.Length)];
    public GameObject RandomizedWaterfall() => WaterfallObstacles[Random.Range(0, WaterfallObstacles.Length)];
    public GameObject RandomizedBackground() => Background[Random.Range(0, Background.Length)];

    public GameObject GetRandom(Transform parent)
    {
        int index = Random.Range(0, 4);
        int[] order = { -2, 1 };
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
