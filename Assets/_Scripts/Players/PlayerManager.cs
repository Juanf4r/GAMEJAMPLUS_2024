using _Scripts.Players.ScriptableObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts.Players
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField] private InputManagerSo inputReaderSo;
        [SerializeField] private bool isPlayerOne;
        [SerializeField] public Vector2 playerInput;
        
        [Header("Flags")] 
        public bool canMove;
        public bool isAttacking;

        //Referencess
        private PlayerLocomotion _playerLocomotion;
        private PlayerActions _playerActions;
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
            
        }

        private void HandlePunch()
        {
            _playerActions.HandlePrimaryAttack();
        }

        private void HandleMovement(Vector2 input)
        {
            playerInput = input * -1f;
            if(input.x == 0) return;
            spriteRenderer.flipX = input.x < 0;
        }

        private void UpdateAnimator()
        {
            animator.SetFloat(id: Movimiento, (int)moveVelocity);
        }
        
        
        //Get All References
        private void GetAllReferences()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _playerActions = GetComponent<PlayerActions>();
            animator = GetComponentInChildren<Animator>();
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
}
