using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellScript : MonoBehaviour
{
    [SerializeField] private float _ejectForce = 1;

    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.AddForce(new Vector3(transform.right.x, Random.Range(transform.right.y, transform.right.y + 20), transform.right.z).normalized * _ejectForce, ForceMode.Impulse);
        Destroy(gameObject, 60.0f);
    }
}
