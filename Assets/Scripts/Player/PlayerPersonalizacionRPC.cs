using Photon.Pun;
using UnityEngine;

public class PlayerPersonalizacionRPC : MonoBehaviourPun
{
    private PersonalizacionPersonaje personalizacion;

    private void Awake()
    {
        personalizacion = GetComponent<PersonalizacionPersonaje>();
    }

    private void Start()
    {
        if (photonView.IsMine)
        {
            personalizacion.CargarPersonalizacion();
            EnviarPersonalizacion();
        }
    }

    public void EnviarPersonalizacion()
    {
        var datos = personalizacion.GetDatosActuales();

        photonView.RPC("AplicarPersonalizacionRPC", RpcTarget.Others,
            datos.indexColor, datos.indexCapa, datos.indexSombrero, 
            datos.indexCara, datos.indexPajarita, datos.indexAlas);
    }

    [PunRPC]
    void AplicarPersonalizacionRPC(int color, int capa, int sombrero, int cara, int pajarita, int alas)
    {
        Debug.Log("Recibiendo personalización por RPC");

        personalizacion.indexColor = color;
        personalizacion.indexCapa = capa;
        personalizacion.indexSombrero = sombrero;
        personalizacion.indexCara = cara;
        personalizacion.indexPajarita = pajarita;
        personalizacion.indexAlas = alas;

        personalizacion.ActualizarAccesorios();
    }
}
