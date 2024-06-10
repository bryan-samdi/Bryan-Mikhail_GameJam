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

    public AudioClip PickUpSound;
    public AudioClip playerhitSound;
    public AudioClip gunShootSound;
    public AudioClip gunReloadSound;
    public AudioClip deathSound;
    public AudioClip switcHandsSound;

    public AudioClip enemyChase;
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

    public void PlaySFXPlayetShoot(AudioClip gunShootSound)
    {
        SFXSource.PlayOneShot(gunShootSound);
    }

    public void PlaySFXPlayerReload (AudioClip gunReloadSound)
    {
        SFXSource.PlayOneShot(gunReloadSound);
    }

    public void PlaySFXPlayerSwitch(AudioClip switcHandsSound)
    {
        SFXSource.PlayOneShot(switcHandsSound);
    }

    public void PlaySFXPlayerPickUp(AudioClip PickUpSound)
    {
        SFXSource.PlayOneShot(PickUpSound);
    }

    public void PlaySFXPlayerHit(AudioClip playerhitSound)
    {
        SFXSource.PlayOneShot(playerhitSound);
    }

    public void PlaySFXEnemyHit(AudioClip enemyHitSound)
    {
        SFXSource.PlayOneShot(enemyHitSound);
    }

    public void PlaySFXEnemyChase(AudioClip enemyChase)
    {
        SFXSource.PlayOneShot(enemyChase);
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
