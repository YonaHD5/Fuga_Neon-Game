using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de escenas

public class MenuPausa : MonoBehaviour
{
    public static bool juegoPausado = false;
    public GameObject menuPausaUI; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (juegoPausado) Reanudar();
            else Pausar();
        }
    }

    void Pausar()
    {
        menuPausaUI.SetActive(true); 
        Time.timeScale = 0f;
        juegoPausado = true;
    }

    public void Reanudar() 
    {
        menuPausaUI.SetActive(false); 
        Time.timeScale = 1f;
        juegoPausado = false;
    }

    // NUEVA FUNCIÓN PARA EL BOTÓN
    public void IrAlMenu()
    {
        Time.timeScale = 1f; // Importante: Descongelar el tiempo antes de cambiar de escena
        juegoPausado = false;
        // Cambia "MenuPrincipal" por el nombre exacto de tu escena de inicio
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}