using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform Follow;
    public Transform RegionsTransform;

    public List<BoundingBox> Regions;

    public List<Transform> BGLayers;
    public List<float> parallaxAmounts;

    BoundingBox CurrentRegion;

    Vector2 CurrentViewTarget, CurrentView, PreviousViewTarget;

    float width, height;

    public static CameraManager Instance;
    void Start()
    {
        Instance = this;
        foreach (Transform row in RegionsTransform)
        {
            foreach (Transform child in row)
            {
                BoundingBox region = child.GetComponent<BoundingBox>();
                Debug.Log($"Adding Region({region.Left}, {region.Right}, {region.Bottom}, {region.Top}), transform: {Follow.transform.position}");
                Regions.Add(region);
            }
        }

        height = GetComponent<Camera>().orthographicSize * 2;
        width = 16 * height / 9;

        foreach (BoundingBox region in Regions)
        {
            //Debug.Log($"Checking: Region({region.Left}, {region.Right}, {region.Bottom}, {region.Top}), transform: {Follow.transform.position}");
            if (region.IsInside(Follow.transform.position))
            {
                CurrentRegion = region;
                break;
            }
        }

        CurrentViewTarget.x = Follow.position.x;
        CurrentViewTarget.y = Follow.position.y;

        CurrentViewTarget.x = Mathf.Clamp(CurrentViewTarget.x, CurrentRegion.GlobalLeft + width / 2, CurrentRegion.GlobalRight - width / 2);
        CurrentViewTarget.y = Mathf.Clamp(CurrentViewTarget.y, CurrentRegion.GlobalBottom + height / 2, CurrentRegion.GlobalTop - height / 2);

        CurrentView = CurrentViewTarget;

        transform.position = new Vector3(CurrentView.x, CurrentView.y, transform.localPosition.z);
    }

    public bool Transitioning;

    float TransitionTimer;
    const float TransitionTime = .5f;

    void Update()
    {
        foreach(BoundingBox region in Regions)
        {
            //Debug.Log($"Checking: Region({region.Left}, {region.Right}, {region.Bottom}, {region.Top}), transform: {Follow.transform.position}");
            if (region.IsInside(Follow.transform.position))
            {
                if (region != CurrentRegion)
                {
                    PreviousViewTarget = CurrentViewTarget;
                    CurrentRegion = region;
                    Transitioning = true;
                    TransitionTimer = TransitionTime;
                    ResetScene();
                    break;
                }
            }
        }
        CurrentViewTarget.x = Follow.position.x;
        CurrentViewTarget.y = Follow.position.y;

        CurrentViewTarget.x = Mathf.Clamp(CurrentViewTarget.x, CurrentRegion.GlobalLeft + width/2, CurrentRegion.GlobalRight - width/2);
        CurrentViewTarget.y = Mathf.Clamp(CurrentViewTarget.y, CurrentRegion.GlobalBottom + height/2, CurrentRegion.GlobalTop - height/2);

        
        if (Transitioning)
        {
            TransitionTimer = Mathf.Max(TransitionTimer - Time.deltaTime, 0);
            float ratio = 1f - (TransitionTimer / TransitionTime);
            CurrentView = Vector2.Lerp(PreviousViewTarget, CurrentViewTarget, Mathf.Sin(ratio * Mathf.PI / 2f));
            if(TransitionTimer <= 0f)
            {
                Transitioning = false;
            }
        }
        else
        {
            CurrentView = Vector2.Lerp(CurrentView, CurrentViewTarget, 0.1f * Time.deltaTime * 60);
        }

        transform.position = new Vector3(CurrentView.x, CurrentView.y, transform.localPosition.z);


        for(int i = 0; i < BGLayers.Count; i++)
        {
            Transform layer = BGLayers[i];
            float parallaxAmount = parallaxAmounts[i];
            Vector2 layerPos = new Vector2(transform.position.x, transform.position.y) * parallaxAmount;
            layer.position = new Vector3(layerPos.x, layerPos.y, layer.position.z);
        }
    }

    public void ResetScene()
    {
        
    }
}
