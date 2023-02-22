using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

public class Master : Singleton<Master>
{
    public GameObject AddChild(GameObject parent, GameObject child, float _scale = -1, Transform _position = null)
    {
        if (child == null)
        {
            Debug.LogError("Parent or Gameobject is NULL!");

            return null;
        }

        GameObject ch = Instantiate(child, parent == null ? null : parent.transform);

        if (ch == null)
            return null;

        if (_scale != -1) ch.transform.localScale = new Vector3(_scale, _scale, _scale);

        return ch;
    }

    public GameObject GetChildByName(GameObject parent, string childName, bool alsoInactiveGameObject = false)
    {

        if (parent != null)
        {
            foreach (Transform item in parent.GetComponentsInChildren<Transform>(alsoInactiveGameObject))
            {
                string childNameItem = item.gameObject.name;
                if (childNameItem == childName)
                {
                    return item.gameObject;
                }
            }
        }
        else
        {

            //  return gameobject.find(childname);
            GameObject[] allObjects = FindObjectsOfType<GameObject>(true);
            foreach (GameObject go in allObjects)
            {
                if (!go.scene.IsValid()) continue;

                if (!alsoInactiveGameObject)
                {
                    if (!go.activeInHierarchy)
                        continue;
                }
                if (go.name == childName)
                {
                    return go;
                }
            }
        }
        return null;
    }

    public void ClearAllChilds(GameObject parent)
    {
        if (parent != null)
        {
            int childCount = parent.transform.childCount;

            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    GameObject sil = parent.transform.GetChild(i).gameObject;
                    Destroy(sil);
                }
            }
            else Debug.Log("Child yok!");
        }
        else
        {
            Debug.Log("Parent is null !");
        }
    }

    public void WaitAndDo(float time, Action onComplete, MonoBehaviour monoBehaviour = null, bool ignoreTimeScale = false)
    {
        StartCoroutine(WaitForRealSeconds(time, onComplete));
    }

    public IEnumerator WaitForRealSeconds(float time, Action onComplete)
    {
        yield return new WaitForSeconds(time);

        if (onComplete != null)
        {
            onComplete();
        }
    }

    public static void LookAt2D(Transform body, Transform target)
    {
        Vector3 dir = target.position - body.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        body.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public static System.Object CloneObject(System.Object other)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            System.Runtime.Serialization.Formatters.Binary.BinaryFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            formatter.Serialize(ms, other);
            ms.Position = 0;
            return formatter.Deserialize(ms);
        }
    }

    public T GetResourcesByName<T>(string _prefabName) where T : UnityEngine.Object
    {
        var pf = Resources.Load<T>(_prefabName);

        return pf;
    }

    public bool CheckChance(float _rate)
    {
        var success = Random.Range(0f, 100f);
        if (!(success <= _rate))
        {
            return false;
        }

        return true;
    }


    #region SERIALIZATION
    public static T DeserializeObject<T>(string value)
    {
        return JsonUtility.FromJson<T>(value);
        //return JsonConvert.DeserializeObject<T>(value);
    }
    public static string SerializeObject(object value)
    {
        return JsonUtility.ToJson(value);
        // JsonConvert.DeserializeObject<GameEntries.GameInfo>(json);
    }
    #endregion

    #region IO
    public static void SaveToDisk(string s, string path)
    {
        StreamWriter sw = new StreamWriter(path);
        sw.Write(s);
        sw.Close();
    }

    public static string LoadFromDisk(string path)
    {
        StreamReader sr = new StreamReader(path);
        string fileString = sr.ReadToEnd();
        sr.Close();
        return fileString;
    }
    #endregion
}
