using UnityEngine;

public class BotonPuerta : MonoBehaviour
{
    [Header("Referencias")]
    public Puerta puertaTarget;
    public SpriteRenderer spriteRenderer;
    public Sprite spritePresionado; // Imagen opcional del botón hundido/encendido

    [Header("Audio")]
    public AudioClip sonidoBoton;
    private AudioSource audioSource;

    private bool yaPresionado = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!yaPresionado && (collider.CompareTag("Player") || collider.name.Contains("Jugador")))
        {
            yaPresionado = true;

            // 1. Cambiar aspecto del botón
            if (spriteRenderer != null && spritePresionado != null)
            {
                spriteRenderer.sprite = spritePresionado;
            }

            // 2. Reproducir sonido
            if (audioSource != null && sonidoBoton != null)
            {
                audioSource.PlayOneShot(sonidoBoton);
            }

            // 3. Mandar la orden de abrir la puerta
            if (puertaTarget != null)
            {
                puertaTarget.Abrir();
            }
        }
    }
}