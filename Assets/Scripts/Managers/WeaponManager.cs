using UnityEngine;
using Mirror;
using UnityEngine.Video;

public class WeaponManager : NetworkBehaviour
{

    [SerializeField]
    private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private Transform weaponHolder;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    void EquipWeapon(PlayerWeapon _weapon)
    {
        currentWeapon = _weapon;

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

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }



}
