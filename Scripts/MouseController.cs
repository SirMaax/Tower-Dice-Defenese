using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using Unity.VisualScripting;

public class MouseController : MonoBehaviour
{
    public Transform place;
    private Camera camera;
    private LayerMask board;
    private LayerMask dice;
    private LayerMask tower;

    private LineRenderer lr;

    [Header("Placing")] 
    public bool placing = false;

    [Header("LineRenderer")] 
    [SerializeField] private float heightOffLine;
    [SerializeField] private float lineExactness;
    public LineRenderer lr2;
    [SerializeField] private float lineHeight;
    private bool thirdDice = false;
    private bool once = false;

    [Header("Text")] 
    public TMP_Text text1;
    public TMP_Text text2;

    [Header("Adding Numbers")] public bool adding = false;
    public int combination = 0;
    private bool startAdding = false;
    public bool startSub = false;
    public bool thirdSub = false;
    public bool secondAdd = false;
    public bool thirdAdd = false;
    public int firstNumber = 0;
    public int secondNumber = 0;
    public bool secondSub = false;
    public int thirdNumber = 0;
    private Vector2[] points;
    private Dice currentDie;
    private Dice firstDice;
    private Dice secondDice;
    private bool differentAdd = false;
    public int otherCombination = 0;
    [Header("DisplayRange")] private GameObject lastTower;

    private bool canClickOnTowers = false;
    // Start is called before the first frame update
    void Start()
    {
        points = new Vector2[5];
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        camera = Camera.main;
        board = LayerMask.GetMask("Board");
        dice = LayerMask.GetMask("Dice");
        tower = LayerMask.GetMask("towerlayer");

    }

    private void Update()
    {
        MouseCheck();
        DrawLine();
        Board();
        RollDice();
        SnapCursorToLine();
    }

    private void FixedUpdate()
    {
        
        
        
        
        
    }

    private void MouseCheck()
    {
        if (placing || !adding) return;
        if (startAdding || startSub)
        {
            
            
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit; 
            if (Physics.Raycast(ray, out hit, 1000, board) )
            {
                points[4] = new Vector2(hit.point.x, hit.point.z);
            }
            
        }
        bool leftB = Mouse.current.leftButton.wasPressedThisFrame;
        bool rightB = Mouse.current.rightButton.wasPressedThisFrame;
        if (leftB || rightB)
        {
            lr.positionCount = 2;
            lr2.positionCount = 2;
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, 1000, dice))
            {
                Debug.Log("hit dice");
                Dice dice = hit.collider.gameObject.GetComponent<Dice>();
                if (thirdAdd || thirdSub)
                {
                    if ((secondDice == firstDice) && (currentDie == dice))AboardAdding(true);
                    if (differentAdd) thirdNumber = secondDice.localScore;
                    else thirdNumber = dice.localScore;
                    Debug.Log(thirdNumber);
                    points[3] = new Vector2(dice.transform.position.x, dice.transform.position.z);
                    EndOfAdd();
                    thirdDice = false;
                    return;
                }
                if (startAdding || startSub)
                {
                    if (secondAdd || secondSub)
                    {
                        
                        if (leftB) thirdAdd = true;
                        else if (rightB) thirdSub = true;
                        if (firstDice != dice && currentDie != dice) differentAdd = true;
                        thirdDice = true;
                        secondDice = dice;
                        points[2] = new Vector2(dice.transform.position.x, dice.transform.position.z);
                        return;
                    }
                    else if (leftB) secondAdd = true;
                    else if (rightB) secondSub = true;

                    
                    if(currentDie == dice)AboardAdding(true);
                    secondNumber = dice.localScore;
                    firstDice = dice;
                    points[1] = new Vector2(dice.transform.position.x, dice.transform.position.z);
                    Debug.Log(secondNumber);
                    EndOfAdd();
                }
                else
                {
                    if (leftB && !startAdding && !startSub)
                    {
                        startAdding = true;
                        firstNumber = dice.localScore;
                        Debug.Log(firstNumber);
                        points[0] = new Vector2(dice.transform.position.x, dice.transform.position.z);
                    }
                    else if (rightB && !startAdding && !startSub)
                    {
                        startSub = true;
                        firstNumber = dice.localScore;
                        Debug.Log(firstNumber);
                        points[0] = new Vector2(dice.transform.position.x, dice.transform.position.z);
                    }

                    currentDie = dice;
                }
            }
            
