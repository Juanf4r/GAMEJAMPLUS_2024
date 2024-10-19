using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player2 : MonoBehaviour
{
    public static Player2 Instance;
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
    public float timeStu = 4;

    [Header("Animations")] 
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Power UP")]
    private bool tp = false;
    [SerializeField] private GameObject uIPlayer2_TP;
    private bool moreVel = false;
    [SerializeField] private GameObject uIPlayer2_Vel;
    private bool moreAtt = false;
    [SerializeField] private GameObject uIPlayer2_Att;
    private float timerPowerUP;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        _inputPlayers = new InputPlayers();
        _inputPlayers.Players.Movement2.Enable();
        _inputPlayers.Players.Punch2.Enable();
        _inputPlayers.Players.PowerUp2.Enable();

        _inputPlayers.Players.Punch2.performed += Punch2;
        _inputPlayers.Players.PowerUp2.performed += PowerUps2;
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
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
            Debug.Log("Golpeo el Jugador 2");
        }
    }
    private void PowerUps2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            powerUP();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            GameManager.Instance.GanarRondaJugador2();
        }

        else if (other.CompareTag("PowerUPTP") && !moreVel && !moreAtt)
        {
            tp = true;
            uIPlayer2_TP.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUPVelocity") && !tp && !moreAtt)
        {
            moreVel = true;
            uIPlayer2_Vel.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUPAttack") && !tp && !moreVel)
        {
            moreAtt = true;
            uIPlayer2_Att.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    private void powerUP()
    {
        if (tp)
        {
            PU_TP.Instance.teleportP1();
            uIPlayer2_TP.SetActive(false);
            tp = false;
        }
        else if (moreVel)
        {
            timerPowerUP = 0f;
            speed = 10f;
            uIPlayer2_Vel.SetActive(false);
            moreVel = false;
        }
        else if (moreAtt)
        {
            timerPowerUP = 0f;
            timeStu = 10f;
            uIPlayer2_Att.SetActive(false);
            moreAtt = false;
        }
    }
    public void restart()
    {
        tp = false;
        uIPlayer2_TP.SetActive(false);
        moreAtt = false;
        uIPlayer2_Vel.SetActive(false);
        moreVel = false;
        uIPlayer2_Att.SetActive(false);
        timerPowerUP = timerPowerUP + 6;
    }
}
