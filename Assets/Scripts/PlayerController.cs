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

    // NUEVA VARIABLE: Para saber si el jugador perdió y bloquear controles
    private bool estaMuerto = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // NUEVO: Si está muerto, ignoramos todo el código de movimiento de abajo
        if (estaMuerto) return;

        if (Keyboard.current == null) return;

        float movimientoX = 0f;
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
            movimientoX = 1f;
        else if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
            movimientoX = -1f;

        rb.linearVelocity = new Vector2(movimientoX * velocidad, rb.linearVelocity.y);

        if (movimientoX < 0) 
        {
            spriteRenderer.flipX = true;
        } 
        else if (movimientoX > 0) 
        {
            spriteRenderer.flipX = false;
        }

        if (movimientoX != 0) 
        {
            animator.SetBool("isRunning", true);
        } 
        else 
        {
            animator.SetBool("isRunning", false);
        }

        bool intentoSalto = Keyboard.current.spaceKey.wasPressedThisFrame || Keyboard.current.upArrowKey.wasPressedThisFrame;
        
        if (intentoSalto && enSuelo)
        {
            rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
        }

        animator.SetBool("isJumping", !enSuelo);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Suelo")) enSuelo = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // MODIFICADO: Ahora llama a la animación de muerte en lugar de reiniciar de golpe
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

    // NUEVA FUNCIÓN: Secuencia de muerte estilo Mario
    private IEnumerator SecuenciaMuerte()
    {
        estaMuerto = true;

        // NUEVO: Congela la cámara definitivamente
        // NUEVO: Busca tu script CameraFollow en la cámara principal y lo apaga
        if (Camera.main != null)
        {
            CameraFollow scriptCamara = Camera.main.GetComponent<CameraFollow>();
            if (scriptCamara != null)
            {
                scriptCamara.enabled = false; // ¡Esto congela la cámara al instante!
            }
        }

        // 1. Apagamos su colisión para que caiga al vacío
        GetComponent<Collider2D>().enabled = false;
        
        // Mueve el sprite a una capa trasera para que el piso lo tape al caer
        GetComponent<SpriteRenderer>().sortingOrder = -1;

        // 2. Permite que el Rigidbody rote libremente y dale un empujón de giro (Debe ir aquí arriba)
        rb.constraints = RigidbodyConstraints2D.None; 
        rb.AddTorque(150f); 

        // 3. Lo frenamos en seco hacia adelante, pero le damos un "saltito" hacia arriba
        rb.linearVelocity = new Vector2(0f, 8f);

        // 4. Apagamos las otras animaciones y encendemos la de muerte
        if (animator != null)
        {
            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isDead", true);
        }

        // 5. Esperamos 2 segundos para ver todo el efecto de la caída
        yield return new WaitForSeconds(2f);

        // 6. Reiniciamos la escena (¡ESTO SIEMPRE VA AL FINAL!)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator SecuenciaVictoria()
    {
        if (textoVictoria != null)
        {
            textoVictoria.SetActive(true);
        }

        rb.linearVelocity = Vector2.zero;
        if(animator != null) animator.SetBool("isRunning", false); 
        this.enabled = false; 

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene("Nivel2");
    }
}