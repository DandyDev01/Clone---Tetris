using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{

	public static bool Approx(this Vector3 me, Vector3 other, float range = 0.01f)
	{
		if (Mathf.Abs(me.x - other.x) <= range && 
			Mathf.Abs(me.y - other.y) <= range &&
			Mathf.Abs(me.z - other.z) <= range)
			return true;

		return false;
	}

	public static Vector3Int ToVec3Int(this Vector3 me)
	{
		return new Vector3Int((int)me.x, (int)me.y, (int)me.z);
	}

	/// <summary>
	/// Shuffles an array in place.
	/// </summary>
	/// <typeparam name="T">The array element type.</typeparam>
	/// <param name="list">The array to shuffle.</param>
	public static void Shuffle<T>(this T[] list)
	{
		var count = list.Length;
		for (int i1 = 0; i1 < count; i1++)
		{
			var i2 = UnityEngine.Random.Range(0, count);
			var element = list[i1];
			list[i1] = list[i2];
			list[i2] = element;
		}
	}

	/// <summary>
	/// Shuffles a list in place.
	/// </summary>
	/// <typeparam name="T">The list element type.</typeparam>
	/// <param name="list">The list to shuffle.</param>
	public static void Shuffle<T>(this List<T> list)
	{
		var count = list.Count;
		for (int i1 = 0; i1 < count; i1++)
		{
			var i2 = UnityEngine.Random.Range(0, count);
			var element = list[i1];
			list[i1] = list[i2];
			list[i2] = element;
		}
	}

	/// <summary>
	/// Returns a random element from the array.
	/// </summary>
	/// <typeparam name="T">The array element type.</typeparam>
	/// <param name="array">The array to return an element from.</param>
	/// <returns>A random element from the array.</returns>
	public static T RandomElement<T>(this T[] array)
	{
		var index = UnityEngine.Random.Range(0, array.Length);
		return array[index];
	}

	/// <summary>
	/// Returns a random element from the list.
	/// </summary>
	/// <typeparam name="T">The list element type.</typeparam>
	/// <param name="list">The list to return an element from.</param>
	/// <returns>A random element from the list.</returns>
	public static T RandomElement<T>(this List<T> list)
	{
		var index = UnityEngine.Random.Range(0, list.Count);
		return list[index];
	}

	/// <summary>
	/// Get a range of elements from a list
	/// </summary>
	/// <typeparam name="T">the list element type</typeparam>
	/// <param name="list">the list to return elements from</param>
	/// <param name="startIndex">index to start getting elements from</param>
	/// <param name="count">the number of elements to get</param>
	/// <returns>a list of within the specified range</returns>
	public static List<T> GetRange<T>(this List<T> list, int startIndex, int count)
	{
		List<T> results = new();

		for (int i = startIndex; i < count; i++)
		{
			results.Add(list[i]);
		}

		return results;
	}

	/// <summary>
	/// Gets component from the first gameobject with a specified component
	/// </summary>
	/// <typeparam name="T">component to get from game object</typeparam>
	/// <param name="list">list of game objects to search</param>
	/// <returns>specified component</returns>
	public static T GetGameObjectWithComponent<T>(this IEnumerable<GameObject> list) where T : MonoBehaviour
	{
		return list.Where(x => x.GetComponent<T>() != null).FirstOrDefault().GetComponent<T>();
	}

	public static Transform[] GetChildren(this Transform me)
	{
		var children = new Transform[me.childCount];

		for (int i = 0; i < children.Length; i++)
		{
			children[i] = me.GetChild(i);
		}

		return children;
	}

	public static GameObject[] GetChildren(this GameObject me)
	{
		var children = new GameObject[me.transform.childCount];

		for (int i = 0; i < children.Length; i++)
		{
			children[i] = me.transform.GetChild(i).gameObject;
		}

		return children;
	}

	/// <summary>
	/// checks that list contains element that matches predicate
	/// </summary>
	/// <typeparam name="T">the list element type</typeparam>
	/// <param name="list">list to check</param>
	/// <param name="predicate">predecate to check</param>
	/// <returns>wheather the predicate is true or not</returns>
	public static bool Contains<T>(this IEnumerable<T> list, System.Func<T, bool> predicate)
	{
		return list.Where(predicate).Any();
	}

	public static Transform GetChildWhere(this Transform me, System.Func<Transform, bool> x)
	{
		List<Transform> list = new List<Transform>();

		for (int i = 0; i < me.childCount; i++)
		{
			list.Add(me.GetChild(i));
		}

		var options = list.Where(x);

		if (options.Any())
			return options.First();


		return null;
	}

	public static Transform[] GetChildrenWhere(this Transform me, System.Func<Transform, bool> x)
	{
		List<Transform> list = new List<Transform>();

		for (int i = 0; i < me.childCount; i++)
		{
			list.Add(me.GetChild(i));
		}

		var options = list.Where(x);


		return options.ToArray();
	}
}
