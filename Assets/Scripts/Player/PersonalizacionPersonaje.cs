using UnityEngine;
using UnityEngine.UI;

public class PersonalizacionPersonaje : MonoBehaviour
{
    // Arrays de los accesorios (ya instanciados en la escena) 
    public GameObject[] capas; // Array de prefabs de capas (ya instanciadas en la escena)
    public GameObject[] sombreros; // Array de prefabs de sombreros (ya instanciados en la escena)

    private int indexCapa = 0; // Índice actual de la capa seleccionada
    private int indexSombrero = 0; // Índice actual del sombrero seleccionado

    public Button botonCapa; // Botón para seleccionar capa
    public Button botonSombrero; // Botón para seleccionar sombrero
    public Button flechaIzquierda; // Botón flecha izquierda
    public Button flechaDerecha; // Botón flecha derecha

    private enum TipoPersonalizacion { Capa, Sombrero }
    private TipoPersonalizacion tipoSeleccionado;

    void Start()
    {
        // Establecer los listeners para los botones
        botonCapa.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Capa));
        botonSombrero.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Sombrero));
        flechaIzquierda.onClick.AddListener(CambiarElementoIzquierda);
        flechaDerecha.onClick.AddListener(CambiarElementoDerecha);

        // Asegurarse de que los accesorios estén en su estado inicial (activados/desactivados)
        ActualizarAccesorios();
    }

    void SeleccionarPersonalizacion(TipoPersonalizacion tipo)
    {
        tipoSeleccionado = tipo;

        // Actualizar los accesorios según el tipo seleccionado
        ActualizarAccesorios();
    }

    void ActualizarAccesorios()
    {
        // Desactivar todas las capas y sombreros
        foreach (GameObject capa in capas)
        {
            capa.SetActive(false);
        }

        foreach (GameObject sombrero in sombreros)
        {
            sombrero.SetActive(false);
        }

        // Activar la capa y el sombrero correspondientes según los índices seleccionados
        if (capas.Length > 0)
        {
            capas[indexCapa].SetActive(true);
        }

        if (sombreros.Length > 0)
        {
            sombreros[indexSombrero].SetActive(true);
        }
    }

    void CambiarElementoIzquierda()
    {
        if (tipoSeleccionado == TipoPersonalizacion.Capa)
        {
            indexCapa = (indexCapa - 1 + capas.Length) % capas.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Sombrero)
        {
            indexSombrero = (indexSombrero - 1 + sombreros.Length) % sombreros.Length;
            ActualizarAccesorios();
        }
    }

    void CambiarElementoDerecha()
    {
        if (tipoSeleccionado == TipoPersonalizacion.Capa)
        {
            indexCapa = (indexCapa + 1) % capas.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Sombrero)
        {
            indexSombrero = (indexSombrero + 1) % sombreros.Length;
            ActualizarAccesorios();
        }
    }
}
