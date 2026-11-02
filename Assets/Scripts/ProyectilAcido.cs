using UnityEngine;

public class ProyectilAcido : MonoBehaviour
{
    [Header("Configuración")]
    public float velocidad = 6f;
    public float tiempoVida = 4f;

    private Vector2 direccion = Vector2.left;

    public void SetDireccion(Vector2 dir)
    {
        direccion = dir.normalized;
    }

    private void Start()
    {
        // Se autodestruye tras unos segundos si no choca con nada
        Destroy(gameObject, tiempoVida);
    }

    private void Update()
    {
        transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Si toca al gato, le quita vida y se destruye
        if (collider.CompareTag("Player"))
        {
            PlayerController jugador = collider.GetComponent<PlayerController>();
            if (jugador != null)
            {
                jugador.PerderVida();
            }
            Destroy(gameObject);
        }
        // Si choca contra el suelo, plataformas o paredes, desaparece
        else if (collider.CompareTag("Suelo") || collider.CompareTag("plataforma") || collider.CompareTag("Pared"))
        {
            Destroy(gameObject);
        }
    }
}