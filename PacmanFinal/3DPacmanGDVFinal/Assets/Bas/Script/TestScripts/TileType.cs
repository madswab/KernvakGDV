using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileType {
    //Enum
    //enum walkableOrNot {Walk, Wall };

    public string name;
    public GameObject tileVisuelPrefabe;
    public bool isWalkable = true;

}
