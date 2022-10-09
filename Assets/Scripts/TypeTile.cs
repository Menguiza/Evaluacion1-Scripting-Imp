using UnityEngine;
using System.Collections;

[System.Serializable]
public class TypeTile 
{
    public string name;
    public GameObject tileVisualPrefab;

    public bool isWalkable = true;
    public float movementCost = 1;
}