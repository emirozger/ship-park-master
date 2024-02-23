using UnityEngine;
using DG.Tweening;

public class ShipRotator : MonoBehaviour
{
    public GameObject[] previewPortShips;
    [SerializeField] private float rotateSpeed;

    private void Start()
    {
        foreach (var ship in previewPortShips)
        {
            ship.transform.DOMoveY(ship.transform.position.y + 0.35f, 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

        }
        
    }
    private void Update()
    {
        foreach (var ship in previewPortShips)
        {
            ship.transform.Rotate(0f, 0f, Time.deltaTime * rotateSpeed);
        }
    }
}
