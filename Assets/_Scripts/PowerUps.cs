using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    public static PowerUps Instance;

    [Header("PowerUps")]
    [SerializeField] private GameObject tpUP;
    [SerializeField] private GameObject velUP;
    [SerializeField] private GameObject attUP;

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
    private void Start()
    {
        PowerUP();
    }
    public void PowerUP()
    {
        int random = Random.Range(0, 3);  

        switch (random)
        {
            case 0:
                Instantiate(tpUP, transform.position, Quaternion.identity); 
                break;
            case 1:
                Instantiate(velUP, transform.position, Quaternion.identity); 
                break;
            case 2:
                Instantiate(attUP, transform.position, Quaternion.identity); 
                break;
        }
    }
}
