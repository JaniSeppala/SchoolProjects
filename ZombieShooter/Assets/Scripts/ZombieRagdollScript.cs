using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRagdollScript : MonoBehaviour
{
    [SerializeField] private float _force = 1.0f;

    private Rigidbody _rb;
    private AudioSource _audio;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(Camera.main.transform.forward.normalized * _force, ForceMode.Impulse);
        _audio = GetComponent<AudioSource>();
        _audio.Play();
    }
}
