using UnityEngine;
using Mirror;

[RequireComponent(typeof(WeaponManager))]
public class PlayerShoot : NetworkBehaviour
{

    private PlayerWeapon currentWeapon;


    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask; // Permet de spécifier les couches sur lesquelles le tir peut interagir
    
    private WeaponManager weaponManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false; // Désactive ce script si la caméra n'est pas assignée
        }

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        currentWeapon = weaponManager.GetCurrentWeapon();
        

        if(currentWeapon.fireRate <= 0f)
        {
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }
        }
        else
        {
            if(Input.GetButton("Fire1") )
            {
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.fireRate); // Appelle la fonction Shoot à une cadence définie par fireRate
            }
            else if(Input.GetButtonUp("Fire1"))
            {
                CancelInvoke("Shoot"); // Arrête d'appeler la fonction Shoot lorsque le bouton de tir est relâché
            }
        }
    }
    




    [Client]
    private void Shoot()
    {
        Debug.Log("Piou piou.");
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShoot(hit.transform.name, currentWeapon.damage); // Envoie une commande au serveur pour indiquer que le joueur a été touché
            }
        }
    }

    [Command]
    private void CmdPlayerShoot(string playerId, float damage)
    {
        Debug.Log(playerId + "a été touché.");

        Player player = GameManager.GetPlayer(playerId);
        player.RpcTakeDamage(damage);
    }
}
