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
        private GameManager _gameManager;

        private static readonly int Golpe = Animator.StringToHash("Golpe");
        private static readonly int Stunt = Animator.StringToHash("Stunt");

        private bool _isInvulnerable;
        public static event Action<int> OnPowerUpOut;

        private Transform childObject;
        private ParticleSystem particle;

        private void Awake()
        {
            childObject = this.transform.Find("FX_Trail_Dust_01");
            particle = childObject.GetComponent<ParticleSystem>();

            _playerManager = GetComponent<PlayerManager>();
            _playerConfig = _playerManager.playerConfig;
            _gameManager = FindAnyObjectByType<GameManager>();
        }

        public void HandlePrimaryAttack()
        {
            _playerManager.animator.SetBool(Golpe, true);
        }

        public void ActivatePowerUp(PowerUpSo powerUp)
        {
            if (!_playerConfig) return;

            Debug.Log($"Activating PowerUp: {powerUp.buffType.ToString()}");
            OnPowerUpOut?.Invoke(_playerManager.isPlayerOne ? 1 : 2);
            switch (powerUp.buffType)
            {
                case PowerUpType.Teleport:
                    HandleTeleport();
                    _playerConfig.ApplyBuff("speed", powerUp.speed);
                    _playerConfig.ApplyBuff("strength", powerUp.strength);
                    break;
                case PowerUpType.Movement:
                    _playerConfig.ApplyBuff("speed", powerUp.speed);
                    
                    childObject.gameObject.SetActive(true);
                    particle.Play();
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
            StartCoroutine(TeleportToEnemy());
        }

        public void OnHit(float duration)
        {
            if (_isInvulnerable) return;
            if(!_playerManager.canMove) return;
            Debug.Log($"Stunned for {duration}");
            if (!_playerManager.canMove) return;
            StartCoroutine(StunnedForSeconds(duration));
        }

        private IEnumerator PowerUpTimer(float timer)
        {
            yield return new WaitForSeconds(timer);
            Debug.Log($"PowerUp has expired");

            childObject.gameObject.SetActive(false);
            particle.Stop();

            _playerConfig.RevertBuff();
        }

        private IEnumerator TeleportToEnemy()
        {
            var teleportPosition = _gameManager.GetTeleportLocation(_playerManager.isPlayerOne ? 2 : 1);
            _playerManager.canMove = false;
            yield return new WaitForSeconds(.5f);
            transform.position = teleportPosition;
            yield return new WaitForSeconds(.25f);
            _playerManager.canMove = true;

        }

        private IEnumerator StunnedForSeconds(float duration)
        {
            _playerManager.playerSoundManager.PlayOnHit(duration > 4);
            _isInvulnerable = true;
            _playerManager.canMove = false;
            _playerManager.animator.SetBool(Stunt, true);
            yield return new WaitForSeconds(duration);
            _playerManager.animator.SetBool(Stunt, false);
            _playerManager.canMove = true;
            StartCoroutine(InvincibilityDuration(_playerConfig.invincibilityTime));
        }

        /* private IEnumerator InvincibilityDuration(float duration)
         {
             var spriteRenderer = _playerManager.GetComponentInChildren<SpriteRenderer>();
             if (!spriteRenderer)
             {
                 Debug.LogWarning("SpriteRenderer not found on player.");
                 yield break;
             }

             var originalColor = spriteRenderer.color; 
             var elapsed = 0f;
             var isWhite = false;

             while (elapsed < duration)
             {
                 isWhite = !isWhite;
                 spriteRenderer.color = isWhite ? Color.red : originalColor;

                 yield return new WaitForSeconds(0.3f); 
                 elapsed += 0.3f;
             }

             spriteRenderer.color = originalColor;
             _isInvulnerable = false;
         }
         */
        private IEnumerator InvincibilityDuration(float duration)
        {
            var spriteRenderer = _playerManager.GetComponentInChildren<SpriteRenderer>();
            if (!spriteRenderer)
            {
                Debug.LogWarning("SpriteRenderer not found on player.");
                yield break;
            }

            var elapsed = 0f;
            var isVisible = true;

            while (elapsed < duration)
            {
                isVisible = !isVisible;
                spriteRenderer.enabled = isVisible;

                yield return new WaitForSeconds(0.15f);
                elapsed += 0.15f;
            }

            spriteRenderer.enabled = true;
            _isInvulnerable = false;
        }
        public void ResetPowerUpsForBothPlayers()
        {
            OnPowerUpOut?.Invoke(1);
            OnPowerUpOut?.Invoke(2);
            _playerManager.storedPowerUp = null;

            childObject.gameObject.SetActive(false);
            particle.Stop();

            //Debug.Log("PowerUps deshabilitados para ambos jugadores");
            _playerConfig.RevertBuff();
        }

    }
}
