using UnityEngine;
using System.Collections;

public class PickUpWeapon : MonoBehaviour
{

    [SerializeField] 
    private WeaponData theWeapon;

    [SerializeField]
    private float respawnDelay = 10f;

    private GameObject pickUpGraphics;
    private bool canPickUp;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ResetWeapon();
    }


    private void ResetWeapon()
    {
        pickUpGraphics =Instantiate(theWeapon.graphics, transform);
        pickUpGraphics.transform.position = transform.position;
        canPickUp = true; 
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && canPickUp)
        {
            WeaponManager weaponManager = other.GetComponent<WeaponManager>();

            if (weaponManager != null)
            {
                EquipNewWeapon(weaponManager);
            }
        }
    }

    void EquipNewWeapon(WeaponManager _weaponManager)
    {
        // detruit l'arme actuelle du joueur et équipe la nouvelle arme
        Destroy(_weaponManager.GetCurrentGraphics().gameObject);
        _weaponManager.EquipWeapon(theWeapon);

        canPickUp = false;
        Destroy(pickUpGraphics);

        StartCoroutine(DelayRespawnWeapon());
    }

    IEnumerator DelayRespawnWeapon()
    {
        yield return new WaitForSeconds(respawnDelay);
        ResetWeapon();
    }
}
