using UnityEngine;
using Mirror;

public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";


    Camera sceneCamera;

    //Permet de desactiver les joueurs qui joue sur leurs propres pc ou console
    private void Start(){
        if(!isLocalPlayer){
            DisableComponents();
            //On desactive la camera de la scene pour les autres joueurs
            AssingRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }      
        }

        RegisterPlayer();
    }

    private void RegisterPlayer(){
        //On utilise le netId du joueur pour le nommer dans la hiérarchie de Unity, ce qui permet de les différencier facilement
        string playerName = GetComponent<NetworkIdentity>().netId.ToString();
        transform.name = playerName;
    }


    private void DisableComponents(){
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }


    private void AssingRemoteLayer(){
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }


    private void OnDisable(){
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
    }
}
