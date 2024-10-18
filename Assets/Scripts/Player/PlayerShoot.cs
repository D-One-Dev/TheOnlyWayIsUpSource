using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerShoot : MonoBehaviour
{
    public int ammoLeft; //amount of ammo left
    public Gun currGun; //current player gun
    [SerializeField] private Gun defaultGun; //default gun
    [SerializeField] private GameObject shootPoint; //shooting point
    public GameObject projectilesObject; //GameObject for all projectiles
    [SerializeField]private bool isShooting = false; //is player currently shooting
    [SerializeField]private bool hasShot = false; //has player already shot
    [SerializeField] private AudioSource AS;
    [SerializeField] private AudioClip shootSound; //shooting sound
    [SerializeField] private CameraController cameraController; //CameraController script
    [SerializeField] private Text ammoText; //left ammo UI text
    [SerializeField] private Image ammoIcon; //ammo UI icon
    [SerializeField] private GameObject ammoUI; //ammo UI GameObject
    [SerializeField] private Gun[] guns;
    private void Start()
    {
        string curGunName = PlayerPrefs.GetString("CurrentGun", "Default");
        if (curGunName == "Default") curGunName = GetComponent<PlayerMovement>().suit.startGun;
        foreach(Gun gun in guns)
        {
            if(gun.gunName == curGunName)
            {
                currGun = gun;
                break;
            }
        }

        ammoLeft = PlayerPrefs.GetInt("CurrentAmmo", 0);
        if (ammoLeft == 0) ammoLeft = currGun.ammoAmount;
        UpdateAmmoUI();
    }
    void Update()
    {
        //FOR DEBUGGING
        if (Input.GetKeyDown(KeyCode.F)) ShootDown();
        if (Input.GetKeyUp(KeyCode.F)) ShootUp();

        if (isShooting)
        {
            if (!hasShot) //if player presses shoot button and havent shoot yet
            {
                AS.pitch = Random.Range(.8f, 1.2f);
                AS.PlayOneShot(shootSound); //playing shoot sound

                cameraController.ShakeCamera(.2f, .01f); //shaking camera

                //Instantiating the projectile
                for(int i = 0; i < currGun.shotBulletsAmount; i++) Instantiate(currGun.projectile, shootPoint.transform.position, shootPoint.transform.rotation, projectilesObject.transform);
                hasShot = true;

                if (!currGun.isDefaultGun && ammoLeft > 0) ammoLeft--; //decreasing ammo
                UpdateAmmoUI(); //updating ammo UI
                if (ammoLeft <= 0 && !currGun.isDefaultGun) currGun = defaultGun; //changing gun to default if no ammo left

                if (currGun.isAutomatic)
                {
                    StartCoroutine(Recoil(currGun.recoilTime));//setting the recoil time
                }
            }
        }
    }

    public void ShootDown()
    {
        if (Time.timeScale > 0f) isShooting = true;
    }

    public void ShootUp()
    {
        isShooting = false;
        hasShot = false;
    }

    public IEnumerator Recoil(float recoilTime) //Recoil
    {
        yield return new WaitForSeconds(recoilTime);
        if (currGun.isAutomatic) hasShot = false;
    }

    public void UpdateAmmoUI() //updating ammo UI
    {
        if (ammoLeft > 0) //if player has upgraded gun
        { 
            ammoText.text = ammoLeft.ToString();
            ammoIcon.sprite = currGun.icon;
            ammoUI.SetActive(true);
        }
        else ammoUI.SetActive(false); //if player has default gun
    }
}
