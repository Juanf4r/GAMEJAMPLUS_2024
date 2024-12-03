using Unity.Collections;
using UnityEngine;

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
        public PlayerConfig playerConfig;
        private PlayerLocomotion _playerLocomotion;
        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        
        private static readonly int Movimiento = Animator.StringToHash("Movimiento");

                
        //Variables
        public float moveVelocity;
        public Vector2 lastInput;
        
        private void Awake()
        {
            GetAllReferences();
        }

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
            
        }

        private void HandleMovement(Vector2 input)
        {
            playerInput = input * -1f;
            if(input.x == 0) return;
            _spriteRenderer.flipX = input.x < 0;
        }

        private void UpdateAnimator()
        {
            _animator.SetFloat(id: Movimiento, (int)moveVelocity);
        }
        
        
        //Get All References
        private void GetAllReferences()
        {
            _playerLocomotion = GetComponent<PlayerLocomotion>();
            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }
    }
}
