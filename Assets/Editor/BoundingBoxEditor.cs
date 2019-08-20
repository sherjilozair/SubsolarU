using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BoundingBox)), CanEditMultipleObjects]
public class BoundingBoxEditor : Editor
{
    BoundingBox BoundingBox;

    void OnSceneGUI()
    {
        BoundingBox = (BoundingBox)target;
        
        Vector3 bottomLeft = BoundingBox.transform.position + new Vector3(BoundingBox.X, BoundingBox.Y);
        Vector3 bottomRight = BoundingBox.transform.position + new Vector3(BoundingBox.X + BoundingBox.Width, BoundingBox.Y);
        Vector3 topRight = BoundingBox.transform.position + new Vector3(BoundingBox.X + BoundingBox.Width, BoundingBox.Y + BoundingBox.Height);
        Vector3 topleft = BoundingBox.transform.position + new Vector3(BoundingBox.X, BoundingBox.Y + BoundingBox.Height);

        Vector3[] verts = {
            bottomLeft,
            topleft,
            topRight,
            bottomRight,
        };

        Handles.DrawSolidRectangleWithOutline(verts, new Color(0.5f, 0.5f, 0.5f, 0.1f), new Color(0.7f, 0.1f, 0.1f, 1));

        
        EditorGUI.BeginChangeCheck();
        Handles.color = Handles.xAxisColor;
        Vector3 newBottomLeft = Handles.Slider2D(bottomLeft, Vector3.forward, Vector3.up, Vector3.right, HandleUtility.GetHandleSize(bottomLeft) / 10, Handles.DotHandleCap, new Vector2(1, 1)) - BoundingBox.transform.position;
        Handles.color = Handles.zAxisColor;
        Vector3 newTopRight = Handles.Slider2D(topRight, Vector3.forward, Vector3.up, Vector3.right, HandleUtility.GetHandleSize(topRight) / 10, Handles.DotHandleCap, new Vector2(1, 1)) - BoundingBox.transform.position;

        if (EditorGUI.EndChangeCheck())
        {
            Undo.RecordObject(target, "Changed Bounding Box");
            BoundingBox.X = Mathf.RoundToInt(newBottomLeft.x);
            BoundingBox.Y = Mathf.RoundToInt(newBottomLeft.y);

            Vector3 sizeDelta = newTopRight - newBottomLeft;
            BoundingBox.Width = Mathf.Max(Mathf.RoundToInt(sizeDelta.x), 0);
            BoundingBox.Height = Mathf.Max(Mathf.RoundToInt(sizeDelta.y), 0);
        }

        if (BoundingBox.transform.hasChanged)
        {
            BoundingBox.transform.position = new Vector3((int)BoundingBox.transform.position.x, (int)BoundingBox.transform.position.y, BoundingBox.transform.position.z);
            BoundingBox.transform.hasChanged = false;
        }
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("Normalize"))
        {
            BoundingBox boundingBox = (BoundingBox)target;

            boundingBox.transform.position = new Vector3((int)(boundingBox.transform.position.x + boundingBox.X), (int)(boundingBox.transform.position.y + boundingBox.Y), boundingBox.transform.position.z);

            boundingBox.X = 0;
            boundingBox.Y = 0;
            Debug.Log("Normalize");
        }
    }
}
