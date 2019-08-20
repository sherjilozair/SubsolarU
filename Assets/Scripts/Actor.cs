using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoundingBox))]
public class Actor : MonoBehaviour
{
    private const int MaxMove = 100;
    Vector2 Remainder;

    [HideInInspector]
    public BoundingBox BoundingBox;

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


    void Awake()
    {
        BoundingBox = GetComponent<BoundingBox>();
    }

    public void MoveX(float amount, Action<Solid> onCollide = null, Space space = Space.World)
    {
        if (Math.Sign(amount) != Math.Sign(Remainder.x))
            Remainder.x = 0;
        Remainder.x += amount;
        //Debug.Log($"moving amount: {amount}, after Remainder.x: {Remainder.x}");

        int move = (int) (Remainder.x);
        //Debug.Log($"Move: {move}");
        if (Mathf.Abs(move) > MaxMove)
        {
            //Debug.Log("Moving naively");
            transform.Translate(move * Vector3.right, space);
            Remainder.x -= move;
            return;
        }

        //Debug.Log($"Moving normally");

        if (move != 0)
        {
            //Debug.Log($"moving move: {move}");
            Remainder.x -= move;
            int sign = Step(move);

            while (move != 0)
            {
                Solid solid = SolidCheckAt(new Vector2(sign, 0));
                if (solid is null)
                {
                    //Debug.Log("no collision");
                    transform.Translate(Vector3.right * sign, space);
                    move -= sign;
                }
                else
                {
                    //Debug.Log("collision");
                    Remainder.x = 0;
                    onCollide?.Invoke(solid);
                    break;
                }
            }
        }
    }

    public void MoveY(float amount, Action<Solid> onCollide = null, Space space = Space.World)
    {
        if (Math.Sign(amount) != Math.Sign(Remainder.y))
            Remainder.y = 0;
        Remainder.y += amount;
        int move = (int) (Remainder.y);
        //Debug.Log($"Move: {move}");
        if (Mathf.Abs(move) > MaxMove)
        {
            //Debug.Log("Moving naively");
            transform.Translate(move * Vector3.up, space);
            Remainder.y -= move;
            return;
        }
        //Debug.Log($"Moving normally");

        if (move != 0)
        {
            Remainder.y -= move;
            int sign = Step(move);

            while (move != 0)
            {
                Solid solid = SolidCheckAt(new Vector2(0, sign));
                if (solid is null)
                {
                    transform.Translate(Vector3.up * sign, space);
                    move -= sign;
                }
                else
                {
                    Remainder.y = 0;
                    onCollide?.Invoke(solid);
                    break;
                }
            }
        }
    }

    public Solid SolidCheckAt(Vector2 offset) {
        Vector3 position = transform.position + new Vector3(offset.x,offset.y);

        foreach (Solid solid in FindObjectsOfType<Solid>())
        {
            if (!solid.Collidable) continue;
            if (BoundingBox.Intersects(position, solid.BoundingBox, solid.transform.position))
            {
                // Debug.Log($"Collision: {Bottom}, {solid.Top}");
                return solid;
            }
        }
        return null;
    }

    public IEnumerable<Actor> ActorCheckAt(Vector2 offset)
    {
        Vector3 position = transform.position + new Vector3(offset.x, offset.y);

        foreach (Actor actor in FindObjectsOfType<Actor>())
        {
            if (actor == this) continue;
            if (BoundingBox.Intersects(position, actor.BoundingBox, actor.transform.position))
            {
                // Debug.Log($"Collision: {Bottom}, {solid.Top}");
                yield return actor;
            }
        }
    }

    public bool IsRiding(Solid solid)
    {
        Vector3 position = transform.position + new Vector3(0, -1);
        return BoundingBox.Intersects(position, solid.BoundingBox, solid.transform.position);
    }

    public void Squish()
    {
        Destroy(gameObject);
    }

    int Step(int amount)
    {
        return Math.Sign(amount);
    }

    public void Reduce(int offsetLeft, int offsetRight, int offsetTop, int offsetBottom)
    {
        BoundingBox.OffsetLeft = offsetLeft;
        BoundingBox.OffsetRight = offsetRight;
        BoundingBox.OffsetTop = offsetTop;
        BoundingBox.OffsetBottom = offsetBottom;
    }

    public void ResetOffsets()
    {
        while(BoundingBox.OffsetLeft > 0)
        {
            Solid solid = SolidCheckAt(new Vector2(-1, 0));
            if (solid != null)
                transform.Translate(Vector3.right, Space.World);
            BoundingBox.OffsetLeft--;
        }

        while (BoundingBox.OffsetRight > 0)
        {
            Solid solid = SolidCheckAt(new Vector2(1, 0));
            if (solid != null)
                transform.Translate(Vector3.left, Space.World);
            BoundingBox.OffsetRight--;
        }
        while (BoundingBox.OffsetTop > 0)
        {
            Solid solid = SolidCheckAt(new Vector2(0, 1));
            if (solid != null)
                transform.Translate(Vector3.down, Space.World);
            BoundingBox.OffsetTop--;
        }

        while (BoundingBox.OffsetBottom > 0)
        {
            Solid solid = SolidCheckAt(new Vector2(0, -1));
            if (solid != null)
                transform.Translate(Vector3.up, Space.World);
            BoundingBox.OffsetBottom--;
        }
        if (SolidCheckAt(Vector2.zero) != null)
        {
            Squish();
        }
    }
}