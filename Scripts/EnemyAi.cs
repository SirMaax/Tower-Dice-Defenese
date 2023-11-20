
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{

    [SerializeField] public float health;

    public float timeTillDmg;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            foreach (var ele in GameObject.FindGameObjectsWithTag("Tower"))
            {
                ele.TryGetComponent(out TowerAi ai); 
                if(ai != null) ai.RemoveGameObject(gameObject);
            }

            Destroy(gameObject);
        }
    }

    public void TakeDmg(float dmg)
    {
        StartCoroutine(Dmg(dmg));
    }

    private IEnumerator Dmg(float dmg)
    {
        yield return new WaitForSeconds(timeTillDmg);
        health -= dmg;
    } 
}
