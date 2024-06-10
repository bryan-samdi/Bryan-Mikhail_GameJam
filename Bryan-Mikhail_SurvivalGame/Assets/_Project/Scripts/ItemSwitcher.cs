using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSwitcher : MonoBehaviour
{
    public GameObject hands;
    public GameObject knife;
    public GameObject gun;
    public GameObject lighter;
    AudioManager audioManager;
    private GameObject activeItem;
    private PlayerAnimation playerAnimation;

    private void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        playerAnimation = GetComponent<PlayerAnimation>();
        activeItem = hands;
        hands.SetActive(true);
        knife.SetActive(false);
        gun.SetActive(false);
        lighter.SetActive(false);

        playerAnimation.EquipWeapon(activeItem);
        EnableScripts(activeItem);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchItem(hands);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchItem(knife);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SwitchItem(gun);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SwitchItem(lighter);

    }

    private void SwitchItem(GameObject newItem)
    {
        if (activeItem != newItem)
        {
            DisableScripts(activeItem);
            activeItem.SetActive(false);
        audioManager.PlaySFX(audioManager.switcHandsSound);

            activeItem = newItem;
            activeItem.SetActive(true);
            playerAnimation.EquipWeapon(activeItem);

            EnableScripts(activeItem);
        }
    }

    private void EnableScripts(GameObject item)
    {
        if (item == gun)
        {
            GunScript gunScript = item.GetComponent<GunScript>();
            if (gunScript != null)
            {
                gunScript.enabled = true;
            }
        }
    }

    private void DisableScripts(GameObject item)
    {
        if (item == gun)
        {
            GunScript gunScript = GetComponent<GunScript>();
            if (gunScript != null)
            {
                gunScript.enabled = false;
            }
        }
    }
}