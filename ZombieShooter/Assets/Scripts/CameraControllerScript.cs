using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraControllerScript : MonoBehaviour
{
    [SerializeField] private GameObject _freeLookCamera;
    [SerializeField] private GameObject _3rdPersonAimCamera;
    [SerializeField] private GameObject _1stPersonCamera;
    [SerializeField] private GameObject _1stPersonCrosshair;

    private GameController _controller;

    void Start()
    {
        _controller = GameController.Controller;
    }

    void Update()
    {
        if (!_controller.FirstPersonMode && !_controller.GameOver)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                _freeLookCamera.SetActive(false);
                _3rdPersonAimCamera.SetActive(true);
            }

            if (Input.GetButtonUp("Fire2"))
            {
                _freeLookCamera.SetActive(true);
                _3rdPersonAimCamera.SetActive(false);
            }

            if (Input.GetButtonDown("ChangeView"))
            {
                _controller.FirstPersonMode = true;
                _1stPersonCamera.SetActive(true);
                _freeLookCamera.SetActive(false);
                _3rdPersonAimCamera.SetActive(false);

                if (Input.GetButton("Fire2"))
                {
                    _1stPersonCrosshair.SetActive(true);
                }
            }
        }
        else if (Input.GetButtonDown("ChangeView") && !_controller.GameOver)
        {
            _controller.FirstPersonMode = false;
            _1stPersonCamera.SetActive(false);

            if (Input.GetButton("Fire2"))
            {
                _3rdPersonAimCamera.SetActive(true);
            }
            else
            {
                _freeLookCamera.SetActive(true);
            }
        }

        if (_controller.FirstPersonMode && !_controller.GameOver)
        {
            if (Input.GetButtonDown("Fire2"))
            {
                _1stPersonCrosshair.SetActive(true);
            }

            if (Input.GetButtonUp("Fire2"))
            {
                _1stPersonCrosshair.SetActive(false);
            }
        }

        if (_controller.GameOver)
        {
            _freeLookCamera.SetActive(true);
            _1stPersonCamera.SetActive(false);
            _3rdPersonAimCamera.SetActive(false);
        }
    }
}
