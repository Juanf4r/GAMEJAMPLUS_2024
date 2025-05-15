using Complete;
using UnityEngine;
using Core;

namespace _Scripts.Players.Deprecated
{
    public class AnimationEvents : MonoBehaviour
    {
        private Animator playerAnimator;
        private PlayerSoundManager playerSoundManager;
        private PlayerManager _playerManager;
        private static readonly int Golpe = Animator.StringToHash("Golpe");
        private static readonly int Eating = Animator.StringToHash("Eating");

        private void Awake()
        {
            playerAnimator = GetComponent<Animator>();
            playerSoundManager = GetComponentInParent<PlayerSoundManager>();
            _playerManager = GetComponentInParent<PlayerManager>();
        }

        public void PlaySoundPunch()
        {
            playerSoundManager.PlayPunch();
        }

        public void StopPunch()
        {
            playerAnimator.SetBool(Golpe, false);
        }

        public void PlayFootstep()
        {
            playerSoundManager.PlayFootStep();
        }

        public void StopEating()
        {
            playerAnimator.SetBool(Eating, false);
            _playerManager.canMove = true;
        }
    }
}
