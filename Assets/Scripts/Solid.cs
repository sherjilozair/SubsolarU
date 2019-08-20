using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundingBox))]
public class Solid : MonoBehaviour
{
    [NonSerialized]
    public BoundingBox BoundingBox;

    Vector2 Remainder;

    [NonSerialized]
    public bool Collidable = true;

    void Awake()
    {
        BoundingBox = GetComponent<BoundingBox>();
    }

    public int Left
    {
        get
        {
            return BoundingBox.X + (int)transform.position.x;
        }
    }

    public int Right
    {
        get
        {
            return BoundingBox.X + BoundingBox.Width + (int)transform.position.x;
        }
    }

    public int Bottom
    {
        get
        {
            return BoundingBox.Y + (int)transform.position.y;
        }
    }

    public int Top
    {
        get
        {
            return BoundingBox.Y + BoundingBox.Height + (int)transform.position.y;
        }
    }


    static List<Actor> AllActors;

    void OnEnable()
    {
        if (AllActors == null)
        {
            AllActors = new List<Actor>();
            foreach (Actor actor in FindObjectsOfType<Actor>())
            {
                AllActors.Add(actor);
            }
        }    
    }

    public void Move(float x, float y)
    {
        Remainder.x += x;
        Remainder.y += y;
        int moveX = (int)(Remainder.x);
        int moveY = (int)(Remainder.y);

        // Debug.Log($"Moving {x}, {y}");

        if (moveX != 0 || moveY != 0)
        {
            //Loop through every Actor in the Level, add it to
            //a list if actor.IsRiding(this) is true
            List<Actor> riding = GetAllRidingActors();

            //Make this Solid non-collidable for Actors,
            //so that Actors moved by it do not get stuck on it
            Collidable = false;

            if (moveX != 0)
            {
                Remainder.x -= moveX;
                transform.Translate(Vector3.right * moveX, Space.World);

                if (moveX > 0)
                {
                    foreach (Actor actor in AllActors)
                    {
                        if (OverlapCheck(actor))
                        {
                            float amount = this.Right - actor.Left;
                            // Debug.Log($"before Actor.left: {actor.Left}");
                            actor.MoveX(amount, (Solid solid) => { actor.Squish(); });
                            // Debug.Log($"after Actor.left: {actor.Left}");
                            // Debug.Log($"Pushing right by {amount} and overlap check: {OverlapCheck(actor)}");
                            //Debug.Break();
                        }
                        else if (riding.Contains(actor))
                        {
                            // Debug.Log("Carrying Right");
                            actor.MoveX(moveX, null);
                            //Debug.Break();
                        }
                    }
                }
                else
                {
                    foreach (Actor actor in AllActors)
                    {
                        if (OverlapCheck(actor))
                        {
                            actor.MoveX(this.Left - actor.Right, (Solid solid) => { actor.Squish(); });
                            // Debug.Log($"Pushing left: {OverlapCheck(actor)}");
                        }
                        else if (riding.Contains(actor))
                        {
                            // Debug.Log("Carrying left");
                            actor.MoveX(moveX, null);
                            //Debug.Break();
                        }
                    }
                }
            }

            if (moveY != 0)
            {
                Remainder.y -= moveY;
                transform.Translate(Vector3.up * moveY, Space.World);

                if (moveY > 0)
                {
                    foreach (Actor actor in AllActors)
                    {
                        if (OverlapCheck(actor))
                        {
                            float amount = this.Top - actor.Bottom;
                            //Debug.Log($"actor.Bottom before: {actor.Bottom}, amount: {amount}");
                            actor.MoveY(amount, (Solid solid) => { actor.Squish(); });
                            //Debug.Log($"actor.Bottom after: {actor.Bottom}, amount: {amount}");
                            //Debug.Log("Actor overlapped");
                        }
                        else if (riding.Contains(actor))
                        {
                            // Debug.Log("Carrying top");
                            actor.MoveY(moveY, null);
                            // Debug.Break();
                            //Debug.Log("Actor riding");
                        }
                        else
                        {
                            //Debug.Log("Actor not interacting");
                        }
                    }
                }
                else
                {
                    foreach (Actor actor in AllActors)
                    {
                        if (OverlapCheck(actor))
                        {
                            actor.MoveY(this.Bottom - actor.Top, (Solid solid) => { actor.Squish(); });
                            // Debug.Log($"Pushing bottom: {OverlapCheck(actor)}");
                        }
                        else if (riding.Contains(actor))
                        {
                            // Debug.Log("carrying bottom");
                            actor.MoveY(moveY, null);
                            //Debug.Break();
                        }
                    }
                }
            }

            //Re-enable collisions for this Solid
            Collidable = true;
        }
    }

    public bool OverlapCheck(Actor actor)
    {
        return BoundingBox.Intersects(transform.position, actor.BoundingBox, actor.transform.position);
    }

    private List<Actor> GetAllRidingActors()
    {
        List<Actor> actors = new List<Actor>();
        foreach (Actor actor in AllActors)
        {
            if (actor.IsRiding(this))
            {
                actors.Add(actor);
            }
        }
        // Debug.Log($"Number of riding actors: {actors.Count}");
        return actors;
    }
}