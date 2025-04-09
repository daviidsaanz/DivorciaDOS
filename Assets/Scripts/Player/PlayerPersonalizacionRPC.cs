using Photon.Pun;
using UnityEngine;

public class PlayerPersonalizacionRPC : MonoBehaviourPun
{
    private PersonalizacionPersonaje personalizacion;

    private void Awake()
    {
        string tag = gameObject.tag;
        if (tag == "Player1")
            personalizacion = GameObject.FindGameObjectWithTag("Player2").GetComponent<PersonalizacionPersonaje>();
        else if (tag == "Player2")
            personalizacion = GameObject.FindGameObjectWithTag("Player1").GetComponent<PersonalizacionPersonaje>();
    }

    public void EnviarPersonalizacion()
    {
        var personalizacionMia = GetComponent<PersonalizacionPersonaje>();
        var datos = personalizacionMia.GetDatosActuales();
        photonView.RPC("AplicarPersonalizacionRPC", RpcTarget.AllBuffered,
            datos.indexColor, datos.indexCapa, datos.indexSombrero, datos.indexCara, datos.indexPajarita, datos.indexAlas);
    }

    [PunRPC]
    void AplicarPersonalizacionRPC(int color, int capa, int sombrero, int cara, int pajarita, int alas)
    {
        Debug.Log("🔁 Recibiendo personalización por RPC");

        personalizacion.indexColor = color;
        personalizacion.indexCapa = capa;
        personalizacion.indexSombrero = sombrero;
        personalizacion.indexCara = cara;
        personalizacion.indexPajarita = pajarita;
        personalizacion.indexAlas = alas;

        personalizacion.ActualizarAccesorios();
    }
}
