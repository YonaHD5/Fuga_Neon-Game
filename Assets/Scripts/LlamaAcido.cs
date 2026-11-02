using System.Collections;
using UnityEngine;

public class LlamaAcido : MonoBehaviour
{
    [Header("Configuración de Salto")]
    public float fuerzaSalto = 12f;       // Altura a la que saldrá impulsada
    public float tiempoEspera = 3f;       // Tiempo en segundos entre cada salto
    public float gravedadSalto = 2.5f;    // Gravedad para controlar la caída

    [Header("Referencias")]
    private Rigidbody2D rb;
    private Vector3 posicionInicial;
    private SpriteRenderer spriteRenderer;
    private bool estaSaltando = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        // Guardar el punto de origen debajo del ácido
        posicionInicial = transform.position;
        rb.gravityScale = 0; // Se mantiene quieta al inicio
        rb.linearVelocity = Vector2.zero;

        StartCoroutine(RutinaSalto());
    }

    private void Update()
    {
        if (estaSaltando)
        {
            // Girar el sprite hacia abajo cuando comience a caer
            if (rb.linearVelocity.y < 0)
            {
                spriteRenderer.flipY = true;
            }
            else
            {
                spriteRenderer.flipY = false;
            }
        }
    }

    private IEnumerator RutinaSalto()
    {
        while (true)
        {
            // Esperar el tiempo configurado debajo del agua/ácido
            yield return new WaitForSeconds(tiempoEspera);

            // Reiniciar posición y lanzar hacia arriba
            transform.position = posicionInicial;
            spriteRenderer.flipY = false;
            rb.gravityScale = gravedadSalto;
            rb.linearVelocity = new Vector2(0, fuerzaSalto);
            estaSaltando = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Regresar abajo al tocar el agua/piso o dañar al jugador
        if (collider.CompareTag("Player"))
        {
            PlayerController jugador = collider.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.PerderVida();
            }
        }
    }
}