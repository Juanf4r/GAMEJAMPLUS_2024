using UnityEngine;

public class PU_TP : MonoBehaviour
{
    public static PU_TP Instance;

    private GameObject refPlayer1;
    private GameObject refPlayer2;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        refPlayer1 = GameObject.FindGameObjectWithTag("Player");
        refPlayer2 = GameObject.FindGameObjectWithTag("Player2");

        if (refPlayer1 == null || refPlayer2 == null)
        {
            Debug.LogError("Player1 o Player2 no se encontraron en la escena.");
        }
    }

    public void TeleportP1()
    {
        if (refPlayer1 == null || refPlayer2 == null) return;
        (refPlayer1.transform.localPosition, refPlayer2.transform.localPosition) = (refPlayer2.transform.localPosition, refPlayer1.transform.localPosition);
    }

    public void teleportP2()
    {
        if (refPlayer1 == null || refPlayer2 == null) return;
        var tempPosition = refPlayer2.transform.localPosition;
        refPlayer2.transform.localPosition = refPlayer1.transform.localPosition;
        refPlayer1.transform.localPosition = tempPosition;
    }

}
