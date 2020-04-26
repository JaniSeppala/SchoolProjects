using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator _anim;
    private GameController _controller;

    void Start()
    {
        _controller = GameController.Controller;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_controller.ShotgunEquipped)
        {
            _anim.SetBool("ShotgunEquipped", true);
            _anim.SetBool("PistolEquipped", false);
        }
        else
        {
            _anim.SetBool("ShotgunEquipped", false);
            _anim.SetBool("PistolEquipped", true);
        }
        if (Input.GetButton("Fire2"))
        {
            _anim.ResetTrigger("WeaponRelease");
            _anim.SetTrigger("WeaponPoint");
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            _anim.ResetTrigger("WeaponPoint");
            _anim.SetTrigger("WeaponRelease");
        }
        if (_controller.Shooting && Input.GetButtonDown("Fire1") && !_controller.Reloading)
        {
            if (_controller.PistolEquipped && _controller.PistolClip > 0)
            {
                _anim.SetTrigger("WeaponShoot");
            }

            else if (_controller.ShotgunEquipped && _controller.ShotgunClip > 0)
            {
                _anim.SetTrigger("WeaponShoot");
            }
        }

        if ((Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f))
        {
            if (Input.GetButton("Sprint") && !_controller.Shooting && _controller.Stamina > 0.0f)
            {
                _anim.SetTrigger("Running");
                _anim.ResetTrigger("Walking");
            }
            else
            {
                _anim.SetTrigger("Walking");
                _anim.ResetTrigger("Running");
            }
            _anim.ResetTrigger("Idle");
        }
        else
        {
            _anim.ResetTrigger("Running");
            _anim.ResetTrigger("Walking");
            _anim.SetTrigger("Idle");
        }
    }
}
