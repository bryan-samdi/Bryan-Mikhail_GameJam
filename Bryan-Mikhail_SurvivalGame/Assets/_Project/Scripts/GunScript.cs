using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class GunScript : MonoBehaviour
{
    public Animator anim;
    public GameObject muzzleFlash;
    public float shootForce, upwardForce;
    public float timeBetweenShooting, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    AudioManager audioManager;

    public TextMeshProUGUI ammunitionDisplay;

    private int bulletsLeft, bulletsShot;
    private bool shooting, readyToShoot, reloading;
    public Transform attackPoint;
    private Camera fpsCam;
    private bool allowInvoke = true;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        bulletsLeft = magazineSize;
        readyToShoot = true;
        fpsCam = Camera.main;
        UpdateUI();
    }

    private void Update()
    {
        MyInput();

        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft + " / " + magazineSize);

       
    }

    private void MyInput()
    {
        if (allowButtonHold)
            shooting = Input.GetKey(KeyCode.Mouse0);
        else
            shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading)
            Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;
        audioManager.PlaySFX(audioManager.gunShootSound);

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy") || hit.collider.CompareTag("Animal"))
            {
                HealthSystem healthSystem = hit.collider.GetComponent<HealthSystem>();
                if (healthSystem != null)
                    healthSystem.TakeDamage(70); 

                if (muzzleFlash != null)
                {
                    GameObject flash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
                    Destroy(flash, 2f);
                }
            }
        }

        bulletsLeft--;
      

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        UpdateUI();
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
    }

    private void Reload()
    {
        audioManager.PlaySFX(audioManager.playerhitSound);
        anim.SetTrigger("PlayerReload");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft + " / " + magazineSize);

    }
}
