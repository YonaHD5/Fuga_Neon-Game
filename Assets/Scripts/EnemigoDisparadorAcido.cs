using System.Collections;
using UnityEngine;

public class EnemigoDisparadorAcido : MonoBehaviour
{
    [Header("Configuración de Disparo")]
    public GameObject prefabProyectilAcido;
    public Transform puntoDisparo;
    public float tiempoEntreAtaques = 3f;      // Pausa entre ráfaga y ráfaga
    public float velocidadProyectil = 6f;
    public float rangoDeteccion = 8f;

    [Header("Ráfaga Alineada")]
    public int cantidadPorRafaga = 3;           // Cantidad de balas por ráfaga
    public float tiempoEntreRafagas = 0.15f;    // Intervalo entre cada bala de la ráfaga

    [Header("Audio")]
    public AudioClip sonidoDisparo;             // AudioClip del disparo de ácido

    private Transform jugador;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            jugador = playerObj.transform;
        }

        StartCoroutine(RutinaDisparo());
    }

    private IEnumerator RutinaDisparo()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoEntreAtaques);

            // Si el gato está dentro del rango, dispara la ráfaga alineada
            if (jugador != null && Vector2.Distance(transform.position, jugador.position) <= rangoDeteccion)
            {
                for (int i = 0; i < cantidadPorRafaga; i++)
                {
                    Disparar();
                    yield return new WaitForSeconds(tiempoEntreRafagas);
                }
            }
        }
    }

    private void Disparar()
    {
        if (prefabProyectilAcido == null || puntoDisparo == null || jugador == null) return;

        GameObject proyectil = Instantiate(prefabProyectilAcido, puntoDisparo.position, Quaternion.identity);
        ProyectilAcido scriptProyectil = proyectil.GetComponent<ProyectilAcido>();

        if (scriptProyectil != null)
        {
            Vector2 direccionHaciaJugador = (jugador.position - puntoDisparo.position).normalized;
            scriptProyectil.velocidad = velocidadProyectil;
            scriptProyectil.SetDireccion(direccionHaciaJugador);

            // Reproduce el sonido de disparo en la posición del enemigo
            if (sonidoDisparo != null)
            {
                AudioSource.PlayClipAtPoint(sonidoDisparo, puntoDisparo.position);
            }

            // Orientar el sprite del enemigo según la posición del jugador
            if (jugador.position.x < transform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, rangoDeteccion);
    }
}
