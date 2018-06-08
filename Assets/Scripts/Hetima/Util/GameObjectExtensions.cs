using System.Linq;
using UnityEngine;

public static class GameObjectExtensions{
	/// <summary>
	/// 自分自身を含まないGetComponentInChildren
	/// </summary>
    public static T GetComponentInChildrenWithoutSelf<T>(this GameObject self) where T : Component{
        return self.GetComponentsInChildren<T>().Where(c => self != c.gameObject).FirstOrDefault();
    }

	/// <summary>
	/// 自分自身を含まないGetComponentsInChildren
	/// </summary>
	public static T[] GetComponentsInChildrenWithoutSelf<T>(this GameObject self) where T : Component{
        return self.GetComponentsInChildren<T>().Where(c => self != c.gameObject).ToArray();
    }
}