using UnityEngine;

static public class UnityEngineExtensions
{
	/// <summary>
	/// Returns the component of Type type. If one doesn't already exist on the GameObject it will be added.
	/// </summary>
	/// <typeparamname=T>The type of Component to return.</typeparam>
	/// <paramname=gameObject>The GameObject this Component is attached to.</param>
	/// <returns>Component</returns>
	static public T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
	{
		return gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
	}
}