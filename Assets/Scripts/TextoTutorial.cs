using UnityEngine;
using TMPro;

public class TextoTutorial : MonoBehaviour
{
    [Header("Referencias")]
    public GameObject contenedorTexto; // El objeto del texto (Text TMP)
    public TextMeshPro componenteTexto;
    
    [Header("Mensaje")]
    [TextArea(2, 5)]
    public string mensaje = "¡Puerta bloqueada!\nSube a la cima y activa la palanca.";

    private void Start()
    {
        if (contenedorTexto != null)
        {
            contenedorTexto.SetActive(false);
        }

        if (componenteTexto != null)
        {
            componenteTexto.text = mensaje;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Detecta si lo que entró es el jugador revisando su script o su Tag
        if (collider.GetComponentInParent<PlayerController>() != null || collider.CompareTag("Player"))
        {
            if (contenedorTexto != null)
            {
                contenedorTexto.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.GetComponentInParent<PlayerController>() != null || collider.CompareTag("Player"))
        {
            if (contenedorTexto != null)
            {
                contenedorTexto.SetActive(false);
            }
        }
    }
}