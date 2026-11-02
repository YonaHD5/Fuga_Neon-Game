using UnityEngine;

public class PalancaPuerta : MonoBehaviour
{
    [Header("Referencias")]
    public Puerta puertaTarget;
    public SpriteRenderer spriteRenderer;
    public Sprite palancaDerecha; // Arrastra aquí el sprite de la palanca activada

    [Header("Audio")]
    public AudioClip sonidoPalanca;
    private AudioSource audioSource;

    private bool yaActivada = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!yaActivada && (collider.CompareTag("Player") || collider.name.Contains("Jugador")))
        {
            yaActivada = true;

            // 1. Cambia la palanca de izquierda a derecha
            if (spriteRenderer != null && palancaDerecha != null)
            {
                spriteRenderer.sprite = palancaDerecha;
            }

            // 2. Reproduce el sonido metálico/eléctrico
            if (audioSource != null && sonidoPalanca != null)
            {
                audioSource.PlayOneShot(sonidoPalanca);
            }

            // 3. Envía la señal para abrir la puerta
            if (puertaTarget != null)
            {
                puertaTarget.Abrir();
            }
        }
    }
}