using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    #region Variables
    
    [Header("Movement")]
    private InputPlayers _inputPlayers;
    private Vector3 _inputVector;
    private Rigidbody _rb;
    
    private bool _movement = true;
    private bool _attack = true;
    
    [SerializeField] private float speed = 1f;
    [SerializeField] private float groundDist;
    [SerializeField] private LayerMask terrainLayer;
    
    [Header("Animations")] 
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        _inputPlayers = new InputPlayers();
        _inputPlayers.Players.Movement2.Enable();
        _inputPlayers.Players.Punch2.Enable();

        _inputPlayers.Players.Punch2.performed += Punch2;
    }
    
    #region Enable & Disable
    private void OnEnable()
    {
        _inputPlayers.Enable();
        _inputPlayers.Players.Movement2.Enable();
    }

    private void OnDisable()
    {
        _inputPlayers.Disable();
        _inputPlayers.Players.Movement2.Disable();
    }
    #endregion

    private void FixedUpdate()
    {
        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        
        //Checamos la altura del Mesh y le sumamos lo que tenga que subir
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }
        
        //Moviento con InputSystem del Jugador1
        _inputVector = _inputPlayers.Players.Movement2.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(-_inputVector.x, 0, -_inputVector.y);
        _rb.velocity = moveDir * (speed);
        
        //Flipear Sprite
        if(_inputVector.x != 0 && _inputVector.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (_inputVector.x != 0 && _inputVector.x > 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void Punch2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            //Accion de golpear
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            
        }
        
        if (other.CompareTag("Player1"))
        {
            
        }
        
        if (other.CompareTag("RandomBox"))
        {
            
        }
    }
}
