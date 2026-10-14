using UnityEngine;
using TMPro;

public class TituloNeon : MonoBehaviour
{
    private TextMeshProUGUI texto;
    
    // Lista de colores neón que irán rotando
    private Color[] coloresNeon = new Color[] { Color.cyan, Color.magenta, Color.green, Color.yellow };
    public float velocidadTransicion = 1.5f;

    private int indiceActual = 0;
    private int indiceSiguiente = 1;
    private float tiempo = 0f;

    void Start()
    {
        texto = GetComponent<TextMeshProUGUI>();
        texto.color = Color.black; // Aseguramos que el centro siga negro
    }

    void Update()
    {
        tiempo += Time.deltaTime * velocidadTransicion;

        // Calcula la mezcla suave entre el color actual y el siguiente
        Color colorAnimado = Color.Lerp(coloresNeon[indiceActual], coloresNeon[indiceSiguiente], tiempo);

        // Aplica ese color únicamente al borde (Outline) de las letras
        texto.fontMaterial.SetColor("_OutlineColor", colorAnimado);

        // Cuando la transición llega al 100%, pasamos al siguiente color en la lista
        if (tiempo >= 1f)
        {
            tiempo = 0f;
            indiceActual = indiceSiguiente;
            // El "% coloresNeon.Length" hace que cuando llegue al final, vuelva a empezar por el cian
            indiceSiguiente = (indiceSiguiente + 1) % coloresNeon.Length; 
        }
    }
}