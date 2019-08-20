using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    public int X;
    public int Y;
    public int Width = 16;
    public int Height = 16;
    public int OffsetLeft;
    public int OffsetRight;
    public int OffsetTop;
    public int OffsetBottom;

    public int Left { get { return X + OffsetLeft; } }
    public int Right { get { return X + Width - OffsetRight; } }
    public int Bottom { get { return Y + OffsetBottom; } }
    public int Top { get { return Y + Height - OffsetTop; } }

    public int GlobalLeft { get { return (int)transform.position.x + X + OffsetLeft; } }
    public int GlobalRight { get { return (int)transform.position.x + X + Width - OffsetRight; } }
    public int GlobalBottom { get { return (int)transform.position.y + Y + OffsetBottom; } }
    public int GlobalTop { get { return (int)transform.position.y + Y + Height - OffsetTop; } }


    public bool GlobalIntersects(BoundingBox value)
    {
        return (value.GlobalLeft < GlobalRight &&
                GlobalLeft < value.GlobalRight &&
                value.GlobalTop > GlobalBottom &&
                GlobalTop > value.GlobalBottom);
    }

    public bool Intersects(BoundingBox value)
    {
        return (value.Left < Right &&
                Left < value.Right &&
                value.Top > Bottom &&
                Top > value.Bottom);
    }

    public bool IsInside(Vector2 position)
    {
        return position.x > GlobalLeft && position.x < GlobalRight && position.y > GlobalBottom && position.y < GlobalTop;
    }

    public bool Intersects(Vector2 offset, BoundingBox other)
    {
        return (other.Left < Right + offset.x &&
                Left + offset.x < other.Right &&
                other.Top > Bottom + offset.y &&
                Top + offset.y > other.Bottom);
    }

    public bool Intersects(Vector2 offset, BoundingBox other, Vector2 otherOffset)
    {
        return (other.Left + otherOffset.x < Right + offset.x &&
                Left + offset.x < other.Right + otherOffset.x &&
                other.Top + otherOffset.y > Bottom + offset.y &&
                Top + offset.y > other.Bottom + otherOffset.y);
    }

    public override string ToString()
    {
        return $"BoundingBox({X}, {Y}, {Width}, {Height})";
    }
}