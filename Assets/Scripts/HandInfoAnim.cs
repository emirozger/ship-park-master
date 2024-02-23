using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInfoAnim : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (LevelManager.Instance.currentLevelIndex > 0)
            Destroy(UIManager.Instance.handAnimObject);
    }

}
