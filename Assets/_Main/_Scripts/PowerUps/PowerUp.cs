using System;
using _ScriptableObjects.Scripts;
using _Scripts.Players;
using Assets.Minimap;
using UnityEngine;

namespace _Scripts.PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] public PowerUpSo powerUpType;
        [SerializeField] private Animator powerUpAnimator;
        private static readonly int AlphaTex = Shader.PropertyToID("_AlphaTex");

        private void OnEnable()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;
            spriteRenderer.sprite = powerUpType.buffSprite;
            if (spriteRenderer.material == null || powerUpType.buffSprite == null) return;
            var mainTexture = powerUpType.buffSprite.texture;
            spriteRenderer.material.mainTexture = mainTexture;
            if (powerUpType.alphaTexture == null) return;
            spriteRenderer.material.SetTexture(AlphaTex, powerUpType.alphaTexture.texture);
            if (powerUpType.powerUpAnimator != null && powerUpAnimator != null)
            {
                powerUpAnimator.runtimeAnimatorController = powerUpType.powerUpAnimator.runtimeAnimatorController;
                powerUpAnimator.Play(0);
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var component = other.GetComponent<PlayerManager>();

            if (component.storedPowerUp != null) return;
            component.UpdateStoredPowerUp(powerUpType);
            MinimapController.instance.RemoveElementAtRuntime(this.transform);
            gameObject.SetActive(false);
            AnimationToImagePU.Instance.SelectPO(powerUpType, component.isPlayerOne);
        }
    }
}
