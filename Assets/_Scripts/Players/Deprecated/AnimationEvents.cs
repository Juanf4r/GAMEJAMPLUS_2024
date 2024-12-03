using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    [SerializeField] private Animator playerAnimator;
    
    public void StopPunch()
    {
        playerAnimator.SetBool("Golpe", false);
    }
}
