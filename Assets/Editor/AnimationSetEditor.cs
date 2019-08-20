using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;

[CustomEditor(typeof(AnimationSet))]
public class AnimationSetEditor : OdinEditor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if (GUILayout.Button("Load Sprites"))
        {
            AnimationSet animationSet = (AnimationSet)target;
            animationSet.Sprites = new Dictionary<string, List<Sprite>>();

            if (animationSet.Textures != null)
            {
                foreach (var kv in animationSet.Textures)
                {
                    string spriteSheet = AssetDatabase.GetAssetPath(kv.Value);
                    Sprite[] sprites = AssetDatabase.LoadAllAssetsAtPath(spriteSheet)
                        .OfType<Sprite>().ToArray();
                    animationSet.Sprites[kv.Key] = new List<Sprite>(sprites);
                }
            }
        }
        
    }
}