            else if (Physics.Raycast(ray, out hit, 1000, board) )
            {
                AboardAdding(true);
            }
        }
    }

    private void LineRenderer()
    {
        
    }

    public void AboardAdding(bool clearLine)

    {
        Debug.Log("Aboared Adding");
        //abort all/ maybe only last one
        if (clearLine)
        {
            lr.positionCount = 0;
            lr2.positionCount = 0;
        }
        startAdding = false;
        secondAdd = false;
        thirdAdd = false;
        startSub = false;
        secondSub = false;
        thirdSub = false;
        differentAdd = false;
        firstNumber = 0;
        secondNumber = 0;
        thirdNumber = 0;
        currentDie = null;
        firstDice = null;
        secondDice = null;
        thirdDice = false;
        points[0] = Vector2.zero;
        points[1] = Vector2.zero;
        points[2] = Vector2.zero;
        points[3] = Vector2.zero;
        placing = false;
        text1.SetText("Dice combination Value: 0");
        place.position = new Vector3(-100, -100, 100);

    }

    private void EndOfAdd()
    {
        
        if (startAdding) combination = firstNumber + secondNumber;
        else if (startSub) combination = firstNumber - secondNumber;

        if (thirdAdd) combination += thirdNumber;
        else if (thirdSub) combination -= thirdNumber;
        if (combination < 1)
        {
            AboardAdding(true);
            text1.SetText("Dice combination Value: 0");
            return;
        }
        text1.SetText("Dice combination Value: " + combination.ToString());
        Debug.Log("Score" + combination);
        SnapCursorToLine();
    }

    private void DrawLine()
    {
        if (startAdding || startSub)
        {
            lr.positionCount = 2;
            lr2.positionCount = 2;
        }
        if ((thirdAdd || thirdSub) && !thirdDice)
        {
            lr2.SetPosition(0, new Vector3(points[2].x,lineHeight,points[2].y));
            lr2.SetPosition(1,new Vector3(points[3].x,lineHeight,points[3].y));
            if(thirdAdd)lr2.endColor=Color.blue;
            else lr2.endColor = Color.red;
        }
        else if (thirdDice)
        {
            lr2.SetPosition(0, new Vector3(points[2].x,lineHeight,points[2].y));
            lr2.SetPosition(1,new Vector3(points[4].x,lineHeight,points[4].y));
            if (thirdAdd)
            {
                lr2.startColor=Color.blue; 
                lr2.endColor =Color.blue;
            }
            else
            {
                lr2.startColor = Color.red;
                lr2.endColor =Color.red;
            }
        }
        else if (((secondAdd || secondSub) && !once))
        {
            once = true;
            lr.SetPosition(0, new Vector3(points[0].x,lineHeight,points[0].y));
            lr.SetPosition(1,new Vector3(points[1].x,lineHeight,points[1].y));
        }
        else if ((startAdding || startSub) && (!secondAdd && !secondSub))
        {
            lr.SetPosition(0, new Vector3(points[0].x,lineHeight,points[0].y));
            lr.SetPosition(1,new Vector3(points[4].x,lineHeight,points[4].y));
            if (startAdding)
            {
                lr.startColor=Color.blue; 
                lr.endColor =Color.blue;
            }
            else
            {
                lr.startColor = Color.red;
                lr.endColor =Color.red;
            }
        }
        

    }

    private void SnapCursorToLine()
    {
        if (!placing) return;
        Vector3 mousepos;
        
        Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000, board) )
        {
            mousepos = hit.point;
            mousepos.y = 0;
            Vector3 shortest = Vector3.zero;



                Vector3 start = new Vector3(points[0].x, heightOffLine, points[0].y);
                Vector3 end = new Vector3(points[1].x, heightOffLine, points[1].y);
                Vector3 dir = end - start;

                float distance = 1000000;
                for (int i = 1; i < lineExactness; i++)
                {
                    Vector3 tempVec = start + (dir * i * 1 / lineExactness);
                    Debug.DrawLine(tempVec, mousepos, Color.red);
                    float temp = (tempVec - mousepos).magnitude;
                    if (temp < distance)
                    {
                        distance = temp;
                        shortest = tempVec;
                    }

                    Debug.DrawLine(start, start + dir / 2, Color.cyan);

                }

                start = new Vector3(points[2].x, heightOffLine, points[2].y);
                end = new Vector3(points[3].x, heightOffLine, points[3].y);
                dir = end - start;
                for (int i = 1; i < lineExactness; i++)
                {
                    Vector3 tempVec = start + (dir * i * 1/lineExactness);
                    Debug.DrawLine(tempVec,mousepos,Color.red);
                    float temp = (tempVec - mousepos).magnitude;
                    if (temp < distance)
                    {
                        distance = temp;
                        shortest = tempVec;
                    }
                    Debug.DrawLine(start,start + dir/2,Color.cyan);
                
                }

                if (combination == 0)
                {
                    //get distance to shortest dice
                    float dist = 10000;
                    GameObject test = null;
                    foreach (var ele in GameObject.FindGameObjectsWithTag("Dice"))
                    {
                        var temp = (ele.transform.position - transform.position).magnitude;
                        if (temp < dist)
                        {
                            shortest = ele.transform.position;
                            dist = temp;
                            test = ele;
                        }
                    }

                    otherCombination = test.GetComponent<Dice>().localScore;
                }
                place.position = shortest;
        }
    }

    public void GetDice()
    {
        StartCoroutine(startDice());
    }

    public void RollDice()
    {
        if (!Dice.returnHand) return;

        if (Mouse.current.leftButton.isPressed)
        {
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000, board) )
            {
                var temp = hit.point;
                temp.y += 10;
                Dice.mousePos = temp;
            }
            
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            Dice.returnHand = false;
            Dice.throwDice = true;
            Debug.Log( "releasing dice");
        }
        
    }

    private IEnumerator startDice()
    {
        yield return new WaitForSeconds(0.1f);
        Dice.returnHand = true;
        Dice.Totalscore = 0;
    }

    private void Board()
    {
        if (!canClickOnTowers) return;
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = camera.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit, 1000, tower ) )
            {
                //Hit
                hit.collider.gameObject.GetComponent<TowerAi>().DisplayRange();
                lastTower = hit.collider.gameObject;
            }else if (Physics.Raycast(ray, out hit, 1000, board))
            {
                lastTower.GetComponent<TowerAi>().StopDisplayRange();
                
            }
            
        }
    }

    public void ToggleCanClickOnTowers()
    {
        canClickOnTowers = !canClickOnTowers;
        Debug.Log("CanClickOnTower: " +  canClickOnTowers.ToString());
    }
    
    public void ToggleAdding()
    {
        adding = !adding;
    }
}