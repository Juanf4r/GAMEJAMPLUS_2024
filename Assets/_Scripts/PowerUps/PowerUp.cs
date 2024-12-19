using System;
using _ScriptableObjects.Scripts;
using _Scripts.Players;
using UnityEngine;

namespace _Scripts.PowerUps
{
    public class PowerUp : MonoBehaviour
    {
        [SerializeField] private PowerUpSo powerUpType;
        private static readonly int AlphaTex = Shader.PropertyToID("_AlphaTex");

        private void Start()
        {
            var spriteRenderer = GetComponent<SpriteRenderer>();
            if (spriteRenderer == null) return;
            spriteRenderer.sprite = powerUpType.buffSprite;
            if (spriteRenderer.material == null || powerUpType.buffSprite == null) return;
            var mainTexture = powerUpType.buffSprite.texture;
            spriteRenderer.material.mainTexture = mainTexture;
            if (powerUpType.alphaTexture == null) return;
            spriteRenderer.material.SetTexture(AlphaTex, powerUpType.alphaTexture.texture);
        }
        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var actions = other.GetComponent<PlayerActions>();
            actions.HandlePowerUp(powerUpType);
            
            gameObject.SetActive(false);
        }
    }
}
