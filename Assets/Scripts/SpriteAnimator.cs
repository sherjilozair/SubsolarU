using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SpriteAnimator : SerializedMonoBehaviour
{

    [NonSerialized]
    public SpriteRenderer SpriteRenderer;

    public AnimationSet AnimationSet;

    public bool Pause;

    List<Sprite> sprites;
    int currentSprite;
    float secondsPerFrame;

    [ShowInInspector]
    string currentAnimation;
    float Timer;

    public bool Flip;

    void Awake()
    {
        if (SpriteRenderer == null)
        {
            SpriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (currentAnimation == null || currentAnimation == "")
        {
            Timer = 0f;
            currentAnimation = AnimationSet.DefaultAnimation;
            currentSprite = 0;
            sprites = AnimationSet.Sprites[currentAnimation];
            Debug.Log($"Loading {currentAnimation}");
            secondsPerFrame = 1.0f / AnimationSet.FramesPerSecond[currentAnimation];
            SpriteRenderer.sprite = sprites[currentSprite];
        }
    }

    public void Set(string id, Action onComplete = null) {
        Set(id, Flip, onComplete);
    }

    public void SetFlip(bool flip)
    {
        Flip = flip;
        SpriteRenderer.flipX = Flip;
    }

    public void RandomizeStartIndex()
    {
        currentSprite = UnityEngine.Random.Range(0, sprites.Count - 1);
        SpriteRenderer.sprite = sprites[currentSprite];
        SpriteRenderer.flipX = Flip;
    }

    Action OnComplete;
    public void Set(string id, bool flip, Action onComplete)
    {
        if (currentAnimation != id || Flip != flip)
        {
            // Debug.Log($"Changing animation {id}");
            Flip = flip;
            currentAnimation = id;
            sprites = AnimationSet.Sprites[id];
            currentSprite = 0;
            Timer = 0f;
            Debug.Log($"Loading {currentAnimation}");
            secondsPerFrame = 1.0f / AnimationSet.FramesPerSecond[currentAnimation];
            SpriteRenderer.sprite = sprites[currentSprite];
            SpriteRenderer.flipX = Flip;

            OnComplete = onComplete;
        }
    }

    void Update()
    {
        // if (CameraManager.Instance.Transitioning || Pause) return;
        Timer += Clock.Instance.DeltaTime;
        if (Timer > secondsPerFrame)
        {
            //Debug.Log($"Changing Sprite. secondsPerFrame: {secondsPerFrame}");
            Timer -= secondsPerFrame;
            currentSprite++;
            
            if (currentSprite == sprites.Count)
            {
                currentSprite = 0;
                OnComplete?.Invoke();
            }

            SpriteRenderer.sprite = sprites[currentSprite];
            SpriteRenderer.flipX = Flip;
        }
        
    }
}
