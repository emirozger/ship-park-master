using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public GameObject shipListParent;
    public ShipList[] shipLists;
    private Route activeRoute;
    private bool isAvaiableLineUIActive = false;

    [Header("Values")]
    [Space]
    [SerializeField] private float fadeDuration;
    public int coinCount;
    public int anchorCount;
    public int PlayerPrefsCurrentCoin;
    public int PlayerPrefsCurrentAnchor;
    public int selectedShipIndex;
    [Header("Texts")]
    [Space]
    public TextMeshProUGUI havecoinText;
    public TextMeshProUGUI haveAnchorText;
    public TextMeshProUGUI levelIDText;
    [Header("UI GameObjects")]
    [Space]
    public GameObject handAnimObject;
    public GameObject portObject;
    public GameObject joystickObject;
    public GameObject levelObject;
    public GameObject watchADObject;
    public GameObject portPanel;
    public GameObject portOpenBackButton;
    public GameObject gameplayUI;
    public GameObject winCanvas;
    public GameObject notEnoughCostPanel;
    [SerializeField] private Image fadePanel;
    [SerializeField] private Image avaiableLineFill;
    [SerializeField] private GameObject avaiableLineHolder;
    [SerializeField] private CanvasGroup avaiableLineCanvasGroup;
    
    public Image timerFillImage;
    public TextMeshProUGUI shipCostText;
    [Space] [SerializeField] private LinesDrawer linesDrawer;
    public GameObject portShip;
    public bool portOpened;

    private void Awake()
    {
        Instance = this;
        Application.targetFrameRate = 9999;
    }


    private void Start()
    {
        shipLists = shipListParent.GetComponentsInChildren<ShipList>(true);
        levelIDText.SetText("Level " + (LevelManager.Instance.currentLevelIndex + 1).ToString());
        PlayerPrefsCurrentCoin = PlayerPrefs.GetInt("HaveCoin");
        PlayerPrefsCurrentAnchor = PlayerPrefs.GetInt("HaveAnchor");
        anchorCount = PlayerPrefsCurrentAnchor;
        coinCount = PlayerPrefsCurrentCoin;
        haveAnchorText.SetText(PlayerPrefsCurrentAnchor.ToString());
        havecoinText.SetText(PlayerPrefsCurrentCoin.ToString());
        fadePanel.DOFade(0f, fadeDuration).From(1f);
        avaiableLineCanvasGroup.alpha = 0f;
        linesDrawer.OnBeginDraw += OnBeginDrawHandler;
        linesDrawer.OnDraw += OnDrawHandler;
        linesDrawer.OnEndDraw += OnEndDrawHandler;
        haveAnchorText.SetText(anchorCount.ToString());
        if (LevelManager.Instance.currentLevelIndex >= 2)
        {
            print("loaded ship");
            LoadLevelSelectedShip();
            DisableDefaultLevelShip();
        }

    }
    public void ClearData()
    {
        PlayerPrefs.DeleteAll();
    }
    public void DisableDefaultLevelShip()
    {
        if (selectedShipIndex == 0)
            return;
 
        if (shipLists != null)
        {
            foreach (var item in shipLists)
            {
                item.levelShips[0].SetActive(false);
            }
        }
    }
    public void LoadLevelSelectedShip()
    {
        if (PlayerPrefs.HasKey("SelectedShipIndex"))
        {
            selectedShipIndex = PlayerPrefs.GetInt("SelectedShipIndex");
            if (shipLists != null)
            {
                foreach (var item in shipLists)
                {
                    item.levelShips[selectedShipIndex].SetActive(true);
                }
            }
        }
    }

    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;
        Debug.Log(activeRoute.gameObject.name);
        avaiableLineFill.color = activeRoute.shipColor;
        avaiableLineFill.fillAmount = 1f;
        avaiableLineCanvasGroup.DOFade(1f, .3f).From(0f);
        isAvaiableLineUIActive = true;
    }

    private void OnDrawHandler()
    {
        if (isAvaiableLineUIActive)
        {
            float maxLineLength = activeRoute.maxLineLength;
            float lineLength = activeRoute.line.length;

            avaiableLineFill.fillAmount = 1 - (lineLength / maxLineLength);
        }
    }

    private void OnEndDrawHandler()
    {
        if (isAvaiableLineUIActive)
        {
            isAvaiableLineUIActive = false;
            activeRoute = null;
            avaiableLineCanvasGroup.DOFade(0f, .3f).From(1f);
        }
    }

    public void CollectCoin(int coinValue)
    {
        coinCount += coinValue;
        PlayerPrefs.SetInt("HaveCoin", coinCount);
        PlayerPrefs.Save();
        havecoinText.SetText(coinCount.ToString());
    }
    public void BuyShipWithCoin(int coinValue)
    {
        coinCount -= coinValue;
        PlayerPrefs.SetInt("HaveCoin", coinCount);
        PlayerPrefs.Save();
        havecoinText.SetText(coinCount.ToString());
    }

    public void CollectAnchor(int anchorValue)
    {
        anchorCount += anchorValue;
        PlayerPrefs.SetInt("HaveAnchor", anchorCount);
        PlayerPrefs.Save();
        haveAnchorText.SetText(anchorCount.ToString());
    }

    public void BuyShipWithAnchor(int value)
    {
        anchorCount -= value;
        PlayerPrefs.SetInt("HaveAnchor", anchorCount);
        PlayerPrefs.Save();
        haveAnchorText.SetText(anchorCount.ToString());
    }

    public void OpenAndClosePort()
    {
        if (!portOpened)
        {
            portOpened = true;
            portOpenBackButton.SetActive(true);
            portObject.SetActive(true);
            joystickObject.SetActive(true);
            levelObject.SetActive(false);
            levelIDText.gameObject.SetActive(false);
            watchADObject.SetActive(false);
            portPanel.SetActive(false);
            gameplayUI.SetActive(true);
        }
        else
        {
            portOpened = false;
            portOpenBackButton.SetActive(true);
            portObject.SetActive(false);
            joystickObject.SetActive(false);
            levelObject.SetActive(true);
            levelIDText.gameObject.SetActive(true);
            watchADObject.SetActive(true);
        }
    }

}