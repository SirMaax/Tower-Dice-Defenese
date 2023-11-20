using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerEnum : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Prefabs")] 
    public GameObject prefab1;
    public GameObject prefab2;
    public GameObject prefab3;
    public GameObject prefab4;
    public GameObject prefab5;
    public GameObject prefab6;
    public GameObject prefab7;
    public GameObject prefab8;
    public GameObject prefab9;
    public GameObject prefab10;
    public GameObject prefab11;
    public GameObject prefab12;
    public GameObject prefab13;
    public GameObject prefab14;
    public GameObject prefab15;
    public GameObject prefab16;
    public GameObject prefab17;
    public GameObject prefab18;
    
    public Tower[] allTowers;
    void Start()
    {
        allTowers = new Tower[18];
        allTowers[0] = new Tower("Wall", 1, prefab1);
        allTowers[1] = new Tower("Ballista", 2, prefab2);
        allTowers[2] = new Tower("StrongBallista", 3, prefab3);
        allTowers[3] = new Tower("Cannon", 3, prefab4);
        allTowers[4] = new Tower("Cannon", 3, prefab5);
        allTowers[5] = new Tower("Cannon", 3, prefab6);
        allTowers[6] = new Tower("Cannon", 3, prefab7);
        allTowers[7] = new Tower("Cannon", 3, prefab8);
        allTowers[8] = new Tower("Cannon", 3, prefab9);
        allTowers[9] = new Tower("Cannon", 3, prefab10);
        allTowers[10] = new Tower("Cannon", 3, prefab11);
        allTowers[11] = new Tower("Cannon", 3, prefab12);
        allTowers[12] = new Tower("Cannon", 3, prefab13);
        allTowers[13] = new Tower("Cannon", 3, prefab14);
        allTowers[14] = new Tower("Cannon", 3, prefab15);
        allTowers[15] = new Tower("Cannon", 3, prefab16);
        allTowers[16] = new Tower("Cannon", 3, prefab17);
        allTowers[17] = new Tower("Cannon", 3, prefab18);
    }

    public class Tower
    {
        public String name;
        public int value;
        public GameObject prefab;

        public Tower(String name, int value, GameObject prefab)
        {
            this.name = name;
            this.value = value;
            this.prefab = prefab;
        }
    }
} 