using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helpers : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private static readonly Dictionary<float, WaitForSeconds> WaitDictionary = new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
	{
        if (WaitDictionary.TryGetValue(time, out WaitForSeconds wait)) return wait;

        WaitDictionary[time] = new WaitForSeconds(time);
        return WaitDictionary[time];
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="min">inclusive</param>
	/// <param name="max">exclusive</param>
	/// <returns></returns>
	public static Vector2 RandomVector2(float min, float max)
	{
		return new Vector2(Random.Range(min, max), Random.Range(min, max));
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="vector"></param>
	/// <param name="min">inclusive</param>
	/// <param name="max">exclusive</param>
	/// <returns></returns>
	public static Vector2 RandomVector2(int min, int max)
	{
		return new Vector2(Random.Range(min, max), Random.Range(min, max));
	}
}
