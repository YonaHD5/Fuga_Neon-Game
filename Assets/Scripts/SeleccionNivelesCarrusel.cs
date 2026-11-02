using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SeleccionNivelesCarrusel : MonoBehaviour
{
    [Header("Tarjetas de Niveles")]
    public GameObject[] tarjetasNiveles; // Arrastra TarjetaNivel1, TarjetaNivel2, etc.

    [Header("Botones de Navegación")]
    public Button botonIzquierda;
    public Button botonDerecha;

    [Header("Escenas")]
    public string[] nombresEscenas = { "Nivel1_Suburbios", "Nivel2" };
    public string escenaMenuPrincipal = "MenuPrincipal";

    private int indiceActual = 0;

    private void Start()
    {
        ActualizarCarrusel();
    }

    public void SiguienteNivel()
    {
        if (indiceActual < tarjetasNiveles.Length - 1)
        {
            indiceActual++;
            ActualizarCarrusel();
        }
    }

    public void AnteriorNivel()
    {
        if (indiceActual > 0)
        {
            indiceActual--;
            ActualizarCarrusel();
        }
    }

    private void ActualizarCarrusel()
    {
        // Enciende únicamente la tarjeta actual y apaga las demás
        for (int i = 0; i < tarjetasNiveles.Length; i++)
        {
            if (tarjetasNiveles[i] != null)
            {
                tarjetasNiveles[i].SetActive(i == indiceActual);
            }
        }

        // Desactiva la flecha izquierda si estamos en el primer nivel
        if (botonIzquierda != null) 
            botonIzquierda.interactable = (indiceActual > 0);

        // Desactiva la flecha derecha si estamos en el último nivel
        if (botonDerecha != null) 
            botonDerecha.interactable = (indiceActual < tarjetasNiveles.Length - 1);
    }

    public void CargarNivelActual()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombresEscenas[indiceActual]);
    }

    public void VolverAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(escenaMenuPrincipal);
    }
}