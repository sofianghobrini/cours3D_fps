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
    private GameObject playerNamePlatesGraphics;


    [SerializeField]
    private GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUIInstance;


    //Permet de desactiver les joueurs qui joue sur leurs propres pc ou console
    private void Start()
    {
        if(!isLocalPlayer){
            DisableComponents();
            //On desactive la camera de la scene pour les autres joueurs
            AssingRemoteLayer();
        }
        else
        {

            //Desactive les graphics du joueur local pour eviter les bugs de camera
            Util.SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));
            Util.SetLayerRecursively(playerNamePlatesGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Instancie le UI du joueur local
            playerUIInstance = Instantiate(playerUIPrefab); 

            PlayerUI ui = playerUIInstance.GetComponent<PlayerUI>();
            if (ui == null)
            {
                Debug.LogError("No PlayerUI component found on playerUIInstance.");
            }
            else
            {
                ui.SetPlayer(GetComponent<Player>()); // Associe le Player au PlayerUI pour permettre la mise à jour de l'interface utilisateur
            } 


            GetComponent<Player>().Setup(); 


            string username = UserAccountManager.LoggedInUsername;

            CmdSetUsername(transform.name, username); 
        }

        
    }

    [Command]
    private void CmdSetUsername(string playerId, string _username)
    {
        Player player = GameManager.GetPlayerId(playerId);
        if (player != null)
        {
            Debug.Log(_username + " a rejoint la partie.");
            player.username = _username;
        }
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


        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
        }
        

        GameManager.UnregisterPlayerId(transform.name); 
    }
}
