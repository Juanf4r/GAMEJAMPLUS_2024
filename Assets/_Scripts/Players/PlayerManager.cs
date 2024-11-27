using System;
using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private InputManagerSo inputReaderSo;
        [SerializeField] private bool isPlayerOne; 

        public Vector2 playerInput;

        private void OnEnable()
        {
            if (isPlayerOne)
            {
                inputReaderSo.PlayerOneMoveEvent += HandleMovement;
                inputReaderSo.PlayerOnePunchEvent += HandlePunch;
                inputReaderSo.PlayerOnePowerUpEvent += HandlePowerUp;
            }
            else
            {
                inputReaderSo.PlayerTwoMoveEvent += HandleMovement;
                inputReaderSo.PlayerTwoPunchEvent += HandlePunch;
                inputReaderSo.PlayerTwoPowerUpEvent += HandlePowerUp;
            }
        }

        private void HandlePowerUp()
        {
            
        }

        private void HandlePunch()
        {
            
        }

        private void HandleMovement(Vector2 input)
        {
            playerInput = input;
        }
    }
}
