using UnityEngine;
using UnityEngine.UI;

public class PersonalizacionPersonaje : MonoBehaviour
{
    public GameObject personaje;
    public Material[] colores;
    public GameObject[] capas;
    public GameObject[] sombreros;
    public GameObject[] caras;
    public GameObject[] pajaritas;
    public GameObject[] alas;

    private int indexColor = 0;
    private int indexCapa = 0;
    private int indexSombrero = 0;
    private int indexCara = 0;
    private int indexPajarita = 0;
    private int indexAlas = 0;

    public Button botonColor;
    public Button botonCapa;
    public Button botonSombrero;
    public Button botonCara;
    public Button botonPajarita;
    public Button botonAlas;
    public Button flechaIzquierda;
    public Button flechaDerecha;

    private enum TipoPersonalizacion { Color, Capa, Sombrero, Cara, Pajarita, Alas }
    private TipoPersonalizacion tipoSeleccionado;

    void Start()
    {
        // Establecer los listeners para los botones
        botonColor.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Color));
        botonCapa.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Capa));
        botonSombrero.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Sombrero));
        botonCara.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Cara));
        botonPajarita.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Pajarita));
        botonAlas.onClick.AddListener(() => SeleccionarPersonalizacion(TipoPersonalizacion.Alas));
        flechaIzquierda.onClick.AddListener(CambiarElementoIzquierda);
        flechaDerecha.onClick.AddListener(CambiarElementoDerecha);

        ActualizarAccesorios();
    }

    void SeleccionarPersonalizacion(TipoPersonalizacion tipo)
    {
        tipoSeleccionado = tipo;
        ActualizarAccesorios();
    }

    void ActualizarAccesorios()
    {
        foreach (GameObject capa in capas)
        {
            capa.SetActive(false);
        }

        foreach (GameObject sombrero in sombreros)
        {
            sombrero.SetActive(false);
        }

        foreach (GameObject cara in caras)
        {
            cara.SetActive(false);
        }

        foreach (GameObject pajarita in pajaritas)
        {
            pajarita.SetActive(false);
        }

        foreach (GameObject ala in alas)
        {
            ala.SetActive(false);
        }

        if (capas.Length > 0)
        {
            capas[indexCapa].SetActive(true);
        }

        if (sombreros.Length > 0)
        {
            sombreros[indexSombrero].SetActive(true);
        }

        if (caras.Length > 0)
        {
            caras[indexCara].SetActive(true);
        }

        if (pajaritas.Length > 0)
        {
            pajaritas[indexPajarita].SetActive(true);
        }

        if (alas.Length > 0)
        {
            alas[indexAlas].SetActive(true);
        }

        if (colores.Length > 0 && personaje != null)
        {
            personaje.GetComponent<Renderer>().material = colores[indexColor];
        }
    }

    void CambiarElementoIzquierda()
    {
        if (tipoSeleccionado == TipoPersonalizacion.Color)
        {
            indexColor = (indexColor - 1 + colores.Length) % colores.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Capa)
        {
            indexCapa = (indexCapa - 1 + capas.Length) % capas.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Sombrero)
        {
            indexSombrero = (indexSombrero - 1 + sombreros.Length) % sombreros.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Cara)
        {
            indexCara = (indexCara - 1 + caras.Length) % caras.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Pajarita)
        {
            indexPajarita = (indexPajarita - 1 + pajaritas.Length) % pajaritas.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Alas)
        {
            indexAlas = (indexAlas - 1 + alas.Length) % alas.Length;
            ActualizarAccesorios();
        }
    }

    void CambiarElementoDerecha()
    {
        if (tipoSeleccionado == TipoPersonalizacion.Color)
        {
            indexColor = (indexColor + 1) % colores.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Capa)
        {
            indexCapa = (indexCapa + 1) % capas.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Sombrero)
        {
            indexSombrero = (indexSombrero + 1) % sombreros.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Cara)
        {
            indexCara = (indexCara + 1) % caras.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Pajarita)
        {
            indexPajarita = (indexPajarita + 1) % pajaritas.Length;
            ActualizarAccesorios();
        }
        else if (tipoSeleccionado == TipoPersonalizacion.Alas)
        {
            indexAlas = (indexAlas + 1) % alas.Length;
            ActualizarAccesorios();
        }
    }
}
