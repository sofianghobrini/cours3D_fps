using UnityEngine;
using Mirror;
using UnityEngine.Video;
using System.Collections;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private WeaponData primaryWeapon;

    private WeaponData currentWeapon;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    [HideInInspector]
    public int currentAmmoSize;

    [HideInInspector]
    public bool IsReloading = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(WeaponData _weapon)
    {
        currentWeapon = _weapon;
        currentAmmoSize = _weapon.maxAmmo;

        GameObject weaponIns = Instantiate(_weapon.graphics, weaponHolder.position, weaponHolder.rotation, weaponHolder);
        weaponIns.transform.SetParent(weaponHolder);


        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();

        if(currentGraphics == null)
        {
            Debug.LogError("WeaponManager: No WeaponGraphics component found on the weapon prefab!");
        }

        if(isLocalPlayer)
        {
            Util.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
        }
    }

    public WeaponData GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    public IEnumerator Reload()
    {
        if(IsReloading)
        {
            yield break;
        }

        IsReloading = true;
        
        CmdOnReload();
        yield return new WaitForSeconds(currentWeapon.reloadTime); 
        currentAmmoSize = currentWeapon.maxAmmo;
        

        IsReloading = false;
        Debug.Log("Rechargement terminé! Munitions remplies: " + currentAmmoSize);

    }


    [Command]
    void CmdOnReload()
    {
        RpcOnReload();
    }

    [ClientRpc]
    void RpcOnReload()
    {
        Animator anim = currentGraphics.GetComponent<Animator>();
        if(anim != null)
        {
            anim.SetTrigger("Reload");
        }
    }

}
