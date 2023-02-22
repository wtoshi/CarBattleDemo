using Sirenix.OdinInspector;

public abstract class DataControler<T> : Singleton<T> where T : DataControler<T>
{
	public abstract void LoadData(System.Action onComplete = null, System.Action onFail = null);

	protected abstract void SaveData();

	[Button(ButtonSizes.Medium)]
	protected abstract void ClearData();

}