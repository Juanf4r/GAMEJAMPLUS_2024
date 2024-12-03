using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerActions : MonoBehaviour
    {
        private PlayerManager _playerManager;
        
        private static readonly int Golpe = Animator.StringToHash("Golpe");

        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
        }
        
        public void HandlePrimaryAttack()
        {
            _playerManager.animator.SetBool(Golpe, true);
        }
    }
}
