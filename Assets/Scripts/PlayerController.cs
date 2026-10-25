using UnityEngine;
using UnityEngine.InputSystem; 
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [Header("Sistema de Vidas")]
    public int vidas = 3;
    public GameObject menuGameOverUI;
    public TextMeshProUGUI textoVidasUI;
    private Vector2 posicionCheckpoint;

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
    public AudioClip musicaDerrota;
    public AudioClip sonidoMovimiento; 
    public AudioClip sonidoSlide; // Recuperamos el sonido del slide
    
    public AudioSource audioMovimiento;
    public AudioSource audioEfectos;

    [Header("Mecánica de Deslizamiento (Slide)")]
    public float velocidadSlide = 10f;       
    public float duracionSlide = 0.5f;       
    private bool estaDeslizando = false;     
    private float tiempoSiguienteSlide = 0f; 
    public float esperaSlide = 1f;

    [Header("Mecánica de Agacharse")]
    private bool estaAgachado = false; 

    [Header("Ajuste de Colisionadores (Agacharse y Slide)")]
    private BoxCollider2D miColisionador;    
    private Vector2 tamanoOriginalCollider;  
    private Vector2 offsetOriginalCollider;  
    public Vector2 tamanoSlideCollider = new Vector2(1f, 0.4f); 
    public Vector2 offsetSlideCollider = new Vector2(0f, -0.3f); 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        
        posicionCheckpoint = transform.position;
        
        miColisionador = GetComponent<BoxCollider2D>();
        if (miColisionador != null)
        {
            tamanoOriginalCollider = miColisionador.size;
            offsetOriginalCollider = miColisionador.offset;
        }
        
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
        ActualizarUIVidas();
    }

    void Update()
    {
        if (MenuPausa.juegoPausado) return;
        if (estaMuerto) return;
        if (Keyboard.current == null) return;
        
        // Si está en medio de un slide, ignoramos el resto de los controles hasta que termine
        if (estaDeslizando) return; 

        // DETECTAR TECLAS
        bool teclaAbajo = Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed;
        bool teclaDerecha = Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed;
        bool teclaIzquierda = Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed;
        bool teclaLateral = teclaDerecha || teclaIzquierda;

        // 1. LÓGICA DE AGACHARSE Y DESLIZAMIENTO
        if (teclaAbajo && enSuelo)
        {
            if (teclaLateral && Time.time >= tiempoSiguienteSlide)
            {
                // Si presionó ABAJO + LADO -> SLIDE
                if (estaAgachado) CambiarEstadoAgachado(false); // Se levanta rápido si estaba agachado
                
                // Aseguramos hacia dónde mira antes de deslizarse
                if (teclaDerecha) spriteRenderer.flipX = false;
                else if (teclaIzquierda) spriteRenderer.flipX = true;

                StartCoroutine(EjecutarSlide());
                return;
            }
            else if (!teclaLateral)
            {
                // Si presionó SOLO ABAJO -> SE AGACHA
                if (!estaAgachado) CambiarEstadoAgachado(true);
            }
        }
        else if (estaAgachado)
        {
            // Si soltó la tecla abajo, se levanta
            CambiarEstadoAgachado(false);
        }

        // 2. SI ESTÁ AGACHADO, NO SE MUEVE
        if (estaAgachado)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            animator.SetBool("isRunning", false);
            if (audioMovimiento != null && audioMovimiento.isPlaying) audioMovimiento.Pause();
            return; 
        }

        // 3. MOVIMIENTO NORMAL
        float movimientoX = 0f;
        if (teclaDerecha) movimientoX = 1f;
        else if (teclaIzquierda) movimientoX = -1f;

        rb.linearVelocity = new Vector2(movimientoX * velocidad, rb.linearVelocity.y);

        if (movimientoX < 0) spriteRenderer.flipX = true;
        else if (movimientoX > 0) spriteRenderer.flipX = false;

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

        // 4. SALTO
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

    private void CambiarEstadoAgachado(bool agachar)
    {
        estaAgachado = agachar;

        if (agachar)
        {
            if (animator != null) animator.SetBool("isSliding", true); 
            if (miColisionador != null)
            {
                miColisionador.size = tamanoSlideCollider;
                miColisionador.offset = offsetSlideCollider;
            }
        }
        else
        {
            if (animator != null) animator.SetBool("isSliding", false);
            if (miColisionador != null)
            {
                miColisionador.size = tamanoOriginalCollider;
                miColisionador.offset = offsetOriginalCollider;
            }
        }
    }

    private IEnumerator EjecutarSlide()
    {
        estaDeslizando = true;
        tiempoSiguienteSlide = Time.time + esperaSlide;

        if (animator != null) animator.SetBool("isSliding", true);

        if (audioEfectos != null && sonidoSlide != null)
        {
            audioEfectos.PlayOneShot(sonidoSlide);
        }

        if (miColisionador != null)
        {
            miColisionador.size = tamanoSlideCollider;
            miColisionador.offset = offsetSlideCollider;
        }

        float direccion = spriteRenderer.flipX ? -1f : 1f;
        rb.linearVelocity = new Vector2(direccion * velocidadSlide, rb.linearVelocity.y);

        yield return new WaitForSeconds(duracionSlide);

        if (miColisionador != null)
        {
            miColisionador.size = tamanoOriginalCollider;
            miColisionador.offset = offsetOriginalCollider;
        }

        if (animator != null) animator.SetBool("isSliding", false);
        estaDeslizando = false;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("plataforma")) enSuelo = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstaculo") && !estaMuerto)
        {
            PerderVida();
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("plataforma")) enSuelo = false;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Meta") && !estaMuerto)
        {
            StartCoroutine(SecuenciaVictoria());
        }
    }

    public void PerderVida()
    {
        vidas--;
        ActualizarUIVidas(); 
        
        if (vidas > 0)
        {
            StartCoroutine(SecuenciaRespawn());
        }
        else
        {
            StartCoroutine(SecuenciaMuerte());
        }
    }

    private IEnumerator SecuenciaRespawn()
    {
        estaMuerto = true; 
        
        if (audioMovimiento != null) audioMovimiento.Stop();
        if (audioEfectos != null && sonidoMuerte != null) audioEfectos.PlayOneShot(sonidoMuerte);

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false; 
        
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        int ordenOriginal = sr.sortingOrder; 
        sr.sortingOrder = -1; 

        rb.constraints = RigidbodyConstraints2D.None; 
        rb.AddTorque(150f); 
        rb.linearVelocity = new Vector2(0f, 8f); 

        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isSliding", false);
            animator.SetBool("isDead", true); 
        }

        yield return new WaitForSeconds(2f);

        rb.linearVelocity = Vector2.zero; 
        rb.angularVelocity = 0f; 
        transform.rotation = Quaternion.identity; 
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; 
        
        if (col != null) col.enabled = true; 
        sr.sortingOrder = ordenOriginal; 
        
        if (animator != null) animator.SetBool("isDead", false); 
        
        transform.position = posicionCheckpoint;
        estaMuerto = false; 
    }

    private IEnumerator SecuenciaMuerte()
    {
        estaMuerto = true;
        if (audioMovimiento != null) audioMovimiento.Stop();
        if (audioEfectos != null && sonidoMuerte != null) audioEfectos.PlayOneShot(sonidoMuerte);

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
            animator.SetBool("isSliding", false);
            animator.SetBool("isDead", true);
        }

        yield return new WaitForSeconds(2f);
        
        AudioListener.pause = true; 

        if (audioEfectos != null && musicaDerrota != null)
        {
            audioEfectos.ignoreListenerPause = true; 
            audioEfectos.PlayOneShot(musicaDerrota);
        }

        if (menuGameOverUI != null) menuGameOverUI.SetActive(true);
        Time.timeScale = 0f; 
    }

    private void ActualizarUIVidas()
    {
        if (textoVidasUI != null) textoVidasUI.text = "x " + vidas.ToString();
    }

    private IEnumerator SecuenciaVictoria()
    {
        if (textoVictoria != null) textoVictoria.SetActive(true);
        if (audioMovimiento != null) audioMovimiento.Stop();
        
        rb.linearVelocity = Vector2.zero;
        if(animator != null) animator.SetBool("isRunning", false); 
        this.enabled = false; 

        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MenuPrincipal");
    }

    public void ReiniciarNivel()
    {
        AudioListener.pause = false; 
        Time.timeScale = 1f; 
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void IrAlMenuPrincipal()
    {
        AudioListener.pause = false; 
        Time.timeScale = 1f; 
        SceneManager.LoadScene("MenuPrincipal"); 
    }
}