using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipList : MonoBehaviour
{
    public static ShipList Instance;
    public GameObject[] levelShips;

    private void Awake()
    {
        Instance=this;
    }
}
