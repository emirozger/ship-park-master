using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shaker : MonoBehaviour
{
    public float duration;
    public float randomness;
    public int vibrato;
    public Vector3 strength;
    
    private void Start()=> this.transform.DOShakePosition(duration, strength, vibrato, randomness).SetLoops(-1,LoopType.Yoyo);
    
}
