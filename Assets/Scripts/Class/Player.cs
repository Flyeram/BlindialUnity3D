using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player
{
	/// <summary>
	/// The Class which define a player in the game.
	/// Attributes :
	/// The image of the player
	/// The GameObject of the player
	/// The name of the player
	/// </summary>

	private Image playerImg;
	private GameObject playerGameObject;
	private RectTransform playerRectTransform;
	private string playerName;

	public Player(string Name)
	{
		playerName = Name;
	}

	//Create a game object with an image, canva renderer and Rect Transform.
	public void PlayerSetImage(GameObject gameObject, string imagePath, GameObject Canva)
	{
		string path = "Assets/Resources/Images/Players/" + imagePath + ".png";
		playerGameObject = gameObject;
		playerImg = playerGameObject.GetComponent<Image>();
		playerRectTransform = playerGameObject.GetComponent<RectTransform>();
		playerGameObject.transform.SetParent(Canva.transform);
		playerImg.sprite = (Sprite) UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(Sprite));
		playerRectTransform.offsetMax = new Vector2(0, 0);
		playerRectTransform.offsetMin = new Vector2(0, 0);
		PlayerSetPosition(0.5f, 0.5f);
	}

	public void PlayerSetPosition(float x, float y)
	{
		playerRectTransform.anchorMin = new Vector2(x, y);
		playerRectTransform.anchorMax = new Vector2(x + 0.0171f, y + 0.0303f);
	}
}
