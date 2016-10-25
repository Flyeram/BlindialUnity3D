using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dice : MonoBehaviour {

	public int DiceRoll()
	{
		int value;
		string path;

		value = (int) Random.Range(1, 5);
		path = "Assets/Resources/Images/Dices/Dice" + value.ToString() + ".png";
		this.GetComponent<Image>().sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
		return value;
	}
}
