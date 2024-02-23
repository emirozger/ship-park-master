using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public List<Route> readyRoutes = new();

    private int totalRoutes;
    private int successfulParks;
    

    public UnityAction<Route> OnShipEntersPark; 
    public UnityAction OnShipCollision;

   
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        totalRoutes = transform.GetComponentsInChildren<Route>().Length;
        successfulParks = 0;
        OnShipEntersPark += OnShipEntersParkHandler;
        OnShipCollision += OnShipCollisionHandler;

    }
    
    private void OnShipCollisionHandler()
    {
        UnityEngine.Debug.Log("GameOver");
        DOVirtual.DelayedCall(3f, () =>
        {
            LevelManager.Instance.RetryLevel();
        });
        
    }
    private void OnShipEntersParkHandler(Route route)
    {
        route.ship.StopDancingAnim();
        successfulParks++;
        if (successfulParks==totalRoutes)
        {
            LevelManager.Instance.currentLevelIndex++;
            Debug.Log("win");
            DOVirtual.DelayedCall(1f,() =>
            {
                LevelManager.Instance.levelParentTransform.gameObject.SetActive(false);
                UIManager.Instance.gameplayUI.SetActive(false);
                UIManager.Instance.winCanvas.SetActive(true);
            });
            DOVirtual.DelayedCall(3f, () =>
            {
                UIManager.Instance.winCanvas.SetActive(false);
                UIManager.Instance.gameplayUI.SetActive(true);
                LevelManager.Instance.levelParentTransform.gameObject.SetActive(true);
                LevelManager.Instance.SetActiveLevel(LevelManager.Instance.currentLevelIndex);
                
            });
        }
    }
    
    public void RegisterRoute(Route route)
    {
        readyRoutes.Add(route);
        if (readyRoutes.Count==totalRoutes)
        {
            MoveAllShips();
        }
    }

    private void MoveAllShips()
    {
        foreach (var route in readyRoutes)
        {
            route.ship.Move(route.linePoints);
        }
    }
}
