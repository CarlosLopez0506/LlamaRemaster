using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spit : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float lifetime;
    [SerializeField] private float speed;
    [SerializeField] private GameObject hitParticles;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
        StartCoroutine(Lifetime());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.forward = -transform.forward;
            other.GetComponent<LlamaMovement>().Stun();
            Instantiate(hitParticles, transform.position, hitParticles.transform.rotation);
            Destroy(gameObject);
        }
    }

    private IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(lifetime);
        Destroy(gameObject);
    }
}
