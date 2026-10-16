using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public GameObject camaraPrincipal;
    public float velocidad = 5f;
    public float fuerzaSalto = 7f;
    private Rigidbody2D rb;
    private bool enSuelo;

    public GameObject textoVictoria; 

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    private bool estaMuerto = false;

    [Header("Sonidos")]
    public AudioClip sonidoSalto;
    public AudioClip sonidoMuerte;
    public AudioClip sonidoMovimiento; 
    
    // NUEVO: Dos reproductores separados para que no choquen
    public AudioSource audioMovimiento;
    public AudioSource audioEfectos;

    [Header("Mecánica de Deslizamiento (Slide)")]
    public float velocidadSlide = 10f;       // El impulso inicial del deslizamiento
    public float duracionSlide = 0.5f;       // Cuánto tiempo dura el gato barriéndose
    private bool estaDeslizando = false;     // Saber si el gato está en medio de un slide
    private float tiempoSiguienteSlide = 0f; // Tiempo de espera para volver a deslizarse (Cooldown)
    public float esperaSlide = 1f;            // 1 segundo de cooldown

    [Header("Ajuste de Colisionadores")]
    private BoxCollider2D miColisionador;    // Referencia al colisionador del gato
    private Vector2 tamanoOriginalCollider;  // Guarda el tamaño original del gato
    private Vector2 offsetOriginalCollider;  // Guarda la posición original del colisionador
    public Vector2 tamanoSlideCollider = new Vector2(1f, 0.4f); // Tamaño aplastado para el deslizamiento
    public Vector2 offsetSlideCollider = new Vector2(0f, -0.3f); // Mover el colisionador hacia el piso

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        // Inicializar el colisionador para el Slide
        miColisionador = GetComponent<BoxCollider2D>();
        if (miColisionador != null)
        {
            tamanoOriginalCollider = miColisionador.size;
            offsetOriginalCollider = miColisionador.offset;
        }
        
        // El código busca los dos Audio Sources automáticamente
        AudioSource[] misAudios = GetComponents<AudioSource>();
        if (misAudios.Length >= 2)
        {
            audioMovimiento = misAudios[0];
            audioEfectos = misAudios[1];
        }
        
        if (audioMovimiento != null) 
        {
            audioMovimiento.clip = sonidoMovimiento;
            audioMovimiento.loop = true; 
        }
    }

    void Update()
    {
        // Si el juego está pausado, salimos del Update y no leemos el teclado
        if (MenuPausa.juegoPausado) return;
        
        if (estaMuerto) return;

        // Si el gato se está deslizando, ignoramos el resto de los controles
        if (estaDeslizando) return;

        if (Keyboard.current == null) return;

        // Disparador del Slide (Tecla S o Flecha Abajo)
        bool quiereDeslizarse = Keyboard.current.sKey.wasPressedThisFrame || Keyboard.current.downArrowKey.wasPressedThisFrame;
        if (quiereDeslizarse && enSuelo && Time.time >= tiempoSiguienteSlide)
        {
            StartCoroutine(EjecutarSlide());
            return; // Salimos del Update este frame para que no interfiera el código de caminar
        }

        float movimientoX = 0f;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            movimientoX = 1f;
        else if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            movimientoX = -1f;

        rb.linearVelocity = new Vector2(movimientoX * velocidad, rb.linearVelocity.y);

        if (movimientoX < 0) spriteRenderer.flipX = true;
        else if (movimientoX > 0) spriteRenderer.flipX = false;

        // Lógica de sonido de movimiento
        if (movimientoX != 0) 
        {
            animator.SetBool("isRunning", true);
            if (enSuelo && audioMovimiento != null && !audioMovimiento.isPlaying)
            {
                audioMovimiento.Play();
            }
        } 
        else 
        {
            animator.SetBool("isRunning", false);
            if (audioMovimiento != null && audioMovimiento.isPlaying)
            {
                audioMovimiento.Pause(); 
            }
        }

        bool intentoSalto = Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame;
        
        if (intentoSalto && enSuelo)
        {
            rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
            
            if (audioEfectos != null && sonidoSalto != null)
            {
                audioEfectos.PlayOneShot(sonidoSalto);
            }
        }

        if (!enSuelo && audioMovimiento != null && audioMovimiento.isPlaying)
        {
            audioMovimiento.Pause();
        }

        animator.SetBool("isJumping", !enSuelo);
    }

    // NUEVO: Corrutina que ejecuta el Slide
    private IEnumerator EjecutarSlide()
    {
        estaDeslizando = true;
        tiempoSiguienteSlide = Time.time + esperaSlide;

        // Activamos la animación de Slide
        if (animator != null) animator.SetTrigger("isSliding");

        // Encogemos el colisionador
        if (miColisionador != null)
        {
            miColisionador.size = tamanoSlideCollider;
            miColisionador.offset = offsetSlideCollider;
        }

        // Impulso hacia adelante dependiendo de a dónde mire el gato
        float direccion = spriteRenderer.flipX ? -1f : 1f;
        rb.linearVelocity = new Vector2(direccion * velocidadSlide, rb.linearVelocity.y);

        // Esperamos a que termine el deslizamiento
        yield return new WaitForSeconds(duracionSlide);

        // Restauramos el tamaño original del colisionador
        if (miColisionador != null)
        {
            miColisionador.size = tamanoOriginalCollider;
            miColisionador.offset = offsetOriginalCollider;
        }

        estaDeslizando = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) enSuelo = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo") && !estaMuerto)
        {
            StartCoroutine(SecuenciaMuerte());
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) enSuelo = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Meta") && !estaMuerto)
        {
            StartCoroutine(SecuenciaVictoria());
        }
    }

    private IEnumerator SecuenciaMuerte()
    {
        estaMuerto = true;
        if (audioMovimiento != null) audioMovimiento.Stop();

        if (audioEfectos != null && sonidoMuerte != null)
        {
            audioEfectos.PlayOneShot(sonidoMuerte);
        }

        if (Camera.main != null)
        {
            CameraFollow scriptCamara = Camera.main.GetComponent<CameraFollow>();
            if (scriptCamara != null) scriptCamara.enabled = false; 
        }

        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().sortingOrder = -1;
        rb.constraints = RigidbodyConstraints2D.None; 
        rb.AddTorque(150f); 
        rb.linearVelocity = new Vector2(0f, 8f);

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDead", true);
        }

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SecuenciaVictoria()
    {
        if (textoVictoria != null) textoVictoria.SetActive(true);
        if (audioMovimiento != null) audioMovimiento.Stop();
        
        rb.linearVelocity = Vector2.zero;
        if(animator != null) animator.SetBool("isRunning", false); 
        this.enabled = false; 

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("Nivel2");
    }
}