using System;
using UnityEngine;
using Random = System.Random;

public class Dice : MonoBehaviour
{
    [Header("Refs")] public Transform pointA;
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;
    public Transform p5;
    public Transform p6;


    [Header("Physics Vars")] public int seed;
    [SerializeField] private int minForce;
    [SerializeField] private int maxForce;
    [SerializeField] private int minSpin;
    [SerializeField] private int maxSpin;
    [SerializeField] private float extraGravity;

    [Header("Others")] public bool roll = true;
    public static int Totalscore;
    public int localScore;
    private MyPoint[] allPs;
    private Rigidbody rb;
    private bool addScoreOnce = false;

    private Vector3 startPoint;
    [SerializeField] private float diceInHandForce;
    public static Vector3 mousePos;

    public static bool returnHand = false;
    public static bool throwDice = false;

    private bool localThrowDice = false;
    // Start is called before the first frame update

    private class MyPoint
    {
        public float x;
        public int score;

        public MyPoint(Transform pos, int score)
        {
            this.x = pos.position.y;
            this.score = score;
        }

        public MyPoint()
        {
            this.x = -1;
            this.score = 0;
        }
    }

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
        startPoint = this.GetComponent<Transform>().position;
        if (roll) InitSpeed();
    }

    public void InitSpeed()
    {
        Random r;
        if (seed == 0) r = new Random();
        else r = new Random(seed);

        Vector3 direction = pointA.position - rb.position;
        rb.AddForce(direction * r.Next(minForce, maxForce));

        //Add random Spin
        Vector3 torque = new Vector3(r.Next(), r.Next(), r.Next());
        Debug.Log(torque);
        rb.AddTorque(torque * r.Next(minSpin, maxSpin));
    }

    // Update is called once per frame
    private void Update()
    {
        PickThemUp();
        CalcPoints();
    }

    public void FixedUpdate()
    {
        GetComponent<Rigidbody>().AddForce(Physics.gravity * extraGravity, ForceMode.Acceleration);
        
        // if (Dice.throwDice && !localThrowDice)
        // {
        //     Random r;
        //     if (seed == 0) r = new Random();
        //     else r = new Random(seed);
        //     Vector3 direction = pointA.position - rb.position;
        //     rb.AddForce(direction * r.Next(minForce, maxForce));
        //
        //     //Add random Spin
        //     Vector3 torque = new Vector3(r.Next(), r.Next(), r.Next());
        //     Debug.Log(torque);
        //     rb.AddTorque(torque * r.Next(minSpin, maxSpin));
        //
        //     localThrowDice = true;
        // }
    }

    public void CalcPoints()
    {
        if (Math.Abs(rb.velocity.magnitude) < 0.05 && !addScoreOnce)
            // Math.Abs((startPoint - rb.position).magnitude) > 1)
        {
            allPs = new MyPoint[]
            {
                new MyPoint(p1, 1),
                new MyPoint(p2, 2),
                new MyPoint(p3, 3),
                new MyPoint(p4, 4),
                new MyPoint(p5, 5),
                new MyPoint(p6, 6),
            };

            MyPoint highestP = new MyPoint();
            foreach (var point in allPs)
            {
                if (point.x > highestP.x) highestP = point;
            }

            Totalscore += highestP.score;
            localScore = highestP.score;
            addScoreOnce = true;
            
            Debug.Log(localScore);
            GameController.allDiceRolled();
        }
    }

    public void PickThemUp()
    {
        if (!returnHand) return;
        if (mousePos == Vector3.zero) return;
        Vector3 direction = mousePos - transform.position;
        rb.AddForce(direction * diceInHandForce);
        addScoreOnce = false;
    }


}