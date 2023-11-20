using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    // Start is called before the first frame update
    public float timeToDespawn;
    public Vector3 direction;
    public float speed;
    private Rigidbody rb;
    private bool stuck = false;
    private Vector3 stuckPos;
    public ParticleSystem _particleSystem;
    private GameObject myObjject;
    private bool once = false;
    void Start()
    {
        direction = direction - transform.position;
        rb = gameObject.GetComponent<Rigidbody>();
        Debug.Log("Start ");
        Debug.Log(direction);

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!stuck) rb.AddForce(direction * speed);
        if (stuck) rb.position = stuckPos;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Tower")) return;
        if (!once)
        {
            once = true;
        stuckPos = rb.position;
        stuck = true;
        rb.velocity = Vector3.zero;
        rb.freezeRotation = true;
        _particleSystem.Play();
        StartCoroutine(removeObjectIn());
        }
    }

    public IEnumerator removeObjectIn()
    {
        yield return new WaitForSeconds(timeToDespawn);

        Destroy(gameObject);
        
        
    }
}
