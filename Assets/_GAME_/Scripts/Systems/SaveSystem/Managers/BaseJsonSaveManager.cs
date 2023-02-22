//using System.IO;
//using Sirenix.OdinInspector;
//using UnityEngine;

//public abstract class BaseJsonSaveManager<T> where T : class, new()
//{

//	protected abstract DataUtils<T> Serializer { get; }

//	protected DataUtils<T> _serializer;

//	public virtual void Init(string dataName)
//	{
//		string path = Application.persistentDataPath + Path.DirectorySeparatorChar + dataName;
//		_serializer = new JsonSerializer<T>(path);
//	}

//	public virtual void SaveData()
//	{
//		Serializer.SaveData();
//	}

//	[Button, PropertyOrder(100)]
//	public virtual void ClearData()
//	{
//		Serializer.ClearData();
//	}

//}