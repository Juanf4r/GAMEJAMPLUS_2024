using System;
using System.Collections;
using System.Collections.Generic;
using _ScriptableObjects.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class AnimationToImagePU : MonoBehaviour
{
    public static AnimationToImagePU Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    [SerializeField] private List<Sprite> TPAnimation;
    [SerializeField] private List<Sprite> SpeedAnimation;
    [SerializeField] private List<Sprite> MazeAnimation;
    [SerializeField] private Image targetImagePlayer1;
    [SerializeField] private Image targetImagePlayer2;
    [SerializeField] private float animationSpeed = 0.2f;

    private Coroutine coroutinePlayer1;
    private Coroutine coroutinePlayer2;

    public void SelectPO(PowerUpSo powerUp, bool isPlayer1)
    {
        Debug.LogWarning("Animation to UI");

        List<Sprite> selectedAnimation = powerUp.buffType switch
        {
            PowerUpType.Teleport => TPAnimation,
            PowerUpType.Movement => SpeedAnimation,
            PowerUpType.Strength => MazeAnimation,
            _ => throw new ArgumentOutOfRangeException()
        };

        if (isPlayer1)
        {
            if (coroutinePlayer1 != null) StopCoroutine(coroutinePlayer1);
            coroutinePlayer1 = StartCoroutine(PlayAnimation(targetImagePlayer1, selectedAnimation));
        }
        else
        {
            if (coroutinePlayer2 != null) StopCoroutine(coroutinePlayer2);
            coroutinePlayer2 = StartCoroutine(PlayAnimation(targetImagePlayer2, selectedAnimation));
        }
    }

    private IEnumerator PlayAnimation(Image targetImage, List<Sprite> animationSprites)
    {
        int index = 0;

        while (animationSprites.Count > 0)
        {
            if (targetImage != null)
            {
                targetImage.sprite = animationSprites[index];
                index = (index + 1) % animationSprites.Count;
            }
            yield return new WaitForSeconds(animationSpeed);
        }
    }
}
