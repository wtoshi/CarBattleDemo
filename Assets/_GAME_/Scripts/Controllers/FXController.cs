using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using ExtensionPlugins;

public class FXController : Singleton<FXController>
{

    public static GameObject CreateEffect(GameObject effectPF, Vector3 position, GameObject parent = null, float scale = 1, float timeToDestroy = 0)
    {
        GameObject effect = Master.Instance.AddChild(parent, effectPF);

        if (parent != null)
        {
            effect.transform.localPosition = position;
        }
        else
        {
            effect.transform.position = position;
        }
        if (timeToDestroy > 0)
        {
            Destroy(effect, timeToDestroy);
        }
        effect.transform.localScale = new Vector3(scale, scale, scale);
        return effect;
    }

   
}
