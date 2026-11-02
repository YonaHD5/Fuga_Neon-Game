using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuVictoria : MonoBehaviour
{
    public static MenuVictoria Instancia { get; private set; }

    [Header("Referencias de UI")]
    public GameObject panelVictoria;
    public TextMeshProUGUI textoMonedasGanadas;

    [Header("Nombres de Escenas")]
    public string nombreSiguienteEscena = "Tienda"; // o "Nivel2"
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
        // Aseguramos que inicie oculto al comenzar la partida
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
            Time.timeScale = 0f; // Pausa el juego en segundo plano

            if (textoMonedasGanadas != null)
            {
                textoMonedasGanadas.text = "Monedas recolectadas: " + monedasObtenidas;
            }
        }
    }

    // Funciones para los botones de la UI
    public void BotonSiguienteNivel()
    {
        Time.timeScale = 1f; // Despausa el tiempo antes de cambiar de escena
        SceneManager.LoadScene(nombreSiguienteEscena);
    }

    public void BotonMenuPrincipal()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreMenuPrincipal);
    }
}