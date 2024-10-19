using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PU_TP : MonoBehaviour
{
    public static PU_TP Instance;

    [SerializeField] private GameObject refPlayer1;
    [SerializeField] private GameObject refPlayer2;

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
    }

    public void teleportP1()
    {
        Vector3 tempPosition = refPlayer1.transform.localPosition;
        refPlayer1.transform.localPosition = refPlayer2.transform.localPosition;
        refPlayer2.transform.localPosition = tempPosition;
        //Destroy(gameObject);
    }

    public void teleportP2()
    {
        Vector3 tempPosition = refPlayer2.transform.localPosition;
        refPlayer2.transform.localPosition = refPlayer1.transform.localPosition;
        refPlayer1.transform.localPosition = tempPosition;
        //Destroy(gameObject);
    }

}
