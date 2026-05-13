
using UnityEngine;

[System.Serializable] // Permet de rendre cette classe visible et éditable dans l'inspecteur de Unity
public class PlayerWeapon
{
    public string weaponName = "Submachine Gun";
    public float damage = 10f;
    public float range = 100f;

    public float fireRate = 0f;
    public GameObject graphics;
}
