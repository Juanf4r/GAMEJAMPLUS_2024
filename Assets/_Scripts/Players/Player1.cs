using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : MonoBehaviour
{
    #region Variables
    
    [Header("Movement")]
    private InputPlayers _inputPlayers;
    private Vector3 _inputVector;
    private Rigidbody _rb;
    
    private bool _movement = true;
    private bool _attack = true;
    
    public float speed = 1f;
    [SerializeField] private float groundDist;
    [SerializeField] private LayerMask terrainLayer;
    public float timeStu = 4;
    
    [Header("Animations")] 
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Power UP")]
    private bool tp = false;
    [SerializeField] private GameObject uIPlayer1_TP;
    private bool moreVel = false;
    [SerializeField] private GameObject uIPlayer1_Vel;
    private bool moreAtt = false;
    [SerializeField] private GameObject uIPlayer1_Att;
    private float timerPowerUP;
    #endregion

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        
        _inputPlayers = new InputPlayers();
        _inputPlayers.Players.Movement1.Enable();
        _inputPlayers.Players.Punch1.Enable();
        _inputPlayers.Players.PowerUp1.Enable();

        _inputPlayers.Players.Punch1.performed += Punch1;
        _inputPlayers.Players.PowerUp1.performed += PowerUps;   

    }
    
    #region Enable & Disable
    private void OnEnable()
    {
        _inputPlayers.Enable();
        _inputPlayers.Players.Movement1.Enable();
    }

    private void OnDisable()
    {
        _inputPlayers.Disable();
        _inputPlayers.Players.Movement1.Disable();
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
        _inputVector = _inputPlayers.Players.Movement1.ReadValue<Vector2>();
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

        timerPowerUP = timerPowerUP + Time.deltaTime;
        if (timerPowerUP >= 6)
        {
            speed = 2.5f;
            timeStu = 4f;
        }
    }

    private void Punch1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Golpeo el Jugador 1");
        }
    }
    private void PowerUps(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            powerUP();
        }
    }

    //private void Powe

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            GameManager.Instance.GanarRondaJugador1();
        }
        else if (other.CompareTag("PowerUPTP") && !moreVel && !moreAtt)
        {
            tp = true;
            uIPlayer1_TP.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUPVelocity") && !tp && !moreAtt)
        {
            moreVel = true;
            uIPlayer1_Vel.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUPAttack") && !tp && !moreVel)
        {
            moreAtt = true;
            uIPlayer1_Att.SetActive(true);
            Destroy(other.gameObject);
        }
    }

    private void powerUP()
    {
        if (tp)
        {
            PU_TP.Instance.teleportP1();
            uIPlayer1_TP.SetActive(false);
            tp = false;
        }
        else if (moreVel)
        {
            timerPowerUP = 0f;
            speed = 10f;
            uIPlayer1_Vel.SetActive(false);
            moreVel = false;
        }
        else if (moreAtt)
        {
            timerPowerUP = 0f;
            timeStu = 10f;
            uIPlayer1_Att.SetActive(false);
            moreAtt = false;
        }
    }
}
