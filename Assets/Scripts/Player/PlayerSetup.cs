using UnityEngine;
using Mirror;


[RequireComponent(typeof(Player))]
[RequireComponent(typeof(PlayerController))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;

    [SerializeField]
    private string remoteLayerName = "RemotePlayer";

    [SerializeField]
    private string dontDrawLayerName = "DontDraw";

    [SerializeField]
    private GameObject playerGraphics;

    [SerializeField]
    private GameObject playerUIPrefab;
    private GameObject playerUIInstance;



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

            //Desactive les graphics du joueur local pour eviter les bugs de camera
            Util.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Instancie le UI du joueur local
            playerUIInstance = Instantiate(playerUIPrefab); 

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No PlayerUI component found on playerUIInstance.");
            }
            else
            {
                ui.SetController(GetComponent<PlayerController>()); // Associe le PlayerController au PlayerUI pour permettre la mise à jour de l'interface utilisateur
            }    
        }

        GetComponent<Player>().Setup();
    }


    public override void OnStartClient()
    {
        base.OnStartClient();

        string netId = GetComponent<NetworkIdentity>().netId.ToString();
        Player player = GetComponent<Player>();

        GameManager.RegisterPlayer(netId, player);
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
        Destroy(playerUIInstance);
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }

        GameManager.UnregisterPlayer(transform.name); 
    }
}
