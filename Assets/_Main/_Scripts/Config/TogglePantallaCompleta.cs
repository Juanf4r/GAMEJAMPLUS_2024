using UnityEngine;

public class TogglePantallaCompleta : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            CambiarModoPantalla();
        }
    }

    public void CambiarModoPantalla()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}