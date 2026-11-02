using UnityEngine;

public class EnemigoPatrullaCoordenadas : MonoBehaviour
{
    [Header("Límites de la Plataforma (Coordenadas X)")]
    [Tooltip("Coordenada X del borde izquierdo de la plataforma")]
    public float limiteIzquierdoX = 8.5f;

    [Tooltip("Coordenada X del borde derecho de la plataforma")]
    public float limiteDerechoX = 13.0f;

    [Header("Configuración de Movimiento")]
    public float velocidad = 3f;

    private bool moviendoHaciaDerecha = true;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float posicionActualX = transform.position.x;

        if (moviendoHaciaDerecha)
        {
            // Avanza a la derecha
            transform.Translate(Vector2.right * velocidad * Time.deltaTime);

            // Al llegar o pasar el límite derecho, da la vuelta
            if (posicionActualX >= limiteDerechoX)
            {
                moviendoHaciaDerecha = false;
                ActualizarOrientacion();
            }
        }
        else
        {
            // Avanza a la izquierda
            transform.Translate(Vector2.left * velocidad * Time.deltaTime);

            // Al llegar o pasar el límite izquierdo, da la vuelta
            if (posicionActualX <= limiteIzquierdoX)
            {
                moviendoHaciaDerecha = true;
                ActualizarOrientacion();
            }
        }
    }

    private void ActualizarOrientacion()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = !moviendoHaciaDerecha;
        }
        else
        {
            Vector3 escala = transform.localScale;
            escala.x = moviendoHaciaDerecha ? Mathf.Abs(escala.x) : -Mathf.Abs(escala.x);
            transform.localScale = escala;
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Muestra dos líneas rojas verticales en la vista Scene para visualizar la zona de patrulla
        Gizmos.color = Color.red;
        float y = transform.position.y;
        float z = transform.position.z;

        Gizmos.DrawLine(new Vector3(limiteIzquierdoX, y - 0.5f, z), new Vector3(limiteIzquierdoX, y + 0.5f, z));
        Gizmos.DrawLine(new Vector3(limiteDerechoX, y - 0.5f, z), new Vector3(limiteDerechoX, y + 0.5f, z));
        Gizmos.DrawLine(new Vector3(limiteIzquierdoX, y, z), new Vector3(limiteDerechoX, y, z));
    }
}