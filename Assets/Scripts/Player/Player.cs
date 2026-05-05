using UnityEngine;
using Mirror;
using System.Collections;

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

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabledOnStart;

    public void Setup()
    {
        wasEnabledOnStart = new bool[disableOnDeath.Length];
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            wasEnabledOnStart[i] = disableOnDeath[i].enabled;
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

        Collider col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTimer);

        SetDefaults();
        Transform spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = spawnPoint.position;
        transform.rotation = spawnPoint.rotation;
    }


    private void Update()
    {
        if(!isLocalPlayer)
        {
            return;
        }

        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(45f);
        }
    }


    [ClientRpc] // Permet d'appeler cette méthode sur tous les clients depuis le serveur
    public void RpcTakeDamage(float amount)
    {
        if (isDead)
        {
            return;
        }
            
        currentHealth -= amount;
        Debug.Log(transform.name + " a " + currentHealth + " points de vie restants.");
        if(currentHealth <= 0 && !isDead)
        {
            Die();
        }
    }
    
    private void Die()
    {
        isDead = true;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        if (col != null)        
        {
            col.enabled = false;
        }
        Debug.Log(transform.name + " est mort.");

        StartCoroutine(Respawn());
    }

}
