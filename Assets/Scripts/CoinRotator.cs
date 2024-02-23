using DG.Tweening;
using UnityEngine;

public class CoinRotator : MonoBehaviour
{
    public int coinValue;
    private void Start()
    {
        transform.DORotate(new Vector3(0, 180, 0), 1f)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UIManager.Instance.CollectCoin(coinValue);
            Destroy(gameObject);
        }
    }
}