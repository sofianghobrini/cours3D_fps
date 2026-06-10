
using UnityEngine;

[CreateAssetMenu(fileName = "weaponData", menuName = "My game/Weapon Data")]
public class WeaponData : ScriptableObject    
{
    public string weaponName = "Submachine Gun";
    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 0f;

    public int maxAmmo = 30;
    public float reloadTime = 2f;
    public GameObject graphics;

    public AudioClip shootSound;
    public AudioClip reloadSound;
}
