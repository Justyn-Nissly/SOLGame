using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class droneMovementPolar : MonoBehaviour
{
    const float
        X_CTR   = -11.0f, // x-value of the drone's center of rotation
        Y_CTR   = 20.0f,  // y-value of the drone's center of rotation
        X_COEFF = 4.0f,   // the x's coefficient
        Y_COEFF = 4.0f,   // the y's coefficient
        PI      = 3.14159265358979323846f;
                          // mathematical constant pi

    private float
        angle = 0.0f;   // the current angle of rotation

    private
        Vector2 movement;

    private
        Rigidbody2D drone;

    bool
        canMove = true;

    /// <summary>
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        drone = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        // the drones's movement angle cannot reach 360 degrees
        if (angle >= 360.0f)
            angle = 0.0f;
        
        // move the drone along the ellipse
        // different values for X/Y_COEFF will change the width/height of
        // the ellipse; different values for X/Y_CTR will change the center
        // of the drone's axis (moves it to a different place)
        drone.transform.position =
            new Vector2(X_COEFF * Cos(angle * PI / 180.0f) + X_CTR,
                        Y_COEFF * Sin(angle * PI / 180.0f) + Y_CTR);
        angle                   += 1.0f;
    }

    // not currently used
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canMove = false;
    }

    // not currently used
    private void OnTriggerExit2D(Collider2D collision)
    {
        canMove = false;
    }

    // square root function
    public static float sqrt(float number, float guess = 1.0f)
    {
        int i;

        for (i = 1; i <= 100; i++)
            guess = 0.5f * (guess + number / guess);
        return guess;
    }

    // sine helps determine the drone's y-location
    public static float Sin(float number)
    {
        float
            sine = 0.0f,
            part;
        int
            counter1,
            counter2;

        for (counter1 = 1; counter1 < 100; counter1 += 2)
        {
            part = 1.0f;
            for (counter2 = 1; counter2 <= counter1; counter2++)
            {
                part *= number;
                part /= counter2;
            }
            if (counter1 % 4 == 1)
                sine += part;
            else
                sine -= part;
        }

        return sine;
    }

    // cosine helps determine the drone's x-location
    public static float Cos(float number)
    {
        return Sin(PI / 2.0f - number);
    }
}