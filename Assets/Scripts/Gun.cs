using UnityEngine;

[CreateAssetMenu(fileName = "Gun", menuName = "Gun")]
public class Gun : ScriptableObject
{
    public string gunName; //gun name
    public bool isAutomatic; //gun type (automatic/single)
    public GameObject projectile; //projectile that gets shooted
    public float recoilTime; //recoil time
    public bool isDefaultGun; //is a default gun
    public int ammoAmount; //amount of ammo on pickup
    public int shotBulletsAmount; //amount of bullets launched at one shot
    public Sprite icon; //gun icon
}
