using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieController : MonoBehaviour
{
    [SerializeField] private GameObject _ragdoll;
    [SerializeField] private float _healthPoints = 100;
    [SerializeField] private GameObject _rightEye;
    [SerializeField] private GameObject _leftEye;
    [SerializeField] private AudioSource _chaseAudio;
    [SerializeField] private AudioSource _walkAudio;

    private int _currentTarget;
    private NavMeshAgent _agent;
    private bool _foundPlayer;
    private Animator _anim;
    private GameController _controller;
    private SphereCollider _collider;

    void Start()
    {
        _controller = GameController.Controller;
        _agent = GetComponent<NavMeshAgent>();
        _collider = GetComponent<SphereCollider>();
        _currentTarget = Random.Range(0, _controller.PatrolPoints.Length - 1);
        _agent.destination = _controller.PatrolPoints[_currentTarget].transform.position;
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance)
        {
            _anim.SetTrigger("Stop");
            _anim.ResetTrigger("Walk");
            if (_walkAudio.isPlaying)
            {
                _walkAudio.Stop();
            }
        }
        else
        {
            _anim.SetTrigger("Walk");
            _anim.ResetTrigger("Stop");
            if (!_walkAudio.isPlaying)
            {
                _walkAudio.Play();
            }
        }

        if (_foundPlayer)
        {
            _agent.destination = _controller.Player.transform.position;
        }
        else if (_agent.remainingDistance < 1.5f)
        {
            int newTarget = Random.Range(0, _controller.PatrolPoints.Length);

            if (newTarget != _currentTarget)
            {
                _currentTarget = newTarget;
            }
            else if (newTarget > 0)
            {
                _currentTarget = newTarget - 1;
            }
            else
            {
                _currentTarget = newTarget + 1;
            }

            _agent.destination = _controller.PatrolPoints[_currentTarget].transform.position;
        }
    }

    public void TakeDamage(float damage)
    {
        _healthPoints -= damage;

        if (_healthPoints <= 0.0f || _controller.ShotgunEquipped)
        {
            _controller.AddScore(100);
            Instantiate(_ragdoll, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        else
        {
            _controller.AddScore(5);
            if (!_foundPlayer)
            {
                ChasePlayer();
            }
        }
    }

    public void ChasePlayer()
    {
        _collider.enabled = false;
        _foundPlayer = true;
        _leftEye.SetActive(true);
        _rightEye.SetActive(true);
        _chaseAudio.Play();
    }
}
