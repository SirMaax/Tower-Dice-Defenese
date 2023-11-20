using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Placer : MonoBehaviour
{

    [SerializeField] private float distanceToNextTower;
    [SerializeField] private float distanceRoad;
    [SerializeField] private float heightOfPlacedTowers;
    
    [Header("Refs")] public Vector3 customVector;
    public MouseController controller;
    public TowerEnum towerEnum;
    
    private LayerMask routLayer;
    // Start is called before the first frame update
    void Start()
    {
        routLayer = LayerMask.GetMask("routLayer");
    }

    // Update is called once per frame
    void Update()
    {
        PlaceTower();
    }

    public void PlaceTowerReady()
    {
        Debug.Log("Placing ready");
        controller.placing = true;
        
    }

    public void PlaceTower()
    {
        
        if (!controller.placing) return;
        if (GameController.placedTower >= 3) return;
        if (controller.placing && Mouse.current.leftButton.wasPressedThisFrame)
        {
            int value = controller.combination;
            Vector3 position = controller.place.position;
            if (value == 0)
            {
                value = controller.otherCombination;
            }
        
        List<GameObject> allObject = new List<GameObject>();
        
        allObject.AddRange((GameObject.FindGameObjectsWithTag("Tower")));
        foreach (var myOjbect in allObject)
        {
            if ((myOjbect.transform.position - position).magnitude < distanceToNextTower)
            {
                controller.placing = false;
                controller.AboardAdding(true);
                Debug.Log("cant place tower next to other tower");
                return;
            }
        }
        allObject.Clear();
        allObject.AddRange((GameObject.FindGameObjectsWithTag("Rout")));
        foreach (var myOjbect in allObject)
        {
            Vector2 temp = new Vector2(myOjbect.transform.position.x, myOjbect.transform.position.z);
            if((temp - new Vector2(position.x,position.z)).magnitude < distanceRoad)
            {
                if (value == 1)
                {
                    var tempVec2 = position;
                    tempVec2.y = -2.5f;
                    Instantiate(towerEnum.allTowers[0].prefab, tempVec2, Quaternion.identity);
                    GameController.placedTower++;
                    // Quaternion.AngleAxis(270,Vector3.up));
                    controller.placing = false;
                    Debug.Log("placed wall");
                    return;

                }
                controller.placing = false;
                controller.AboardAdding(true);
                
                Debug.Log("cant place on the road");
                return;
            }
        }

        if (value == 1) return;
        var tempVec = position;
        tempVec.y = heightOfPlacedTowers;
        Instantiate(towerEnum.allTowers[value - 1].prefab, tempVec, Quaternion.identity);
        GameController.placedTower++;
            // Quaternion.AngleAxis(270,Vector3.up));
        controller.placing = false;
        controller.AboardAdding(true);
        
        }
        else if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            controller.placing = false;
            controller.AboardAdding(true);
        }
    }
    
    
}
