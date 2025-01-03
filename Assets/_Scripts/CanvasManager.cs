using System.Collections.Generic;
using _ScriptableObjects.Scripts;
using _Scripts.Players;
using UnityEngine;
using UnityEngine.UI;

namespace _Scripts
{
    public class CanvasManager : MonoBehaviour
    {
        [SerializeField] private List<Image> powerUpList = new();

        private void OnEnable()
        {
            PlayerManager.OnPowerUpUpdated += UpdateImage;
            PlayerActions.OnPowerUpOut += DisablePowerUp;
        }
        private void Start()
        {
            foreach (var image in powerUpList)
            {
                image.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            PlayerManager.OnPowerUpUpdated -= UpdateImage;
            PlayerActions.OnPowerUpOut -= DisablePowerUp;
        }

        private void UpdateImage(PowerUpSo powerUpData, int player)
        {
            var image = powerUpList[player - 1];
            image.sprite = powerUpData.buffSprite;
            image.gameObject.SetActive(true);
        }

        public void DisablePowerUp(int player)
        {
            powerUpList[player - 1].gameObject.SetActive(false);
        }
    }
}
