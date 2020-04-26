using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRagdollScript : MonoBehaviour
{
    [SerializeField] private float _force = 1.0f;
    [SerializeField] private GameObject _pistol;
    [SerializeField] private GameObject _hipPistol;
    [SerializeField] private GameObject _shotgun;
    [SerializeField] private GameObject _backShotgun;
    [SerializeField] private Rigidbody _spine;

    private GameController _controller;
    private AudioSource _audio;

    void Start()
    {
        _controller = GameController.Controller;
        _audio = GetComponent<AudioSource>();

        if (_controller.ShotgunEquipped)
        {
            _shotgun.gameObject.SetActive(true);
            _hipPistol.gameObject.SetActive(true);
        }
        else
        {
            _pistol.gameObject.SetActive(true);
            _backShotgun.gameObject.SetActive(true);
        }

        _spine.AddForce(Camera.main.transform.forward.normalized * _force, ForceMode.Impulse);
        _audio.Play();
    }
}
