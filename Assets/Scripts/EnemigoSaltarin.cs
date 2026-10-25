using UnityEngine;

public class EnemigoSaltarin : MonoBehaviour
{
    [Header("Configuración de Salto")]
    public float fuerzaSalto = 6f;       
    public float intervaloSalto = 2f;    
    private float cronometroSalto;

    [Header("Movimiento y Límites")]
    public float velocidad = 2f;
    private int direccion = 1; // 1 para derecha, -1 para izquierda
    
    [Header("Coordenadas de la Plataforma")]
    public float limiteIzquierdo = 12f; // Posición X mínima donde gira
    public float limiteDerecho = 22f;   // Posición X máxima donde gira

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    [Header("Detección de Suelo")]
    public Transform controladorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask queEsSuelo;
    private bool enSuelo;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        cronometroSalto = intervaloSalto;
    }

    void Update()
    {
        if (controladorSuelo != null)
        {
            LayerMask capaFinal = (queEsSuelo == 0) ? ~0 : queEsSuelo;
            enSuelo = Physics2D.OverlapCircle(controladorSuelo.position, radioSuelo, capaFinal);
        }

        cronometroSalto -= Time.deltaTime;

        if (cronometroSalto <= 0f && enSuelo)
        {
            Saltar();
            cronometroSalto = intervaloSalto; 
        }

        // LÍMITES INFALIBLES: Si toca el extremo izquierdo o derecho, gira obligatoriamente
        if (transform.position.x <= limiteIzquierdo)
        {
            direccion = 1; // Va hacia la derecha
        }
        else if (transform.position.x >= limiteDerecho)
        {
            direccion = -1; // Va hacia la izquierda
        }
    }

    void FixedUpdate()
    {
        if (rb == null) return;

        rb.linearVelocity = new Vector2(direccion * velocidad, rb.linearVelocity.y);

        if (spriteRenderer != null)
        {
            spriteRenderer.flipX = (direccion < 0);
        }
    }

    void Saltar()
    {
        rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
    }
}