using System.Collections;
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
    private bool canAttack = false;

    public float speed = 4.2f;
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

    private GameObject refPlayer1;
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
        _inputPlayers.Players.PowerUp2.Enable();
        _inputPlayers.Players.Punch2.Enable();
        _inputPlayers.Players.Pause.Enable();
    }

    private void OnDisable()
    {
        _inputPlayers.Disable();
        _inputPlayers.Players.Movement2.Disable();
        _inputPlayers.Players.PowerUp2.Disable();
        _inputPlayers.Players.Punch2.Disable();
        _inputPlayers.Players.Pause.Disable();
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

        _inputVector = _inputPlayers.Players.Movement2.ReadValue<Vector2>();
        Vector3 moveDir = new Vector3(-_inputVector.x, 0, -_inputVector.y);

        if (moveDir.magnitude > 1)
        {
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

        _attack = false;
        playerAnimator.SetBool("Golpe", _attack);

        if (_inputVector.x != 0 && _inputVector.x < 0)
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
            speed = 4.2f;
            timeStu = 4f;
        }
    }

    private void Punch2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _attack = true;
            playerAnimator.SetBool("Golpe", true);

            if (canAttack)
            {
                refPlayer1 = GameObject.Find("Player1");
                //Debug.LogError("Entro");
                refPlayer1.GetComponent<Player1>().Stunt(timeStu);
            }
        }
    }

    private void PowerUps2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PowerUp();
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
        else if (other.CompareTag("Player1"))
        {
            canAttack = true;
            //Debug.LogError(canAttack);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player2"))
        {
            canAttack = false;
            //Debug.LogError(canAttack);
        }
    }
    public void Stunt(float ts)
    {
        StartCoroutine(StuntCoroutine(ts));
    }

    public IEnumerator StuntCoroutine(float duration)
    {
        _movement = false; 
        _rb.velocity = Vector3.zero; 

        yield return new WaitForSeconds(duration);

        _movement = true;
    }

    private void PowerUp()
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
