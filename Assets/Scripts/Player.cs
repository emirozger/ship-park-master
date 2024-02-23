using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : MonoBehaviour
{
    public static Player Instance;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FixedJoystick joystick;
    private Vector3 facingDirection;
    private float timer = 2;
    public GameObject[] portShips;
    public bool hasActivated;
    private float moveX, moveZ;
    private Vector3 movement;
    private bool isStayBuyingArea;

    public delegate void ShipActivationHandler(int shipIndex);
    public static event ShipActivationHandler OnShipActivated;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        LoadSelectedShip();
        DisableDefaultShip();
    }

    public void DisableDefaultShip()
    {
        if (UIManager.Instance.selectedShipIndex == 0 || portShips.Length == 0)
        {
            return;
        }

        portShips[0].SetActive(false);

        if (UIManager.Instance.shipLists != null)
        {
            foreach (var item in UIManager.Instance.shipLists)
            {
                item.levelShips[0].SetActive(false);
            }
        }
    }

    private void Update()
    {
        moveX = joystick.Horizontal;
        moveZ = joystick.Vertical;
        if (movement != Vector3.zero)
        {
            facingDirection = movement.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(facingDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void FixedUpdate()
    {
        movement = new Vector3(moveX, 0f, moveZ);
        rb.velocity = movement * moveSpeed * Time.fixedDeltaTime;
    }

    private void SetTimerFillAmount(Collider otherObject)
    {
        float fillAmount = timer / 2f;

        UIManager.Instance.timerFillImage.gameObject.SetActive(true);
        UIManager.Instance.shipCostText.gameObject.SetActive(true);

        var newShipID = otherObject.gameObject.GetComponent<NewShipController>().shipID;
        var newShipCost = otherObject.gameObject.GetComponent<NewShipController>().shipCost;
        var color = this.gameObject.transform.GetChild(newShipID).GetComponentInChildren<MeshRenderer>().sharedMaterial.color;

        UIManager.Instance.timerFillImage.color = color;
        UIManager.Instance.timerFillImage.transform.position = otherObject.gameObject.transform.position;

        UIManager.Instance.shipCostText.transform.position = new Vector3(otherObject.transform.position.x, otherObject.transform.position.y, otherObject.transform.position.z + 2f);
        UIManager.Instance.shipCostText.SetText(newShipCost.ToString() + " Coin");
        UIManager.Instance.timerFillImage.fillAmount = fillAmount;
    }

    private void SetFillAmountExit(Collider otherObject)
    {
        UIManager.Instance.timerFillImage.gameObject.SetActive(false);
        UIManager.Instance.timerFillImage.fillAmount = 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("NewShip") && !isStayBuyingArea)
        {
            NewShipController shipController = other.gameObject.GetComponent<NewShipController>();

            if (!hasActivated)
            {
                
                timer -= Time.deltaTime;
                SetTimerFillAmount(other);

                if (timer <= 0)
                {
                    if (!shipController.hasBuyedShip)
                    {
                        ActivateShip(shipController);
                    }
                    else
                    {
                        ActivateExistingShip(shipController);
                    }
                    isStayBuyingArea = true;
                }
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NewShip"))
        {
            timer = 2f;
            isStayBuyingArea = false;
            SetFillAmountExit(other);
        }
    }

    private void ActivateShip(NewShipController shipController)
    {
        if (UIManager.Instance.coinCount >= shipController.shipCost)
        {
            UIManager.Instance.BuyShipWithCoin(shipController.shipCost);
            shipController.hasBuyedShip = true;
            PlayerPrefs.SetInt("HasBuyedShip_" + shipController.shipID, 1);
            PlayerPrefs.Save();

            ActivateSelectedShip(shipController.shipID);
            SaveSelectedShip();
            StartCoroutine(HasActivatedAndTime());
        }
        else
        {
            HandleNotEnoughCoins();
        }
    }

    private void ActivateExistingShip(NewShipController shipController)
    {
        ActivateSelectedShip(shipController.shipID);
        SaveSelectedShip();
        StartCoroutine(HasActivatedAndTime());
    }

    private void ActivateSelectedShip(int shipIndex)
    {
        UIManager.Instance.selectedShipIndex = shipIndex;
        for (int i = 0; i < portShips.Length; i++)
        {
            portShips[i].SetActive(false);
            if (UIManager.Instance.shipLists != null)
            {
                foreach (var item in UIManager.Instance.shipLists)
                {
                    item.levelShips[i].SetActive(false);
                }
            }
        }
        portShips[shipIndex].SetActive(true);

        if (UIManager.Instance.shipLists != null)
        {
            foreach (var item in UIManager.Instance.shipLists)
            {
                item.levelShips[shipIndex].SetActive(true);
            }
        }

        OnShipActivated?.Invoke(shipIndex);
    }

    private void HandleNotEnoughCoins()
    {

        Debug.Log("Not enough coins to buy the ship!");

        if (UIManager.Instance.notEnoughCostPanel != null)
        {
            UIManager.Instance.notEnoughCostPanel.SetActive(true);
            StartCoroutine(DeactivateNotEnoughCostPanel());
        }

        for (int i = 0; i < portShips.Length; i++)
        {
            portShips[i].SetActive(false);
            if (UIManager.Instance.shipLists != null)
            {
                foreach (var item in UIManager.Instance.shipLists)
                {
                    item.levelShips[i].SetActive(false);
                }
            }
        }
        portShips[UIManager.Instance.selectedShipIndex].SetActive(true);

        if (UIManager.Instance.shipLists != null)
        {
            foreach (var item in UIManager.Instance.shipLists)
            {
                item.levelShips[UIManager.Instance.selectedShipIndex].SetActive(true);
            }
        }
        SaveSelectedShip();
        StartCoroutine(HasActivatedAndTime());
    }

    IEnumerator DeactivateNotEnoughCostPanel()
    {
        yield return new WaitForSeconds(2f);

        // Deactivate the notEnoughCostPanel
        if (UIManager.Instance.notEnoughCostPanel != null)
        {
            UIManager.Instance.notEnoughCostPanel.SetActive(false);
        }
    }

    IEnumerator HasActivatedAndTime()
    {
        hasActivated = true;
        timer = 2;
        yield return new WaitForSeconds(1f);
        hasActivated = false;
    }

    private void SaveSelectedShip()
    {
        PlayerPrefs.SetInt("SelectedShipIndex", UIManager.Instance.selectedShipIndex);
        PlayerPrefs.Save();
    }

    public void LoadSelectedShip()
    {
        if (PlayerPrefs.HasKey("SelectedShipIndex"))
        {
            UIManager.Instance.selectedShipIndex = PlayerPrefs.GetInt("SelectedShipIndex");
            portShips[UIManager.Instance.selectedShipIndex].SetActive(true);

            if (UIManager.Instance.shipLists != null)
            {
                foreach (var item in UIManager.Instance.shipLists)
                {
                    item.levelShips[UIManager.Instance.selectedShipIndex].SetActive(true);
                }
            }
        }
    }
}
