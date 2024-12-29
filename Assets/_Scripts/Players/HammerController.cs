using UnityEngine;

namespace _Scripts.Players
{
    public class HammerController : MonoBehaviour
    {
        private GameObject _owner;
        private PlayerManager _playerManager;

        private void Awake()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            _owner = _playerManager.gameObject;
        }

        public void FlipCollider(float direction)
        {
            transform.localScale = new Vector3(direction, 1, 1);
        }
        
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || other.gameObject == _owner) return;
            other.GetComponent<PlayerActions>().OnHit(_playerManager.playerConfig.strength);
        }
    }
}
