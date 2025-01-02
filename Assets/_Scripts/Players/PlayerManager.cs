using System;
using _ScriptableObjects.Scripts;
using _Scripts.Players.ScriptableObjects;
using UnityEngine;

namespace _Scripts.Players
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private InputManagerSo inputReaderSo;
        [SerializeField] public Vector2 playerInput;
        public bool isPlayerOne;
        
        [Header("Flags")] 
        public bool canMove;
        public bool isAttacking;
        
        //Events
        public static event Action<PowerUpSo, int> OnPowerUpUpdated;
        
        //Referencess
        private PlayerLocomotion _playerLocomotion;
        private PlayerActions _playerActions;
        private PowerUpSo _storedPowerUp;
        private HammerController _hammerController;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public PlayerConfig playerConfig;
        
        
        private static readonly int Movimiento = Animator.StringToHash("Movimiento");

                
        //Variables
        public float moveVelocity;
        
        private void Awake()
        {
            GetAllReferences();
        }

        private void OnEnable()
        {
            canMove = false;
            if (isPlayerOne)
            {
                inputReaderSo.Initialize();
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
            playerConfig.OnStart();
        }
        
        private void OnDisable()
        {
            if (isPlayerOne)
            {
                inputReaderSo.PlayerOneMoveEvent -= HandleMovement;
                inputReaderSo.PlayerOnePunchEvent -= HandlePunch;
                inputReaderSo.PlayerOnePowerUpEvent -= HandlePowerUp;
            }
            else
            {
                inputReaderSo.PlayerTwoMoveEvent -= HandleMovement;
                inputReaderSo.PlayerTwoPunchEvent -= HandlePunch;
                inputReaderSo.PlayerTwoPowerUpEvent -= HandlePowerUp;
            }
        }

        private void FixedUpdate()
        {
            _playerLocomotion.HandleAllLocomotion();
        }
        
        private void Update()
        {
            UpdateAnimator();
        }
        
        private void LateUpdate()
        {
             
        }

        private void HandlePowerUp()
        {
            if(!_storedPowerUp || !canMove) return;
            _playerActions.ActivatePowerUp(_storedPowerUp);
            _storedPowerUp = null;
        }

        private void HandlePunch()
        {
            if (!canMove) return;
            _playerActions.HandlePrimaryAttack();
        }

        private void HandleMovement(Vector2 input)
        {
            if (!canMove) return;
            playerInput = input * -1f;
            if(input.x == 0) return;
            spriteRenderer.flipX = input.x < 0;
            _hammerController.FlipCollider(input.x);
            
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(id: Movimiento, (int)moveVelocity);
        }
        
        //Public methods
        public void UpdateStoredPowerUp(PowerUpSo newPowerUp)
        {
            OnPowerUpUpdated?.Invoke(newPowerUp, isPlayerOne ? 1 : 2);
            _storedPowerUp = newPowerUp;
        }
        
        //Get All References
        private void GetAllReferences()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerActions = GetComponent<PlayerActions>();
            animator = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _hammerController = GetComponentInChildren<HammerController>();
        }
    }
}
