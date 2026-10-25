using UnityEngine;

public class PlataformaMovil : MonoBehaviour
{
    [Header("Puntos de Movimiento")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 3f;

    private Vector3 destino;

    void Start()
    {
        // La plataforma comienza moviéndose hacia el Punto B
        destino = puntoB.position;
    }

    void Update()
    {
        // Movimiento suave hacia el destino
        transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

        // Si llega al destino, cambia al punto contrario
        if (Vector3.Distance(transform.position, destino) < 0.1f)
        {
            destino = destino == puntoA.position ? puntoB.position : puntoA.position;
        }
    }

    // Evita que el jugador resbale al estar encima
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name.Contains("Jugador"))
        {
            collision.transform.SetParent(transform);
        }
    }

    // Suelta al jugador cuando salta de la plataforma
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name.Contains("Jugador"))
        {
            collision.transform.SetParent(null);
        }
    }
}