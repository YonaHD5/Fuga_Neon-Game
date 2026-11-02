using UnityEngine;

public class ProyectilLaser : MonoBehaviour
{
    [Header("Configuración del Láser")]
    public float velocidad = 12f;
    public float tiempoVida = 4f;

    public void Inicializar(Vector2 direccion)
    {
        // Orientar el sprite hacia la dirección del movimiento
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angulo);

        // Mover el proyectil
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direccion * velocidad;
        }

        // Destruir automáticamente después de cierto tiempo
        Destroy(gameObject, tiempoVida);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 1. Ignorar si choca con el Dron, Enemigos u otros Proyectiles (Evita autodestruirse al nacer)
        if (collision.CompareTag("Enemigo") || 
            collision.CompareTag("Proyectil") || 
            collision.GetComponent<DronDisparador>() != null)
        {
            return;
        }

        // 2. Si choca con el jugador
        if (collision.CompareTag("Player"))
        {
            PlayerController jugador = collision.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.PerderVida();
            }

            Destroy(gameObject);
        }
        // 3. Si choca con paredes o plataformas sólidas
        else if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}