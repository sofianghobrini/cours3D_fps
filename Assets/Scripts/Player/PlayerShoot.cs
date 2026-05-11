using UnityEngine;
using Mirror;
public class PlayerShoot : NetworkBehaviour
{

    [SerializeField]
    private PlayerWeapon weapon;

    [SerializeField]
    private GameObject weaponGFX;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private LayerMask mask; // Permet de spécifier les couches sur lesquelles le tir peut interagir
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(cam == null)
        {
            Debug.LogError("PlayerShoot: No camera referenced!");
            this.enabled = false; // Désactive ce script si la caméra n'est pas assignée
        }

        weaponGFX.layer = LayerMask.NameToLayer(weaponLayerName); // Assigne la couche spécifiée au modèle de l'arme
    }

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }



    [Client]
    private void Shoot()
    {
        RaycastHit hit;
        if(Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, weapon.range, mask))
        {
            if(hit.collider.tag == "Player")
            {
                CmdPlayerShoot(hit.transform.name, weapon.damage); // Envoie une commande au serveur pour indiquer que le joueur a été touché
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
