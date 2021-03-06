﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Player
{
	/// <summary>
	/// The Class which define a player in the game.
	/// Attributes :
	/// The image of the player
	/// The GameObject of the player
	/// The RectTransform of the player
	/// The name of the player
	/// The offset to display with
	/// The player Square position
	/// </summary>

	private Image playerImg;
	private GameObject playerGameObject;
	private RectTransform playerRectTransform;
	private string playerName;
	private Vector2 playerOffset;
	public Vector2 playerPosition;

	public Player(string Name, Vector2 offset)
	{
		playerName = Name;
		playerOffset = offset;
		playerPosition = new Vector2(4, 4);
	}

	//Instantiate a GameObject player with the right image and set his DisplayPosition to default
	public void PlayerSetImage(GameObject gameObject, string imagePath, GameObject gameboard)
	{
		string path = "Images/Players/" + imagePath;
		playerGameObject = gameObject;
		playerImg = playerGameObject.GetComponent<Image>();
		playerRectTransform = playerGameObject.GetComponent<RectTransform>();
		playerGameObject.transform.SetParent(gameboard.transform);
		playerImg.sprite = Resources.Load<Sprite>(path);
		playerRectTransform.offsetMax = new Vector2(0, 0);
		playerRectTransform.offsetMin = new Vector2(0, 0);
		PlayerSetDisplayPosition(0.44999f, 0.44999f);
	}

	//Display the player's image at the position given (with the player's offset)
	public void PlayerSetDisplayPosition(float x, float y)
	{
		float offx = x + playerOffset.x;
		float offy = y + playerOffset.y;
		playerRectTransform.anchorMin = new Vector2(offx, offy);
		playerRectTransform.anchorMax = new Vector2(offx + 0.0333f, offy + 0.0333f);
	}


}
