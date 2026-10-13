using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    // Aquí conectaremos el texto que queremos mostrar
    public GameObject textoTutorial; 

    void Start()
    {
        // Nos aseguramos de que el texto empiece apagado al arrancar el nivel
        if (textoTutorial != null)
        {
            textoTutorial.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Si el gato entra, encendemos el texto
        if (collider.CompareTag("Player")) 
        {
            if (textoTutorial != null) textoTutorial.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        // Cuando el gato sale de la zona, apagamos el texto
        if (collider.CompareTag("Player"))
        {
            if (textoTutorial != null) textoTutorial.SetActive(false);
        }
    }
}