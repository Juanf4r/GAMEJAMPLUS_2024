using UnityEngine;
using _Scripts.Players;

public class CameraHelper : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 5, -10);
    public float followSpeed = 5f;

    private Vector3 velocity = Vector3.zero;
    private PlayerActions playerActions;

    private void Start()
    {
        if (target != null)
        {
            playerActions = target.GetComponent<PlayerActions>();
        }
    }

    void LateUpdate()
    {
        if (target == null || playerActions == null)
            return;

        // Verificar si tiene el PowerUp de Movement
        if (playerActions.HasMovementPowerUp)
            return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / followSpeed);
    }
}
