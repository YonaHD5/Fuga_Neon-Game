using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{
    [Header("Sprites de la Bandera")]
    public SpriteRenderer spriteRenderer;
    public Sprite banderaAmarilla; // Aquí pondremos tu dibujo de la bandera amarilla

    [Header("Audio")]
    public AudioClip sonidoCheckpoint;
    private AudioSource audioSource;

    [Header("Texto Animado")]
    public GameObject textoCheckpoint;
    public float tiempoTexto = 2f;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        if (textoCheckpoint != null)
        {
            textoCheckpoint.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player") || collider.name.Contains("Jugador"))
        {
            PlayerController scriptJugador = collider.GetComponent<PlayerController>();
            
            if (scriptJugador != null)
            {
                // 1. Guardar la posición en el jugador
                scriptJugador.ActualizarCheckpoint(transform.position);
                
                // 2. Cambiar la imagen a la bandera amarilla
                if (spriteRenderer != null && banderaAmarilla != null)
                {
                    spriteRenderer.sprite = banderaAmarilla;
                }

                // 3. Reproducir el sonido
                if (audioSource != null && sonidoCheckpoint != null)
                {
                    audioSource.PlayOneShot(sonidoCheckpoint);
                }

                // 4. Encender el texto temporalmente
                if (textoCheckpoint != null)
                {
                    StartCoroutine(MostrarTexto());
                }
                
                // 5. Desactivar el trigger para que no se repita
                GetComponent<Collider2D>().enabled = false;
            }
        }
    }

    private IEnumerator MostrarTexto()
    {
        textoCheckpoint.SetActive(true);
        yield return new WaitForSeconds(tiempoTexto);
        textoCheckpoint.SetActive(false);
    }
}