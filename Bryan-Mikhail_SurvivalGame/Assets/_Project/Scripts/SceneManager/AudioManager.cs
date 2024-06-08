using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource musicSource;

    [Header("Audio Clips")]

    public AudioClip background;

    public AudioClip weaponPickUpSound;

    public AudioClip playerhitSound;
    public AudioClip playerReload;

    public AudioClip deathSound;

    public AudioClip enemyDie;
    public AudioClip enemyHitSound;

    public AudioClip pauseSound;
    public AudioClip buttonHoverSound;
    public AudioClip buttonSelectSound;

    private void Start()
    {
        musicSource.clip = background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

   

    public void PlaySFXPickUp(AudioClip playerReload)
    {
        SFXSource.PlayOneShot(playerReload);
    }

    public void PlaySFXPlayerReload(AudioClip weaponPickUpSound)
    {
        SFXSource.PlayOneShot(weaponPickUpSound);
    }

    public void PlaySFXPlayerHit(AudioClip playerhitSound)
    {
        SFXSource.PlayOneShot(playerhitSound);
    }

    public void PlaySFXEnemyHit(AudioClip enemyHitSound)
    {
        SFXSource.PlayOneShot(enemyHitSound);
    }
    public void PlaySFXEnemyDie(AudioClip enemyDie)
    {
        SFXSource.PlayOneShot(enemyDie);
    }


    public void PlayButtonHoverSound()
    {
        SFXSource.PlayOneShot(buttonHoverSound);
    }

    public void PlayButtonSelectSound()
    {
        SFXSource.PlayOneShot(buttonSelectSound);
    }



    public void PlayPauseSound()
    {
        SFXSource.PlayOneShot(pauseSound);
    }



}
