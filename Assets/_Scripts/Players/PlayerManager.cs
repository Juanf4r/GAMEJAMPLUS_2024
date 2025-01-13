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
        private PlayerLocomotion playerLocomotion;
        private PlayerActions playerActions;
        private PowerUpSo storedPowerUp;
        private HammerController hammerController;
        private PlayerSoundManager playerSoundManager;
        public Animator animator;
        public SpriteRenderer spriteRenderer;
        public PlayerConfig playerConfig;
        
        
        private static readonly int Movimiento = Animator.StringToHash("Movimiento");

                
        //Variables
        public float moveVelocity;
        private static readonly int Win = Animator.StringToHash("Win");
        private static readonly int Lost = Animator.StringToHash("Lost");

        private void Awake()
        {
            GetAllReferences();
        }

        private void OnEnable()
        {
            animator.runtimeAnimatorController = playerConfig.playerAnimator;
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
            playerLocomotion.HandleAllLocomotion();
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
            if(!storedPowerUp || !canMove) return;
            playerActions.ActivatePowerUp(storedPowerUp);
            storedPowerUp = null;
        }

        private void HandlePunch()
        {
            if (!canMove) return;
            playerActions.HandlePrimaryAttack();
        }

        private void HandleMovement(Vector2 input)
        {
            if (!canMove) return;
            playerInput = input * -1f;
            if(input.x == 0) return;
            spriteRenderer.flipX = input.x < 0;
            hammerController.FlipCollider(isPlayerOne ? input.x: input.x *-1);

        }
        
        private void UpdateAnimator()
        {
            animator.SetFloat(id: Movimiento, (int)moveVelocity);
        }
        
        //Public methods
        public void UpdateStoredPowerUp(PowerUpSo newPowerUp)
        {
            OnPowerUpUpdated?.Invoke(newPowerUp, isPlayerOne ? 1 : 2);
            playerSoundManager.PlayPickup();
            storedPowerUp = newPowerUp;
        }
        
        public void OnWin()
        {
            canMove = false;
            animator.SetBool(Win, true);
        }
    
        public void OnLose()
        {
            canMove = false;
            animator.SetBool(Lost, true);
        }
        
        //Get All References
        private void GetAllReferences()
        {
            playerLocomotion = GetComponent<PlayerLocomotion>();
            playerActions = GetComponent<PlayerActions>();
            animator = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            hammerController = GetComponentInChildren<HammerController>();
            playerSoundManager = GetComponent<PlayerSoundManager>();
        }
    }
}
