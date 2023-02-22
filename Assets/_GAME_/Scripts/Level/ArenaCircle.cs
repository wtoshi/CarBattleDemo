using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ArenaCircle : MonoBehaviour
{
    [SerializeField] Transform obstaclesHolder;
    [SerializeField] Transform partsHolder;

    List<Transform> obstaclePositions = new List<Transform>();

    List<GameObject> circleParts = new List<GameObject>();

    public bool canBeDestroyed;

    Coroutine destroyCO;

    private void Awake()
    {
        Transform[] allChildren = partsHolder.GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            circleParts.Add(child.gameObject);
        }

        circleParts.RemoveAt(0);

        canBeDestroyed = true;
    }

    public void DestroyCircle(float destroyDuration)
    {
        if (!canBeDestroyed)
            return;

        destroyCO = StartCoroutine(doDestroyCircle(destroyDuration));
    }

    IEnumerator doDestroyCircle(float destroyDuration)
    {
        ArenaController.circleDestroying = true;
        canBeDestroyed = false;

        StartCoroutine(doBlink());

        yield return new WaitUntil(()=> blinkCompleted);

        var currentParts = circleParts;

        while (currentParts.Count > 0)
        {
            GameObject part = currentParts[Random.Range(0, currentParts.Count)];

            part.transform.DOMoveY(part.transform.position.y - 50, GameManager.Instance.GameSettings.circlePartFallingSpeed).SetSpeedBased().SetEase(Ease.Linear);

            var waitingTime = destroyDuration / currentParts.Count;

            currentParts.Remove(part);

            yield return new WaitForSeconds(waitingTime);
            
            destroyDuration -= waitingTime;

            yield return null;
        }

        ArenaController.circleDestroying = false;

    }

    bool blinkCompleted = false;
    IEnumerator doBlink()
    {
        float blinkSpeed = GameManager.Instance.GameSettings.circlePartBlinkSpeed;
        float duration = GameManager.Instance.GameSettings.circlePartBlinkDuration;

        float currentTime = 0;

        List<MeshRenderer> allRenderers = new List<MeshRenderer>();

        foreach (var part in circleParts)
        {
            var renderer = part.gameObject.GetComponent<MeshRenderer>();

            allRenderers.Add(renderer);
        }

        if (allRenderers.Count <= 0)
            yield break;

        while (currentTime <= duration)
        {
            foreach (var part in allRenderers)
            {
                part.material = GameManager.Instance.GameSettings.circlePartBlinkMat;
            }

            yield return new WaitForSeconds(blinkSpeed);

            foreach (var part in allRenderers)
            {
                part.material = GameManager.Instance.GameSettings.circlePartDefaultMat;
            }

            yield return new WaitForSeconds(blinkSpeed);

            currentTime += blinkSpeed * 2;
            yield return null;
        }

        blinkCompleted = true;
    }
}
