using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ArenaController : MonoBehaviour
{
    List<ArenaCircle> allCircles = new List<ArenaCircle>();

    bool hasDestroyableCircle = true;
    Coroutine destroyingCO;

    public static bool circleDestroying = false;

    private void OnEnable()
    {
        GameEvents.LevelStarted += OnLevelStarted;
        GameEvents.LevelFinished += OnLevelFinished;
    }

    private void OnDisable()
    {
        GameEvents.LevelStarted -= OnLevelStarted;
        GameEvents.LevelFinished -= OnLevelFinished;
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);

            var arenaCircle = child.GetComponent<ArenaCircle>();

            if (arenaCircle == null)
                continue;

            allCircles.Add(arenaCircle);
        }

        allCircles.OrderByDescending(x => x.transform.GetSiblingIndex());
        allCircles[0].canBeDestroyed = false;
    }

    public void DestroyCircles(float duration = 3f)
    {
        destroyingCO = StartCoroutine(doDestroyCircles(duration));
    }

    IEnumerator doDestroyCircles(float duration = 3f)
    {
        var destroyableCircle = allCircles.FindLast(x => x.canBeDestroyed == true);

        while (true)
        {
            if (destroyableCircle == null)
            {
                hasDestroyableCircle = false;
                yield break;
            }

            UIManager.Get<InGameUI>().InitDestroyingSlider(GameManager.Instance.GameSettings.destroyGap);

            yield return new WaitForSeconds(GameManager.Instance.GameSettings.destroyGap);

            Debug.Log("Destroying: "+ destroyableCircle.name);

            destroyableCircle.DestroyCircle(duration);

            destroyableCircle = allCircles.FindLast(x => x.canBeDestroyed == true);

            yield return null;
        }
    }


    void OnLevelStarted()
    {
        DestroyCircles(5f);
    }

    void OnLevelFinished(bool successed)
    {
        StopAllCoroutines();
    }
}
