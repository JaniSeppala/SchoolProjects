using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioScript : MonoBehaviour
{
    [SerializeField] private AudioSource _pistolShoot;
    [SerializeField] private AudioSource _pistolReload;
    [SerializeField] private AudioSource _shotgunShoot;
    [SerializeField] private AudioSource _shotgunReload;
    [SerializeField] private AudioSource _walking;
    [SerializeField] private AudioSource _emptyClip;
    [SerializeField] private AudioClip _pistolShootClip;
    [SerializeField] private AudioClip _shotgunShootClip;

    private GameController _controller;

    void Start()
    {
        _controller = GameController.Controller;        
    }

    void Update()
    {
        if (_controller.Shooting && Input.GetButtonDown("Fire1") && !_controller.Reloading)
        {
            if (_controller.PistolEquipped && _controller.PistolClip > 0)
            {
                _pistolShoot.PlayOneShot(_pistolShootClip, 1.0f);
            }
            else if (_controller.ShotgunEquipped && _controller.ShotgunClip > 0)
            {
                _shotgunShoot.PlayOneShot(_shotgunShootClip, 1.0f);
            }
            else
            {
                _emptyClip.Play();
            }
        }
        else if (_controller.Reloading && Input.GetButton("Reload"))
        {
            if (_controller.PistolEquipped)
            {
                _pistolReload.Play();
            }
            else if (_controller.ShotgunEquipped)
            {
                _shotgunReload.Play();
            }
        }

        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {
            if (!_walking.isPlaying)
            {
                _walking.Play();
            }
        }
        else if (_walking.isPlaying)
        {
            _walking.Stop();
        }
    }
}
