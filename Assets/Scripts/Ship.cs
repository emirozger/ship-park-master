using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class Ship : MonoBehaviour
{
    public Route route;
    public Transform bottomTransform;
    public Transform bodyTransform;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private ParticleSystem smokeFX;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float danceValue;
    [SerializeField] private float danceTime;
    [SerializeField] private float durationMultiplier;
    [SerializeField] private Ease shipEase = Ease.Linear;
    [SerializeField] private PathType pathType;
    [SerializeField] private PathMode pathMode;
    [SerializeField] private float lookAtFloat;


    private void Start()
    {
        //bodyTransform.DOLocalMoveY(danceValue,danceTime ).SetLoops(-1, LoopType.Yoyo).SetEase(shipEase);
        bodyTransform.DOLocalRotate(new Vector3(6, 0, 0), danceTime).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutCubic);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Ship otherShip))
        {
            StopDancingAnim();
            rb.DOKill(false);
            Vector3 hitPoint = other.contacts[0].point;
            AddExplosionForce(hitPoint);
            smokeFX.Play();
            GameManager.Instance.OnShipCollision?.Invoke();
        }
    }


    private void AddExplosionForce(Vector3 hitPoint)
    {
        rb.AddExplosionForce(400f, hitPoint, 3f);
        rb.AddForceAtPosition(Vector3.up * 2f, hitPoint, ForceMode.Impulse);
        rb.AddTorque(new Vector3(GetRandomAngle(), GetRandomAngle(), GetRandomAngle()));
    }

    private float GetRandomAngle()
    {
        float angle = 10f;
        float rand = Random.value;
        return rand > .5f ? angle : -angle;
    }
    public void Move(Vector3[] path)
    {
        rb.DOLocalPath(path, 3f * path.Length * durationMultiplier, pathType, pathMode)
            .SetLookAt(lookAtFloat, false)
            .SetEase(shipEase);
    }


    public void StopDancingAnim()
    {
        bodyTransform.DOKill(true);
    }

    public void SetColor(Color _color)
    {
        MeshRenderer[] childMeshRenderers = GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer childRenderer in childMeshRenderers)
        {
            foreach (var item in childRenderer.sharedMaterials)
            {
                item.color=_color;
            }
        }
    }
}
