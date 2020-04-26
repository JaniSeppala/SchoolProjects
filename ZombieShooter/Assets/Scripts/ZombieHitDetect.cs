using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHitDetect : MonoBehaviour
{
    [SerializeField] private float _damage = 10.0f;

    private ZombieController _zombieController;
    private GameController _controller;
    private PlayerMovement _pm;

    void Start()
    {
        _controller = GameController.Controller;
        _zombieController = GetComponentInParent<ZombieController>();
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Player")
        {
            Vector3 force = new Vector3(_zombieController.transform.forward.x, 0, _zombieController.transform.forward.z);
            _controller.PlayerMovement.TakeDamage(25, force);
        }
    }

    public void TakeDamage()
    {
        _zombieController.TakeDamage(_damage);
    }
}
