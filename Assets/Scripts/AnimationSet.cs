using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/AnimationSet")]
public class AnimationSet : SerializedScriptableObject
{
    public Dictionary<string, Texture2D> Textures;

    public Dictionary<string, List<Sprite>> Sprites;

    public Dictionary<string, int> FramesPerSecond;

    public string DefaultAnimation;
}


