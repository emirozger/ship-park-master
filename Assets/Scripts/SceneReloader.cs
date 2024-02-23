using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
  public void ReloadScene()
  {
    int currentLevel = SceneManager.GetActiveScene().buildIndex;
    SceneManager.LoadScene(currentLevel);
  }
}
