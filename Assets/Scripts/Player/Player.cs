using UnityEngine;
using Mirror;
using System.Runtime.InteropServices;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private float maxHealth = 100f;

    [SyncVar] // Permet de synchroniser cette variable entre le serveur et les clients  
    private float currentHealth;

    private void Awake()
    {
        SetDefaults();
    }

    public void SetDefaults()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        Debug.Log(transform.name + " a " + currentHealth + " points de vie restants.");
    }
    

}
