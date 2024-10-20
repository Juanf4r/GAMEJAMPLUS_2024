using UnityEngine;
using DG.Tweening;

public class FloatingEffect : MonoBehaviour
{
    [SerializeField] private float upMovement = 0.25f;
    [SerializeField] private float duration = 3;
    
    private void Start()
    {
        transform.DOMoveY(transform.position.y + upMovement, duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}
