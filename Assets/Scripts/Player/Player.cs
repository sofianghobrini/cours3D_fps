using UnityEngine;
using Mirror;
using System.Collections;
using NUnit.Framework;


[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;

    public bool isDead
    {
        get { return _isDead;}
        protected set {_isDead = value;}
    }

    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar] // Permet de synchroniser cette variable entre le serveur et les clients  
    private float currentHealth;

    public float GetHealthPct()
    {
        return currentHealth / maxHealth; // Retourne le pourcentage de santé actuel du joueur
    }

    public int kills;
    public int deaths;


    [SerializeField]
    private Behaviour[] disableOnDeath;


    [SerializeField]
    private GameObject[] disableOnGameObjectsOnDeath;
    private bool[] wasEnabledOnStart;

    [SerializeField]
    private GameObject deathEffect;

    [SerializeField]
    private GameObject respawnEffect;

    private bool firstSetup = true;

    public void Setup()
    {
        if(isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(false); // Désactive la caméra de la scène pour le joueur local après sa mort
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(true); // Désactive l'UI du joueur local après sa mort
        }
        

        CmdBroadCastNewPlayerSetup();
    }

    [Command(requiresAuthority = true)] // Permet d'appeler cette méthode depuis un client vers le serveur, même si le client n'a pas l'autorité sur cet objet
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }


    [ClientRpc]
    private void RpcSetupPlayerOnAllClients()
    {
        if(firstSetup)
        {
            wasEnabledOnStart = new bool[disableOnDeath.Length];
            for (int i = 0; i < disableOnDeath.Length; i++)
            {
                wasEnabledOnStart[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }

        SetDefaults();
    }

    public void SetDefaults()
    {
        isDead = false;
        currentHealth = maxHealth;


        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabledOnStart[i];
        }

        // réactive les GameObjects spécifiés dans disableOnGameObjectsOnDeath pour simuler la mort du joueur
        for (int i = 0; i < disableOnGameObjectsOnDeath.Length; i++)
        {
            disableOnGameObjectsOnDeath[i].SetActive(true);
        }


        // Réactive le collider du joueur pour permettre les interactions physiques après la réapparition
        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }



        //Apparit une particule d'effet de mort à la position du joueur
        GameObject _gfxIns = Instantiate(respawnEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f);
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);

        
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);

        Setup();
    }


    private void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(45f, "Test");
        }
    }


    [ClientRpc] // Permet d'appeler cette méthode sur tous les clients depuis le serveur
    public void RpcTakeDamage(float amount, string sourceId)
    {
        if (isDead)
        {
            return;
        }
            
        currentHealth -= amount;
        //Debug.Log(transform.name + " a " + currentHealth + " points de vie restants.");
        if(currentHealth <= 0 && !isDead)
        {
            Die(sourceId);
        }
    }
    
    private void Die(string sourceId)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayerId(sourceId);
        

        if(sourcePlayer != null)
        {
            sourcePlayer.kills++;
            if(GameManager.instance.onPlayerKilledCallback != null)
            {
                if (isServer)
                {
                    GameManager.instance.onPlayerKilledCallback(transform.name, sourcePlayer.transform.name);
                }
            }
            else
            {
                Debug.LogError("GameManager instance onPlayerKilledCallback est null !");
            }
        }

        

        
        deaths++;

        // Désactive les composants spécifiés dans disableOnDeath pour simuler la mort du joueur
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }


        // Désactive les GameObjects spécifiés dans disableOnGameObjectsOnDeath pour simuler la mort du joueur
        for (int i = 0; i < disableOnGameObjectsOnDeath.Length; i++)
        {
            disableOnGameObjectsOnDeath[i].SetActive(false);
        }

        //Désactive le collider du joueur pour éviter les interactions physiques après la mort
        Collider col = GetComponent<Collider>();
        if (col != null)        
        {
            col.enabled = false;
        }

        //Apparit une particule d'effet de mort à la position du joueur
        GameObject _gfxIns = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gfxIns, 3f); // Détruit l'effet de mort après 3 secondes pour éviter d'encombrer la scène

        Debug.Log(transform.name + " est mort.");
        

        // Change la caméra pour le joueur local après sa mort
        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true); // Désactive la caméra de la scène pour le joueur local après sa mort
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false); // Désactive l'UI du joueur local après sa mort
            StartCoroutine(Respawn());
        }
    }

}
