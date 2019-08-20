using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    public GameObject LeftThruster;
    public GameObject RightThruster;
    public GameObject TopThruster;
    public GameObject BottomThruster;

    public GameObject LeftLaser;
    public GameObject RightLaser;
    public GameObject TopLaser;
    public GameObject BottomLaser;

    //public GameObject LaserParticles;

    Actor Actor;

    Vector2Int Inputs;
    Vector2 Velocity;

    const float Speed = 90f;
    const float Accel = 90f;

    void Awake()
    {
        Actor = GetComponent<Actor>();
    }

    void Start()
    {
        LeftThruster.SetActive(false);
        RightThruster.SetActive(false);
        TopThruster.SetActive(false);
        BottomThruster.SetActive(false);
	}

    void Update()
    {
        Inputs = Vector2Int.zero;
        
        if (Input.GetKey(KeyCode.LeftArrow)) Inputs.x--;
        if (Input.GetKey(KeyCode.RightArrow)) Inputs.x++;
        if (Input.GetKey(KeyCode.UpArrow)) Inputs.y++;
        if (Input.GetKey(KeyCode.DownArrow)) Inputs.y--;
        
        RightThruster.SetActive(Inputs.x < 0);
        LeftThruster.SetActive(Inputs.x > 0);
        BottomThruster.SetActive(Inputs.y > 0);
        TopThruster.SetActive(Inputs.y < 0);

        if (Input.GetKey(KeyCode.A))
        {
            LeftLaser.SetActive(true);
        }
        else
        {
            LeftLaser.SetActive(false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            RightLaser.SetActive(true);
        }
        else
        {
            RightLaser.SetActive(false);
        }
        if (Input.GetKey(KeyCode.W))
        {
            TopLaser.SetActive(true);
        }
        else
        {
            TopLaser.SetActive(false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            BottomLaser.SetActive(true);
        }
        else
        {
            BottomLaser.SetActive(false);
        }

        if (Input.GetKey(KeyCode.X))
        {
            Inputs = Vector2Int.zero;
        }


		Velocity.x = Velocity.x + Accel * Clock.Instance.DeltaTime * Inputs.x;
		Velocity.y = Velocity.y + Accel * Clock.Instance.DeltaTime * Inputs.y;

		if (Mathf.Abs(Velocity.x) < 0.99f * Accel * Clock.Instance.DeltaTime)
			Velocity.x = 0;
		if (Mathf.Abs(Velocity.y) < 0.99f * Accel * Clock.Instance.DeltaTime)
            Velocity.y = 0;


		//Velocity.x = Calc.Approach(Velocity.x, Speed * Inputs.x, Accel * Clock.Instance.DeltaTime);
		//Velocity.y = Calc.Approach(Velocity.y, Speed * Inputs.y, Accel * Clock.Instance.DeltaTime);

		Actor.MoveX(Velocity.x * Clock.Instance.DeltaTime, OnCollideH);
        Actor.MoveY(Velocity.y * Clock.Instance.DeltaTime, OnCollideV);
    }

    public void OnCollideH(Solid solid)
    {
        Velocity.x *= -0.5f;
    }

    public void OnCollideV(Solid solid)
    {
        Velocity.y *= -0.5f;
    }
}

public static class Calc
{
    public static float Approach(float start, float target, float amount)
    {
        amount = Mathf.Abs(amount);
        if (target >= start)
        {
            return Mathf.Min(target, start + amount);
        }
        else
        {
            return Mathf.Max(target, start - amount);
        }
    }
}
