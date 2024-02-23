using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelData
{
    public GameObject levelPrefab;
    public Transform startPosition;
}
public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public List<LevelData> levelsData = new List<LevelData>();
    public int currentLevelIndex = 0;
    public GameObject currentLevel;
    public Transform levelParentTransform;


    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        
        for (int i = 0; i < levelsData.Count; i++)
        {
            GameObject level = Instantiate(levelsData[i].levelPrefab);
            level.name = "Level " + (i + 1);
            level.SetActive(false);
            levelsData[i].startPosition = level.transform; 
            levelsData[i].levelPrefab = level;
            levelsData[i].startPosition.parent = levelParentTransform; 
        }

        int savedLevelIndex = PlayerPrefs.GetInt("ActiveLevelIndex", currentLevelIndex);

        currentLevelIndex = savedLevelIndex;
        SetActiveLevel(currentLevelIndex);

    }

    public void SetActiveLevel(int levelIndex)
    {
        for (int i = 0; i < levelsData.Count; i++)
        {
            levelsData[i].levelPrefab.SetActive(i == levelIndex);
        }

        currentLevel = levelsData[levelIndex].levelPrefab;
        currentLevelIndex = levelIndex;

        PlayerPrefs.SetInt("ActiveLevelIndex", currentLevelIndex);
        PlayerPrefs.Save();
        UIManager.Instance.levelIDText.SetText("Level " + (currentLevelIndex + 1));
        if (currentLevelIndex == 2)
        {
            UIManager.Instance.portPanel.SetActive(true);
            levelParentTransform.gameObject.SetActive(false);
            UIManager.Instance.gameplayUI.SetActive(false);
        }

        if (currentLevelIndex > 2)
        {
            UIManager.Instance.portOpenBackButton.SetActive(true);
        }
    }

    public void SetNextLevel()
    {
        if (currentLevelIndex < levelsData.Count - 1)
        {
            currentLevelIndex++;
            SetActiveLevel(currentLevelIndex);
        }
    }

    public void SetPreviousLevel()
    {
        if (currentLevelIndex > 0)
        {
            currentLevelIndex--;
            SetActiveLevel(currentLevelIndex);
        }
    }

    public void RetryLevel()
    {
        currentLevel.transform.position = levelsData[currentLevelIndex].startPosition.position;
        currentLevel.transform.rotation = levelsData[currentLevelIndex].startPosition.rotation;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        SetActiveLevel(currentLevelIndex);
    }
}
