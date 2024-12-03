using UnityEngine;

namespace _Scripts.Players.Deprecated
{
    public class AnimationEvents : MonoBehaviour
    {
        [SerializeField] private Animator playerAnimator;
        private static readonly int Golpe = Animator.StringToHash("Golpe");

        public void StopPunch()
        {
            playerAnimator.SetBool(Golpe, false);
        }
    }
}
