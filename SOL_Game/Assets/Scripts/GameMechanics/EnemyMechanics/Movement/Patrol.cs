using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Patrol : MonoBehaviour
{
    #region Enums
    public enum Movement
    {
        MoveTowards,
        LerpTowards
    }
    #endregion

    #region Public Variables
    public Movement     Type = Movement.MoveTowards; // The movement type that will be used
    public MovementPath enemyPath;                   // The path the enemy will follow
    public float speed;                              // The speed the enemy will be moving at
    public float maxDistanceFromPoint = 0.1f;        // The maximum distance an enemy can be to be be considered to be at a point on its path

    #endregion

    #region Private Variables
    private IEnumerator<Transform> pointInPath; // 
    #endregion

    // Unity Named Methods
    #region Main Methods
    void Start()
    {
        if (enemyPath == null) 
        {
            
        }
    }
    #endregion

    #region Utility Methods

    #endregion

    #region Coroutines

    #endregion
}

/*
 *     public float speed;
     private float waitTime;
     public float startWaitTime;

     public Transform[] moveSpots;
     public int randomSpot;
     public bool returnRoute;
     // Start is called before the first frame update
     void Start()
     {
         returnRoute = true;
         waitTime   = startWaitTime;
         randomSpot = 0;
     }

     // Update is called once per frame
     void Update()
     {
         transform.position = Vector2.MoveTowards(transform.position, moveSpots[randomSpot].position, speed * Time.deltaTime);
         if(Vector2.Distance(transform.position, moveSpots[randomSpot].position) < 0.2f)
         {
            if (returnRoute)
            {
                if (waitTime <= 0)
                {
                    if (randomSpot == 0)
                    {
                        randomSpot = randomSpot + 1;
                    }
                    else
                    {
                        randomSpot = (int)(Random.Range(randomSpot + 1, randomSpot - 1));
                    }
                    waitTime = startWaitTime;
                }
                else
                {
                    waitTime -= Time.deltaTime;
                }
            }
         }
     }
}
    public float  speed,     // The enemy patrol speed
                  startWaitTime;
    public int stop;
    private float waitTime;
    public bool   moveRight,  // Should the enemy move right
                  moveLeft,   // Should the enemy move left
                  moveUp,     // Should the enemy move up
                  moveDown;   // Should the enemy move down

    void Start()
    {
        waitTime = startWaitTime;
        stop = 0;
    }
    void Update()
    {
        if (moveRight)
        {
            GetWaitTime();
            transform.Translate(2 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector2(2, 2);
        }
        else
        {
            GetWaitTime();
            transform.Translate(-2 * Time.deltaTime * speed, 0, 0);
            transform.localScale = new Vector2(-2, 2);
        }
    }

    private void OnTriggerEnter2D(Collider2D movePoint)
    {
        Time.timeScale = stop;
        if (waitTime <= 0)
        {
            waitTime = startWaitTime;
        }
        else
        {
            waitTime -= Time.deltaTime;
            Time.timeScale = 1;
        }
        if (movePoint.gameObject.CompareTag("move"))
        {
            if (moveRight)
            {
                moveRight = false;
            }
            else
            {
                moveRight = true;
            }
        }
    }

    void GetWaitTime()
    {
        if (waitTime <= 0)
        {
            waitTime = startWaitTime;
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
    }


*/
