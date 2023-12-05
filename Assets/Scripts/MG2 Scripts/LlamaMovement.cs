using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LlamaPoints))]
public class LlamaMovement : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 moveValue;
    private bool _stunned;
    private bool _canShoot;
    private Vector3 spawnPos;
    
    [SerializeField] private float speed;
    [SerializeField] private GameObject spit;
    [SerializeField] private Transform spitPos;

    private void Start()
    {
        _canShoot = true;
        spawnPos = transform.position;
        rb = GetComponent<Rigidbody>();
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!_stunned)
        {
            var moveValueBase = speed * context.ReadValue<Vector2>();
            moveValue = new Vector3(-moveValueBase.x, 0, -moveValueBase.y);
        }
        else
        {
            moveValue = Vector3.zero;
        }
    }

    public void Spit(InputAction.CallbackContext context)
    {
        if (context.started && _canShoot)
        {
            Instantiate(spit, spitPos.position, transform.rotation);
            StartCoroutine(CanShootEnumerator());
        }
    }

    private void FixedUpdate()
    {
        if (moveValue.magnitude > 0)
        {
            rb.MovePosition(transform.position + moveValue);
            var lookRotation = Quaternion.LookRotation(moveValue.normalized);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 10);
        }
    }

    public void Stun()
    {
        StartCoroutine(StunCoroutine());
    }

    public void ReSpawn()
    {
        StartCoroutine(RespawnCoroutine());
    }

    private IEnumerator StunCoroutine()
    {
        _stunned = true;
        rb.AddForce(-transform.forward * 5, ForceMode.Impulse);
        yield return new WaitForSeconds(.5f);
        _stunned = false;
    }

    private IEnumerator CanShootEnumerator()
    {
        _canShoot = false;
        yield return new WaitForSeconds(.5f);
        _canShoot = true;
    }

    private IEnumerator RespawnCoroutine()
    {
        _stunned = true;
        yield return new WaitForSeconds(1.5f);
        transform.position = spawnPos;
        _stunned = false;
    }
}
