using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerPersonalizacionRPC : MonoBehaviourPun
{
    public void EnviarPersonalizacion(int PhotonId) //envia la personalitzacio al altre player amn els seus indexes
    {
        PersonalizacionPersonaje personalizacion = GetComponent<PersonalizacionPersonaje>();
        var datos = personalizacion.GetDatosActuales();

        personalizacion.indexColor = datos.indexColor;
        personalizacion.indexCapa = datos.indexCapa;
        personalizacion.indexSombrero = datos.indexSombrero;
        personalizacion.indexCara = datos.indexCara;
        personalizacion.indexPajarita = datos.indexPajarita;
        personalizacion.indexAlas = datos.indexAlas;

        int viewID = PhotonId; //player.GetComponent<PhotonView>().ViewID;

        photonView.RPC("AplicarPersonalizacionRPC", RpcTarget.Others,
            viewID, datos.indexColor, datos.indexCapa, datos.indexSombrero,
            datos.indexCara, datos.indexPajarita, datos.indexAlas);
    }

    [PunRPC]
    void AplicarPersonalizacionRPC(int PhotonId, int color, int capa, int sombrero, int cara, int pajarita, int alas) //actualitza la personalitzacio per veure al player que ho envia amb els seus accesoris corresponents
                                                                                                                                                  //(no es per aplicar-ho al player que ho rep, sino per veure al player que ho crida amnb els seus accessoris)
    {
        PhotonView view = PhotonView.Find(PhotonId);
        if (view == null)
        {
            Debug.LogWarning("PhotonView no trobat amb ID: " + view);
            return;
        }


        var item = view.GetComponent<PersonalizacionPersonaje>(); //agafa el script de personalitzacio del player que ha enviat la personalitzacio
        if (item == null)
        {
            Debug.LogWarning("No s'ha trobat el script PersonalizacionPersonaje al jugador");
            return;
        }

        item.indexColor = color;
        item.indexCapa = capa;
        item.indexSombrero = sombrero;
        item.indexCara = cara;
        item.indexPajarita = pajarita;
        item.indexAlas = alas;

        SpawnPlayers spawnPlayers = SpawnPlayers.instance;

        if (SpawnPlayers.instance != null)
        {
            SpawnPlayers.instance.AsignarAccesoriosDesdePrefab(view.gameObject, item);
        }
        else
        {
            Debug.LogError("SpawnPlayers.instance es nulo. No se puede asignar accesorios.");
        }

        Debug.Log("Asignant accesoris desde el prefab " + item.gameObject.name);

        item.ActualizarAccesorios();
        Debug.Log("Actualitzant accessoris " + item.gameObject.name);

    }
}
