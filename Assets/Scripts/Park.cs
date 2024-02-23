using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Park : MonoBehaviour
{
    public Route route;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem fx;
    private ParticleSystem.MainModule fxMainModule;

    public bool hasShipParked = false;

    private float timer = 0f;
    private void Start()
    {
        fxMainModule = fx.main;
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Ship ship))
        {
            timer += Time.deltaTime;

            if (ship.route == route && timer >= .35f && !hasShipParked)
            {
                StartFX();
                DestroyHand();
                hasShipParked = true;
                timer = 0f;
                GameManager.Instance.OnShipEntersPark?.Invoke(route);
            }
        }
    }
    private void DestroyHand()
    {
        Destroy(UIManager.Instance.handAnimObject);
    }
    private void StartFX()
    {
        fxMainModule.startColor = route.shipColor;
        fx.loop = true;
        fx.Play();
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
