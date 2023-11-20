using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TowerAi : MonoBehaviour
{
    [Header("Kind of Tower")] public int kindOfTower;
    public float slowEffect;
    public float rangeMulit;
    public float reduceCooldown;
    [Header("Physic Vars")] public Vector3 customVec;
    [SerializeField] private float range;
    [SerializeField] private float dmg;
    [SerializeField] private float shootingCooldown;
    [SerializeField] private float lives;
    private bool canShot = true;
    [Header("Refs")] public Transform uppberBody;
    public GameObject target;
    public Quaternion startRot;
    public GameObject missle;
    [Header("Behavior")] public bool shooting = false;
    private List<GameObject> entered;
    private bool countDownRunning = false;
    private LayerMask enemyLayer;
    public bool effect = false;
    public bool turn; // Start is called before the first frame update

    [Header("Shwoing")] public bool displayRange = false;

    private LineRenderer lr;

    void Start()
    {
        if (kindOfTower != 1)
        {
            lr = gameObject.GetComponent<LineRenderer>();
            lr.loop = true;
            this.GetComponent<CapsuleCollider>().radius = range;
        }

        if (turn)
        {
            startRot = uppberBody.rotation;
        }

        enemyLayer = LayerMask.GetMask("Enemies");
        entered = new List<GameObject>();


        if (kindOfTower == 11)
        {
            foreach (var ele in GameObject.FindGameObjectsWithTag("Tower"))
            {
                if ((ele.transform.position - transform.position).magnitude < range)
                {
                    ele.GetComponent<TowerAi>().range *= rangeMulit;
                    ele.GetComponent<TowerAi>().UpdateRange();
                }
            }
        }

        if (kindOfTower == 14)
        {
            foreach (var ele in GameObject.FindGameObjectsWithTag("Tower"))
            {
                if ((ele.transform.position - transform.position).magnitude < range)
                {
                    ele.GetComponent<TowerAi>().shootingCooldown *= 1 / reduceCooldown;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (kindOfTower != 1)
        {
            ChooseTarget();
        }
    }

    void FixedUpdate()
    {
        Shooting();
        if (kindOfTower != 1)
        {
            AimAt();
        }
        if (!canShot && !countDownRunning)
        {
            countDownRunning = true;
            StartCoroutine(ShootingCooldown());
        }
    }

    private void CheckForEnemies()
    {
    }

    private void AimAt()
    {
        if (!turn) return;
        if (shooting)
        {
            if (target != null)
            {
                uppberBody.LookAt(target.transform.position, Vector3.right);
                Quaternion finalRot = uppberBody.localRotation * this.GetComponent<Rigidbody>().rotation *
                                      Quaternion.AngleAxis(90, Vector3.down);
                uppberBody.rotation = finalRot;
            }
        }
        // uppberBody.LookAt(target);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            entered.Add(other.gameObject);
            if (target == null) target = other.gameObject;
            if (kindOfTower == 7)
            {
                other.gameObject.GetComponent<RouteMovement>().movementSpeed /= slowEffect;
            }
            if (kindOfTower == 1)
            {
                other.gameObject.GetComponent<RouteMovement>().movementSpeed = 0;
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            entered.Remove(other.gameObject);
            CheckTarget();
            if (kindOfTower == 7)
            {
                other.gameObject.GetComponent<RouteMovement>().movementSpeed *= slowEffect;
            }


        }
    }

    private void Shooting()
    {
        if (effect && canShot)
        {
            if (kindOfTower == 10)
            {
                foreach (var ele in entered)
                {
                    ele.GetComponent<EnemyAi>().TakeDmg(dmg);
                    canShot = false;
                }
            }

            if (kindOfTower == 1)
            {
                
                int enemies = entered.Count;
                if (enemies == 0) return;
                lives -= enemies * 1;
                if (lives <= 0)
                {
                    foreach (var ele in entered)
                    {
                        var temp = ele.GetComponent<RouteMovement>();
                        temp.movementSpeed = temp.originalMovementSpeed;
                    }

                    Destroy(gameObject);
                }
                canShot = false;
            }
        }
        else
        {
            if (canShot && target != null && shooting)
            {
                var newObject = Instantiate(missle, uppberBody.position, Quaternion.identity);
                newObject.GetComponent<Missile>().direction = target.transform.position;
                newObject.transform.rotation =
                    transform.rotation * uppberBody.rotation * Quaternion.AngleAxis(90, Vector3.down);
                newObject.transform.localScale *= 3;
                target.GetComponent<EnemyAi>().TakeDmg(dmg);
                canShot = false;
            }
        }
    }

    private IEnumerator ShootingCooldown()
    {
        yield return new WaitForSeconds(shootingCooldown);
        canShot = true;
        countDownRunning = false;
        Debug.Log("can shot again");
    }

    private void ChooseTarget()
    {
        if (target == null)
        {
            if (entered.Count != 0) target = entered[0];
        }
        else if (entered.Count != 0)
        {
            target = entered[0];
            shooting = true;
            if (!countDownRunning)
            {
                StartCoroutine(ShootingCooldown());
                countDownRunning = true;
            }
        }
    }

    private void CheckTarget()
    {
        if (!entered.Contains(target))
        {
            if (entered.Count == 0)
            {
                StopCoroutine(ShootingCooldown());
                target = null;
                uppberBody.rotation = startRot;
                shooting = false;
            }
            else target = entered[0].gameObject;
        }
    }

    public void DisplayRange()
    {
        int steps = 150;
        lr.positionCount = steps;
        for (int i = 0; i < steps; i++)
        {
            float cicrum = (float)i / steps;
            double radian = cicrum * 2 * Math.PI;

            float xScale = Mathf.Cos((float)radian);
            float ySacel = Mathf.Sin((float)radian);

            float x = xScale * range * 3;
            float y = ySacel * range * 3;

            Vector3 currentPosition = new Vector3(x + transform.position.x, 5, y + transform.position.z);

            lr.SetPosition(i, currentPosition);
        }
    }

    public void StopDisplayRange()
    {
        lr.positionCount = 0;
    }

    public void UpdateRange()
    {
        this.GetComponent<CapsuleCollider>().radius = range;
    }

    public void RemoveGameObject(GameObject myObject)
    {
        if (entered.Contains(myObject)) entered.Remove(myObject);
    }

    public void checkIfTouching()
    {
    }
}