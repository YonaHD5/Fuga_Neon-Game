using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public Image fondoPantalla; // Aquí va tu Panel con la ciudad
    public CanvasGroup grupoBoton; // Aquí va el Canvas Group del botón

    public void IniciarTutorial()
    {
        // Iniciamos la animación
        StartCoroutine(TransicionCinematica());
    }

    private IEnumerator TransicionCinematica()
    {
        // Apagamos los clics para que el jugador no le pique dos veces
        grupoBoton.interactable = false;

        float tiempo = 0f;
        float duracionTransicion = 1.5f; // Durará 1.5 segundos en apagarse
        
        while (tiempo < duracionTransicion)
        {
            tiempo += Time.deltaTime;
            float porcentaje = tiempo / duracionTransicion;

            // 1. La ciudad pasa de su color normal (blanco) a negro oscuro
            fondoPantalla.color = Color.Lerp(Color.white, Color.black, porcentaje);
            
            // 2. El botón desaparece suavemente (el Alpha baja de 1 a 0)
            grupoBoton.alpha = Mathf.Lerp(1f, 0f, porcentaje);

            yield return null; 
        }

        // Hacemos una pausa de medio segundo extra para que el jugador admire el título a oscuras
        yield return new WaitForSeconds(0.5f);

        // Ahora sí, lo mandamos al nivel
        SceneManager.LoadScene("SampleScene");
    }
}