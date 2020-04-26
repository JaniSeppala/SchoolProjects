using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private int _healthPoints = 100;
    [SerializeField] private float _force = 80;
    [SerializeField] private float _sprintForce = 120;
    [SerializeField] private float _knockBackForce = 80;
    [SerializeField] private GameObject _characterAimFocusPoint;
    [SerializeField] private float _pistolShootDistance = 90.0f;
    [SerializeField] private float _shotgunShootDistance = 20.0f;
    [SerializeField] private GameObject _ragDoll;
    [SerializeField] private GameObject _pistol;
    [SerializeField] private GameObject _shotgun;
    [SerializeField] private Transform _pistolShellEjector;
    [SerializeField] private GameObject _pistolShell;
    [SerializeField] private Transform _shotgunShellEjector;
    [SerializeField] private GameObject _shotgunShell;
    [SerializeField] private GameObject _backShotgun;
    [SerializeField] private GameObject _hipPistol;
    [SerializeField] private GameObject _pistolMuzzleFlare;
    [SerializeField] private GameObject _shotgunMuzzleFlare;

    private Rigidbody _rb;
    private GameController _controller;

    void Start()
    {
        _controller = GameController.Controller;
        _rb = GetComponent<Rigidbody>();
        _controller.PistolEquipped = true;
        _controller.ShotgunEquipped = false;
        _controller.UpdateAmmoCountPistol();
    }

    void Update()
    {
        if ((Input.GetAxis("ScrollWheel") != 0.0f || Input.GetButtonDown("ChangeWeapon")) && !_controller.Reloading)
        {
            if (_controller.PistolEquipped)
            {
                _controller.PistolEquipped = false;
                _controller.ShotgunEquipped = true;
                _pistol.gameObject.SetActive(false);
                _hipPistol.gameObject.SetActive(true);
                _backShotgun.gameObject.SetActive(false);
                _shotgun.gameObject.SetActive(true);
                _controller.UpdateAmmoCountShotgun();
            }
            else
            {
                _controller.PistolEquipped = true;
                _controller.ShotgunEquipped = false;
                _shotgun.gameObject.SetActive(false);
                _backShotgun.gameObject.SetActive(true);
                _hipPistol.gameObject.SetActive(false);
                _pistol.gameObject.SetActive(true);
                _controller.UpdateAmmoCountPistol();
            }
        }

        if (Input.GetButton("Fire2"))
        {
            _controller.Shooting = true;
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            transform.rotation = Quaternion.LookRotation(new Vector3(transform.forward.x, 0, transform.forward.z));
            _controller.Shooting = false;
        }

        if (_controller.Shooting && Input.GetButtonDown("Fire1") && !_controller.Reloading)
        {
            if (_controller.PistolEquipped && _controller.PistolClip > 0)
            {
                PistolShoot();
                Instantiate(_pistolShell, _pistolShellEjector.position, transform.rotation);
            }
            else if (_controller.ShotgunEquipped && _controller.ShotgunClip > 0)
            {
                ShotgunShoot();
                Instantiate(_shotgunShell, _shotgunShellEjector.position, transform.rotation);
            }
        }
    }

    void FixedUpdate()
    {
        float horizontalMovement = Input.GetAxis("Horizontal");
        float verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0.0f || verticalMovement != 0.0f)
        {
            Vector3 relativeMovement = Camera.main.transform.TransformVector(new Vector3(horizontalMovement, 0, verticalMovement));
            float originalVelocity = _rb.velocity.y;

            if (Input.GetButton("Sprint") && !_controller.Shooting && _controller.Stamina > 0.0f)
            {
                _rb.velocity = new Vector3(relativeMovement.x, 0, relativeMovement.z).normalized * _sprintForce * Time.deltaTime + new Vector3(0, originalVelocity, 0);
            }
            else
            {
                _rb.velocity = new Vector3(relativeMovement.x, 0, relativeMovement.z).normalized * _force * Time.deltaTime + new Vector3(0, originalVelocity, 0);
            }

            if (!_controller.Shooting)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(_rb.velocity.x, 0, _rb.velocity.z)), 0.1f);
            }
            else if (!_controller.FirstPersonMode)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_characterAimFocusPoint.transform.forward), 0.75f);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.forward), 0.75f);
            }
        }
        else if (_controller.Shooting)
        {
            if (!_controller.FirstPersonMode)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_characterAimFocusPoint.transform.forward), 0.75f);
            }
            else
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.forward), 0.75f);
            }
        }
        else if (_controller.FirstPersonMode)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Camera.main.transform.forward), 0.75f);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            TakeDamage(100, Vector3.forward);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Zombie")
        {
            Vector3 rayOrigin = new Vector3(transform.position.x, transform.position.y + 1.65f, transform.position.z);
            Vector3 rayTarget = new Vector3(other.transform.position.x, other.transform.position.y + 1.65f, other.transform.position.z);
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, (rayTarget - rayOrigin).normalized, out hit, 10.0f))
            {
                ZombieController zc = hit.collider.GetComponentInParent<ZombieController>();
                if (zc != null)
                {
                    zc.ChasePlayer();
                }
            }
        }
    }

    private void PistolShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _pistolShootDistance))
        {
            ZombieHitDetect hitDetect = hit.collider.gameObject.GetComponent<ZombieHitDetect>();
            if (hitDetect != null)
            {
                hitDetect.TakeDamage();
            }
            else if (hit.collider.gameObject.tag == "LampPost")
            {
                LampPostScript lamp = hit.collider.gameObject.GetComponent<LampPostScript>();
                lamp.DisableLight();
            }
        }
        _pistolMuzzleFlare.SetActive(true);
        _controller.PistolClip--;
        _controller.UpdateAmmoCountPistol();
        Invoke("DisableMuzzleFlare", 0.1f);
    }

    private void ShotgunShoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, _shotgunShootDistance))
        {
            ZombieHitDetect hitDetect = hit.collider.gameObject.GetComponent<ZombieHitDetect>();
            if (hitDetect != null)
            {
                hitDetect.TakeDamage();
            }
        }
        _shotgunMuzzleFlare.SetActive(true);
        _controller.ShotgunClip--;
        _controller.UpdateAmmoCountShotgun();
        Invoke("DisableMuzzleFlare", 0.1f);
    }

    public void TakeDamage(int damage, Vector3 force)
    {
        _healthPoints -= damage;
        _rb.AddForce(force.normalized * _knockBackForce, ForceMode.Impulse);

        if (_healthPoints <= 0 && !_controller.GameOver)
        {
            _controller.GameEnd();
            Instantiate(_ragDoll, transform.position, transform.rotation);
            gameObject.SetActive(false);
        }
    }

    private void DisableMuzzleFlare()
    {
        _pistolMuzzleFlare.SetActive(false);
        _shotgunMuzzleFlare.SetActive(false);
    }
}
