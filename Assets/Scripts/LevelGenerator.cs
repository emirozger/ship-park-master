using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public GameObject baseLevelPrefab;
    public GameObject base2ShipLevelPrefab;
    public int numberOfLevels = 5;
    public Vector2 objectXRange = new Vector2(-2.5f, 2f);
    public float objectYPosition = -2.917f;
    public Vector2 objectZRange = new Vector2(-2f, 8f);
    private GameObject[] generatedLevels;
    public List<GameObject> levels;
    public Transform[] ships;
    public Transform[] parks;
    
    private HashSet<Vector3> usedPositions;

  

    [ContextMenu(nameof(GenerateLevels))]
    public void GenerateLevels()
    {
        usedPositions = new HashSet<Vector3>();

        for (int i = 0; i < numberOfLevels; i++)
        {
            levels.Add(Instantiate(baseLevelPrefab, Vector3.zero,Quaternion.identity));
        }

        for (int i = 0; i < levels.Count; i++)
        {
            var ship = levels[i];
            ship.gameObject.SetActive(false);
            var shipTransform = ship.transform.GetChild(1).GetChild(0).GetChild(0);
            var parkTransform = ship.transform.GetChild(1).GetChild(0).GetChild(1);

            ships = ships.Concat(new Transform[] { shipTransform }).ToArray();
            parks = parks.Concat(new Transform[] { parkTransform }).ToArray();

            Vector3 shipPosition = GetUniqueRandomPlace();
            Vector3 parkPosition = GetUniqueRandomPlace();

            float distance = Vector3.Distance(shipPosition, parkPosition);

            while (distance < 2.0f)
            {
                shipPosition = GetUniqueRandomPlace();
                parkPosition = GetUniqueRandomPlace();
                distance = Vector3.Distance(shipPosition, parkPosition);
            }
            shipTransform.position = shipPosition;
            parkTransform.position = parkPosition;
        }
    }
    
    bool IsPositionOccupied(Vector3 position)
    {
        return usedPositions.Contains(position);
    }
    Vector3 GetUniqueRandomPlace()
    {
        Vector3 randomPosition;
        int maxAttempts = 10;

        for (int i = 0; i < maxAttempts; i++)
        {
            randomPosition = new Vector3(
                Random.Range(objectXRange.x, objectXRange.y),
                objectYPosition,
                Random.Range(objectZRange.x, objectZRange.y)
            );

            if (!IsPositionOccupied(randomPosition) && IsPositionValid(randomPosition))
            {
                usedPositions.Add(randomPosition);
                return randomPosition;
            }
        }

        return new Vector3(
            Random.Range(objectXRange.x, objectXRange.y),
            objectYPosition,
            Random.Range(objectZRange.x, objectZRange.y)
        );
    }

    bool IsPositionValid(Vector3 position)
    {
        foreach (var usedPosition in usedPositions)
        {
            float distance = Vector3.Distance(position, usedPosition);
            //Debug.Log(distance);
            if (distance < 5.0f)
            {
                return false;
            }
        }
        return true;
    }

  
}