using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    [Header("Referencias de la Interfaz")]
    public Image fondoPantalla;       // Panel o fondo con la ciudad
    public CanvasGroup grupoBotones;  // Canvas Group que contiene los botones del menú

    [Header("Nombres de Escenas")]
    public string escenaSeleccionNiveles = "SeleccionNiveles";
    public string escenaTutorial = "NivelTutorial";
    public string escenaTienda = "Tienda";

    // --- Métodos públicos para los eventos OnClick ---

    public void IniciarJuego()
    {
        StartCoroutine(TransicionCinematica(escenaSeleccionNiveles));
    }

    public void IniciarTutorial()
    {
        StartCoroutine(TransicionCinematica(escenaTutorial));
    }

    public void AbrirTienda()
    {
        StartCoroutine(TransicionCinematica(escenaTienda));
    }

    // --- Corrutina de Transición ---

    private IEnumerator TransicionCinematica(string nombreEscena)
    {
        // Apagamos los clics para evitar múltiples pulsaciones
        if (grupoBotones != null)
        {
            grupoBotones.interactable = false;
        }

        float tiempo = 0f;
        float duracionTransicion = 1.5f; // Duración en segundos

        while (tiempo < duracionTransicion)
        {
            tiempo += Time.deltaTime;
            float porcentaje = tiempo / duracionTransicion;

            // 1. La ciudad pasa de blanco a negro oscuro
            if (fondoPantalla != null)
            {
                fondoPantalla.color = Color.Lerp(Color.white, Color.black, porcentaje);
            }

            // 2. Los botones desaparecen suavemente (Alpha de 1 a 0)
            if (grupoBotones != null)
            {
                grupoBotones.alpha = Mathf.Lerp(1f, 0f, porcentaje);
            }

            yield return null;
        }

        // Pausa de 0.5s para apreciar la escena a oscuras
        yield return new WaitForSeconds(0.5f);

        // Aseguramos que el tiempo del juego esté activo y cargamos la escena
        Time.timeScale = 1f;
        SceneManager.LoadScene(nombreEscena);
    }
}