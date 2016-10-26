using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Dice : MonoBehaviour {

	public int DiceRoll()
	{
		int value;
		string path;

		value = (int) Random.Range(1, 5);
		path = "Images/Dices/Dice" + value.ToString();
		this.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
		return value;
	}
}
