using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuVictoria : MonoBehaviour
{
    public static MenuVictoria Instancia { get; private set; }

    [Header("Referencias de UI")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoMonedasGanadas;

    [Header("Audio (Opcional)")]
    public AudioSource musicaFondo;
    public AudioSource musicaVictoria;

    [Header("Nombres de Escenas")]
    public string nombreSiguienteEscena = "SeleccionNiveles";
    public string nombreMenuPrincipal = "MenuPrincipal";

    private void Awake()
    {
        if (Instancia == null)
        {
            Instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Ocultar el panel automáticamente al iniciar la escena
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(false);
        }
    }

    public void MostrarMenu(int monedasObtenidas)
    {
        if (panelVictoria != null)
        {
            panelVictoria.SetActive(true);
            Time.timeScale = 0f; // Pausa el tiempo del juego

            if (musicaFondo != null && musicaFondo.isPlaying)
            {
                musicaFondo.Stop();
            }

            if (musicaVictoria != null)
            {
                musicaVictoria.Play();
            }

            if (textoMonedasGanadas != null)
            {
                textoMonedasGanadas.text = "Monedas recolectadas: " + monedasObtenidas;
            }
        }
    }

    public void BotonSiguienteNivel()
    {
        Time.timeScale = 1f; // Reanudar el tiempo
        SceneManager.LoadScene(nombreSiguienteEscena);
    }

    public void BotonMenuPrincipal()
    {
        Time.timeScale = 1f; // Reanudar el tiempo
        SceneManager.LoadScene(nombreMenuPrincipal);
    }
}