using UnityEngine;

public class DronRayo : MonoBehaviour
{
    public enum DireccionRayo { Izquierda, Derecha, Arriba, Abajo }

    [Header("Configuración de Dirección")]
    [Tooltip("Elige hacia qué lado saldrá disparado el láser")]
    public DireccionRayo direccion = DireccionRayo.Derecha;
    public float distanciaMaxima = 12f;

    [Header("Referencias Visuales y Físicas")]
    public LayerMask capasImpacto; // Capas: Player y Plataformas/Paredes
    public LineRenderer lineRenderer;
    public Transform origenDisparo; // Si se deja vacío, usa la posición del Dron

    [Header("Estado del Laser")]
    public bool rayoActivo = true;

    private Vector2 vectorDireccion;

    void Start()
    {
        if (lineRenderer == null)
            lineRenderer = GetComponent<LineRenderer>();
    }

    void Update()
    {
        ActualizarVectorDireccion();

        if (rayoActivo)
        {
            ProcesarRayo();
        }
        else if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }
    }

    void ActualizarVectorDireccion()
    {
        switch (direccion)
        {
            case DireccionRayo.Izquierda: vectorDireccion = Vector2.left; break;
            case DireccionRayo.Derecha:   vectorDireccion = Vector2.right; break;
            case DireccionRayo.Arriba:    vectorDireccion = Vector2.up; break;
            case DireccionRayo.Abajo:     vectorDireccion = Vector2.down; break;
        }
    }

    void ProcesarRayo()
    {
        Vector2 origen = origenDisparo != null ? (Vector2)origenDisparo.position : (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origen, vectorDireccion, distanciaMaxima, capasImpacto);

        Vector2 puntoFinal = origen + (vectorDireccion * distanciaMaxima);

        if (hit.collider != null)
        {
            puntoFinal = hit.point;

            // Detectar si el rayo toca al jugador
            if (hit.collider.CompareTag("Player"))
            {
                // Reinicia al jugador o aplica daño
                hit.collider.GetComponent<PlayerController>()?.PerderVida();
            }
        }

        // Dibujar el láser con LineRenderer
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, origen);
            lineRenderer.SetPosition(1, puntoFinal);
        }
    }

    // Muestra una guía gráfica azul neón en la ventana Scene al seleccionar el Dron
    private void OnDrawGizmosSelected()
    {
        ActualizarVectorDireccion();
        Vector2 origen = origenDisparo != null ? (Vector2)origenDisparo.position : (Vector2)transform.position;
        
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(origen, origen + (vectorDireccion * distanciaMaxima));
        Gizmos.DrawWireSphere(origen + (vectorDireccion * distanciaMaxima), 0.2f);
    }
}