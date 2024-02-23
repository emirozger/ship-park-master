using System.Collections;
using UnityEngine;

public class NewShipController : MonoBehaviour
{
    public int shipID;
    public bool hasBuyedShip;
    public int shipCost;

    private void Start()
    {
        LoadHasBuyedShip();
    }

    public void LoadHasBuyedShip()
    {
        string hasBuyedKey = "HasBuyedShip_" + shipID;

        if (PlayerPrefs.HasKey(hasBuyedKey))
        {
            hasBuyedShip = PlayerPrefs.GetInt(hasBuyedKey) == 1;
        }
    }
}
