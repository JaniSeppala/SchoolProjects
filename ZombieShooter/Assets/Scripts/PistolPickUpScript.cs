using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistolPickUpScript : MonoBehaviour
{
    [SerializeField] private int _ammoAmount = 5;
    [SerializeField] private GameObject _pistol;
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
        _pistol.SetActive(false);
    }

    void Update()
    {
        if (_pickedUp)
        {
            _respawnTimer += Time.deltaTime;

            if (_respawnTimer >= _spawnTime)
            {
                _pistol.SetActive(true);
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
                _controller.AddPistolAmmo(_ammoAmount);
                _pistol.SetActive(false);
                _audio.Play();
                _pickedUp = true;
            }
        }
    }
}
