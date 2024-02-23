using DG.Tweening;
using UnityEngine;

public class AnchorManager : MonoBehaviour
{
    public int anchorValue;
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
            UIManager.Instance.CollectAnchor(anchorValue);
            Destroy(gameObject);
        }
    }
}