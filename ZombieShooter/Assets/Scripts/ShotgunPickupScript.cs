using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPickupScript : MonoBehaviour
{
    [SerializeField] private int _ammoAmount = 5;
    [SerializeField] private GameObject _shotgun;
    [SerializeField] private float _spawnTime = 30;

    private AudioSource _audio;
    private bool _pickedUp;
    private float _respawnTimer;
    private GameController _controller;

    void Start()
    {
        _audio = GetComponent<AudioSource>();
        _controller = GameController.Controller;
        _pickedUp = true;
        _shotgun.SetActive(false);
    }

    void Update()
    {
        if (_pickedUp)
        {
            _respawnTimer += Time.deltaTime;

            if (_respawnTimer >= _spawnTime)
            {
                _shotgun.SetActive(true);
                _pickedUp = false;
                _respawnTimer = 0.0f;
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!_pickedUp)
        {
            if (col.gameObject.tag == "Player")
            {
                _controller.AddShotgunAmmo(_ammoAmount);
                _shotgun.SetActive(false);
                _audio.Play();
                _pickedUp = true;
            }
        }
    }
}