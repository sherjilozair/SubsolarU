using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    Solid Solid;
    Vector2 Velocity;

    public float Speed = 15;

    void Start()
    {
        Solid = GetComponent<Solid>();
        Velocity = UnityEngine.Random.insideUnitCircle * Random.Range(0, Speed);
    }


    void Update()
    {
        Solid.Move(
            Velocity.x * Clock.Instance.DeltaTime,
            Velocity.y * Clock.Instance.DeltaTime);
    }
}
