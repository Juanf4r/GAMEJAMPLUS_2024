using UnityEngine;

namespace _Scripts.Players
{
    public class HammerController : MonoBehaviour
    {
        private GameObject _owner;
        private PlayerManager _playerManager;
        private BoxCollider _collider;

        private void Awake()
        {
            _playerManager = GetComponentInParent<PlayerManager>();
            _owner = _playerManager.gameObject;
            _collider = GetComponent<BoxCollider>(); // Cache the collider
        }

        public void FlipCollider(float direction)
        {
            Debug.Log($"{_owner.name} FlipCollider called with direction: {direction}");

            // Get current center and size
            Vector3 center = _collider.center;
            Vector3 size = _collider.size;

            // Calculate new center (flip around the pivot)
            center.x = Mathf.Abs(center.x) * Mathf.Sign(direction);
            
            // Apply the changes
            _collider.center = center;
            
            Debug.Log($"{_owner.name} New collider center: {center}");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player") || other.gameObject == _owner) return;
            Debug.Log($"{_owner.name} hammer hit {other.gameObject.name}");
            other.GetComponent<PlayerActions>().OnHit(_playerManager.playerConfig.strength);
        }
    }
}