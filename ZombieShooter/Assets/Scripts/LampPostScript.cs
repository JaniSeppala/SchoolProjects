using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampPostScript : MonoBehaviour
{
    [SerializeField] private GameObject _spotLight;

    private Renderer _rend;
    private SphereCollider _collider;

    void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _rend = GetComponent<Renderer>();
    }

    public void DisableLight()
    {
            _spotLight.SetActive(false);
            _rend.material.DisableKeyword("_EMISSION");
            _collider.enabled = false;
    }
}
