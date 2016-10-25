using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	[SerializeField]
	private GameObject[] buttonList;
	[SerializeField]
	private GameObject canvaRef;
	[SerializeField]
	private GameObject playerGameObject;
	[SerializeField]
	private Dice diceScript;

	private Player[] playerList;
	private int turns;

	/*
	 * Functions to initialize the game
	 */
	 	
	void Awake()
	{
		SetControllerOnButtons();
	}

	void Start()
	{
		playerList = new Player[4];
		CreatePlayerList();
		SetPlayer();
		PlayTurn();
	}

	void SetControllerOnButtons()
	{
		for (int i = 0; i < buttonList.Length; i++)
		{
			buttonList[i].GetComponent<Squares>().SetGameController(this);
		}
	}

	/*
	 * Function to manage the flow of the game
	 */
	
	public void PlayTurn()
	{
		int diceValue;

		SetSquareInteractable(false);
		diceValue = diceScript.DiceRoll();
		CheckSquarePossible(diceValue);
	}

	public Player GetCurrentPlayer()
	{
		return playerList[turns];
	}

	public void ChangeTurn()
	{
		if (turns < 3)
			turns++;
		else
			turns = 0;
	}

	void CheckSquarePossible(int diceValue)
	{
		Vector2 position = GetCurrentPlayer().playerPosition;

		for (int i = -diceValue; i < (diceValue + 1); i++)
		{
			for (int j = 0; j < buttonList.Length; j++)
			{
				Vector2 squarePosition = buttonList[j].GetComponent<Squares>().GetSquarePosition();
				int xOffset = diceValue - Mathf.Abs(i);
				if (squarePosition == new Vector2(position.x - xOffset, position.y + i) || squarePosition == new Vector2(position.x + xOffset, position.y + i))
				{
					buttonList[j].GetComponent<Button>().interactable = true;
				}
			}
		}
	}

	/*
	 * Tools function
	 */

	void SetSquareInteractable(bool toggle)
	{
		for (int i = 0; i < buttonList.Length; i++)
		{
			buttonList[i].GetComponent<Button>().interactable = toggle;
		}
	}

	/*
	 * Function to create players and initialise them
	 */

	void CreatePlayerList()
	{
		Vector2[] offsets = new Vector2[4] {new Vector2(0.005f, 0.01f), new Vector2(0.03f, 0.01f), new Vector2(0.005f, 0.055f), new Vector2(0.03f, 0.055f)};

		for (int i = 0; i < 4; i++)
		{
			playerList[i] = new Player("Player" + i.ToString(), offsets[i]);
		}
	}

	void SetPlayer()
	{
		for (int i = 0; i < 4; i++)
		{
			GameObject tmp = (GameObject)Instantiate(playerGameObject, new Vector3(0, 0, 0), Quaternion.identity);
			tmp.name = "Player" + i.ToString();
			playerList[i].PlayerSetImage(tmp, tmp.name, canvaRef);
		}
	}

}
