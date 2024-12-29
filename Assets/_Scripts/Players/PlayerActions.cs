using System;
using System.Collections;
using _ScriptableObjects.Scripts;
using Unity.VisualScripting;
using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerActions : MonoBehaviour
    {
        private PlayerManager _playerManager;
        private PlayerConfig _playerConfig;
        
        private static readonly int Golpe = Animator.StringToHash("Golpe");
        private static readonly int Stunt = Animator.StringToHash("Stunt");
        
        private void Awake()
        {
            _playerManager = GetComponent<PlayerManager>();
            _playerConfig = _playerManager.playerConfig;
        }
        
        public void HandlePrimaryAttack()
        {
            _playerManager.animator.SetBool(Golpe, true);
        }

        public void ActivatePowerUp(PowerUpSo powerUp)
        {
            if (!_playerConfig) return;

            Debug.Log($"Activating PowerUp: {powerUp.buffType.ToString()}");
            
            switch (powerUp.buffType)
            {
                case PowerUpType.Teleport:
                    HandleTeleport();
                    _playerConfig.ApplyBuff("speed", powerUp.speed);
                    _playerConfig.ApplyBuff("strength", powerUp.strength);
                    break;
                case PowerUpType.Movement:
                    _playerConfig.ApplyBuff("speed", powerUp.speed);
                    break;
                case PowerUpType.Strength:
                    _playerConfig.ApplyBuff("speed", powerUp.speed);
                    _playerConfig.ApplyBuff("strength", powerUp.strength);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            StartCoroutine(PowerUpTimer(powerUp.duration));
        }
        
        private void HandleTeleport()
        {
            
        }
        
        public void OnHit(float duration)
        {
            Debug.Log($"Stunned for {duration}");
            if(!_playerManager.canMove) return;
            StartCoroutine(StunnedForSeconds(duration));
        }
        
        private IEnumerator PowerUpTimer(float timer)
        {
            yield return new WaitForSeconds(timer);
            Debug.Log($"PowerUp has expired");
            _playerConfig.RevertBuff();
        }

        private IEnumerator StunnedForSeconds(float duration)
        {
            _playerManager.canMove = false;
            _playerManager.animator.SetBool(Stunt, true);
            yield return new WaitForSeconds(duration);
            _playerManager.animator.SetBool(Stunt, false);
            _playerManager.canMove = true;
        }
    }
}
 