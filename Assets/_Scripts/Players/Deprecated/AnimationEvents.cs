using UnityEngine;

namespace _Scripts.Players.Deprecated
{
    public class AnimationEvents : MonoBehaviour
    {
        private Animator playerAnimator;
        private PlayerSoundManager playerSoundManager;
        private static readonly int Golpe = Animator.StringToHash("Golpe");

        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
            playerSoundManager = GetComponentInParent<PlayerSoundManager>();
        }
        public void StopPunch()
        {
            playerAnimator.SetBool(Golpe, false);
        }

        public void PlayFootstep()
        {
            playerSoundManager.PlayFootStep();
        }
    }
}
