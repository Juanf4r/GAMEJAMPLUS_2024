using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player1 : MonoBehaviour
{
    public static Player1 Instance;
    #region Variables
    
    [Header("Movement")]
    public InputPlayers _inputPlayers;
    private Vector3 _inputVector;
    private Rigidbody _rb;

    private bool _movement = true;
    private bool _attack = true;
    private bool canAttack = false;
    
    public float speed = 4.2f;
    [SerializeField] private float groundDist;
    [SerializeField] private LayerMask terrainLayer;
    public float timeStu = 4;

    [SerializeField] private GameObject dustEffect;
    
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

    private GameObject refPlayer2;

    [Header("Sonidos")]
    [SerializeField] private AudioSource audios;
    [SerializeField] private AudioClip sonidoPaso;
    [SerializeField] private AudioClip sonidoGolpe;
    [SerializeField] private AudioClip sonidoGolpeFuerte;
    [SerializeField] private AudioClip sonidoPickUP;
    [SerializeField] private AudioClip sonidoVelocidad;
    [SerializeField] private AudioClip sonidoTP;
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

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    private void Start()
    {
    }

    #region Enable & Disable
    private void OnEnable()
    {
        _inputPlayers.Enable();
        _inputPlayers.Players.Movement1.Enable();
        _inputPlayers.Players.PowerUp1.Enable();
        _inputPlayers.Players.Punch1.Enable();
    }

    private void OnDisable()
    {
        _inputPlayers.Disable();
        _inputPlayers.Players.Movement1.Disable();
        _inputPlayers.Players.PowerUp1.Disable();
        _inputPlayers.Players.Punch1.Disable();
    }
    #endregion

    private void FixedUpdate()
    {
        if (!_movement)
        {
            return;
        }

        RaycastHit hit;
        Vector3 castPos = transform.position;
        castPos.y += 1;
        
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + groundDist;
                transform.position = movePos;
            }
        }

        _inputVector = _inputPlayers.Players.Movement1.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(-_inputVector.x, 0, -_inputVector.y);

        if (moveDir.magnitude > 1)
        {
            audios.clip = sonidoPaso;
            audios.Play();
            moveDir = moveDir.normalized;
        }
        _rb.velocity = moveDir * (speed);

        if (_inputVector.x == 0 && Mathf.Approximately(_inputVector.y, 1) || Mathf.Approximately(_inputVector.y, -1))
        {
            playerAnimator.SetFloat("Movimiento", _inputVector.y);
        }
        else
        {
            playerAnimator.SetFloat("Movimiento", _inputVector.x);
        }

        if (_attack)
        {
            Debug.Log("Reproduciendo golpe de martillo");
            
            
            if (moreAtt)
            {
                audios.clip = sonidoGolpeFuerte;
                audios.Play();
            }
            else
            {
                audios.clip = sonidoGolpe;
                audios.Play();
            }
        }

        _attack = false;
        playerAnimator.SetBool("Golpe",_attack);
        
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
            speed = 5f;
            timeStu = 4f;
            dustEffect.gameObject.SetActive(false);
        }
    }

    private void Punch1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _attack = true;
            playerAnimator.SetBool("Golpe", true);

            if (canAttack)
            {
                refPlayer2 = GameObject.Find("Player2");
                //Debug.LogError("Entro");
                refPlayer2.GetComponent<Player2>().Stunt(timeStu); 
            }
        }
    }

    private void PowerUps(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PowerUP();
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Meat"))
        {
            GameManager.Instance.GanarRondaJugador1();
        }
        else if (other.CompareTag("PowerUPTP") && !moreVel && !moreAtt)
        {
            audios.clip = sonidoPickUP;
            audios.Play();
            tp = true;
            uIPlayer1_TP.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUPVelocity") && !tp && !moreAtt)
        {
            audios.clip = sonidoPickUP;
            audios.Play();
            moreVel = true;
            uIPlayer1_Vel.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("PowerUPAttack") && !tp && !moreVel)
        {
            audios.clip = sonidoPickUP;
            audios.Play();
            moreAtt = true;
            uIPlayer1_Att.SetActive(true);
            Destroy(other.gameObject);
        }
        else if (other.CompareTag("Player2"))
        {
            canAttack = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            canAttack = false;
        }
    }
    public void Stunt(float ts)
    {
        StartCoroutine(StuntCoroutine(ts));
    }

    public IEnumerator StuntCoroutine(float duration)
    {
        playerAnimator.SetBool("Stunt", true);
        _movement = false;
        _rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(duration);

        _movement = true;
        
        playerAnimator.SetBool("Stunt", false);
    }

    private void PowerUP()
    {
        if (tp)
        {
            audios.clip = sonidoTP;
            audios.Play();
            PU_TP.Instance.teleportP1();
            uIPlayer1_TP.SetActive(false);
            tp = false;
        }
        else if (moreVel)
        {
            audios.clip = sonidoVelocidad;
            audios.Play();
            timerPowerUP = 0f;
            speed = 10f;
            
            dustEffect.gameObject.SetActive(true);
            
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
    
    public void restart()
    {
        tp = false;
        uIPlayer1_TP.SetActive(false);
        moreAtt = false;
        uIPlayer1_Vel.SetActive(false);
        moreVel = false;
        uIPlayer1_Att.SetActive(false);
        timerPowerUP = timerPowerUP + 6;
    }

    public void WinAnimationP1()
    {
        playerAnimator.SetBool("Win", true);
    }
    
    public void LostAnimationP1()
    {
        playerAnimator.SetBool("Lost", true);
    }
}
