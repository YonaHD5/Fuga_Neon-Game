using UnityEngine;

public class PlataformaMovil2 : MonoBehaviour
{
    [Header("Puntos de Trayectoria")]
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 3f;

    [Header("Comportamiento")]
    [Tooltip("La plataforma espera a que el jugador la pise para avanzar")]
    public bool activarAlPisar = true;
    [Tooltip("Va y viene continuamente mientras esté activa")]
    public bool irYVenir = true;

    [Header("Reinicio tras Checkpoint / Caída")]
    [Tooltip("Segundos sin el jugador encima antes de volver al Punto A")]
    public float tiempoParaRegresar = 1.5f;

    private Vector3 posInicial;
    private Vector3 posFinal;
    private Vector3 objetivoActual;
    private bool estaEnMovimiento = false;
    private bool jugadorEncima = false;
    private float temporizadorAbandono = 0f;
    private Transform jugadorTransform;

    void Awake()
    {
        // Guardamos las posiciones fijas en el mundo para que nunca se pierdan
        posInicial = (puntoA != null) ? puntoA.position : transform.position;
        posFinal = (puntoB != null) ? puntoB.position : transform.position;
    }

    void Start()
    {
        ReiniciarPlataforma();
    }

    void Update()
    {
        if (puntoA == null || puntoB == null) return;

        // Si el jugador murió y reapareció lejos en un checkpoint, soltarlo y reiniciar plataforma
        if (jugadorEncima && jugadorTransform != null)
        {
            if (Vector2.Distance(transform.position, jugadorTransform.position) > 3.5f)
            {
                if (jugadorTransform.parent == transform)
                {
                    jugadorTransform.SetParent(null);
                }
                ReiniciarPlataforma();
                return;
            }
        }

        // Si el jugador saltó fuera, contar para regresar sola al inicio
        if (activarAlPisar && !jugadorEncima && estaEnMovimiento)
        {
            temporizadorAbandono += Time.deltaTime;
            if (temporizadorAbandono >= tiempoParaRegresar)
            {
                objetivoActual = posInicial;
            }
        }

        if (!estaEnMovimiento) return;

        // Mover hacia el objetivo actual
        transform.position = Vector3.MoveTowards(transform.position, objetivoActual, velocidad * Time.deltaTime);

        // Al llegar a uno de los extremos
        if (Vector3.Distance(transform.position, objetivoActual) < 0.05f)
        {
            if (objetivoActual == posInicial && !jugadorEncima && activarAlPisar)
            {
                estaEnMovimiento = false;
                objetivoActual = posFinal;
            }
            else if (irYVenir)
            {
                objetivoActual = (objetivoActual == posInicial) ? posFinal : posInicial;
            }
            else
            {
                estaEnMovimiento = false;
            }
        }
    }

    // Regresa la plataforma de inmediato a su posición de origen
    public void ReiniciarPlataforma()
    {
        transform.position = posInicial;
        objetivoActual = posFinal;
        jugadorEncima = false;
        jugadorTransform = null;
        temporizadorAbandono = 0f;
        estaEnMovimiento = !activarAlPisar;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            jugadorTransform = collision.transform;
            collision.transform.SetParent(transform);
            jugadorEncima = true;
            temporizadorAbandono = 0f;

            if (activarAlPisar)
            {
                objetivoActual = posFinal;
                estaEnMovimiento = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.gameObject.activeInHierarchy && collision.transform.parent == transform)
            {
                collision.transform.SetParent(null);
            }
            jugadorEncima = false;
            temporizadorAbandono = 0f;
        }
    }

    private void OnDrawGizmos()
    {
        if (puntoA != null && puntoB != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(puntoA.position, puntoB.position);
            Gizmos.DrawWireSphere(puntoA.position, 0.3f);
            Gizmos.DrawWireSphere(puntoB.position, 0.3f);
        }
    }
}