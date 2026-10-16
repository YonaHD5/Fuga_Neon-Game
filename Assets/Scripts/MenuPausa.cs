using UnityEngine;

public class MenuPausa : MonoBehaviour
{
    public static bool juegoPausado = false;
    
    // Nueva variable para asignar la interfaz visual
    public GameObject menuPausaUI; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (juegoPausado)
            {
                Reanudar();
            }
            else
            {
                Pausar();
            }
        }
    }

    void Pausar()
    {
        // Enciende la interfaz y detiene el tiempo
        menuPausaUI.SetActive(true); 
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reanudar() 
    {
        // Apaga la interfaz y restaura el tiempo
        menuPausaUI.SetActive(false); 
        Time.timeScale = 1f;
        juegoPausado = false;
    }
}