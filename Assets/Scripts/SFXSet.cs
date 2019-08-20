using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "Data/SFXSet")]
public class SFXSet : SerializedScriptableObject
{
    public Dictionary<string, List<AudioClip>> sources;

    public void Play(string id, AudioSource source)
    {
        // Debug.Log($"Playing: {id}");
        List<AudioClip> clips = sources[id];
        int index = Random.Range(0, clips.Count-1);
        source.Stop();
        source.PlayOneShot(clips[index]);
    }
}
