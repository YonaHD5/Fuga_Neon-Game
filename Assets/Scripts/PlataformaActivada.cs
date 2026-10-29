using UnityEngine;

public class PlataformaActivada : MonoBehaviour
{
    [Header("Configuración de Destino")]
    public Transform puntoDestino;
    public float velocidad = 3f;

    private bool debeMoverse = false;

    void Update()
    {
        // Si el jugador ya la pisó, la plataforma avanza
        if (debeMoverse && puntoDestino != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, puntoDestino.position, velocidad * Time.deltaTime);

            // Cuando llega exactamente al destino, se detiene por completo
            if (Vector3.Distance(transform.position, puntoDestino.position) < 0.01f)
            {
                debeMoverse = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name.Contains("Jugador"))
        {
            // Enciende el motor de la plataforma al sentir al gato
            debeMoverse = true; 
            
            // Empareja al jugador para que viaje con ella sin resbalarse
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.name.Contains("Jugador"))
        {
            // Suelta al jugador cuando salta de la plataforma
            collision.transform.SetParent(null);
        }
    }
}