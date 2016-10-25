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

		//We check from x = -diceValue to x = diceValue
		for (int i = -diceValue; i < (diceValue + 1); i++)
		{
			//We browse all the squares to see if one has a matching position
			for (int j = 0; j < buttonList.Length; j++)
			{
				Vector2 squarePosition = buttonList[j].GetComponent<Squares>().GetSquarePosition();
				/* this offset is here to check squares that are not on the same y axis
				 * Exemple : if the player position is (4, 4) and the dice rolls 3
				 * x = -3, so offset = 0 (3 - abs(-3)), so we check is a square is at [1, 4] ((4 - 3, 4 - offset) == (4 - 3, 4 + offset))
				 * x = -2, so offset = 1 (3 - abs(-2)), so we check is a square is either at [2, 3] (4 - 2, 4 - offset) or [2, 5] (4 - 2, 4 + offset)
				 */
				int yOffset = diceValue - Mathf.Abs(i);
				if (squarePosition == new Vector2(position.x + i, position.y - yOffset) || squarePosition == new Vector2(position.x + i, position.y + yOffset))
				{
					//if the dice value equal 4 we have to remove the squares that can be accessed by using non existing squares
					if (diceValue == 4)
					{
						// If the square is on the same x axis
						if (i == -4 || i == 4)
						{
							// if there is a square on the right AND the left of the player the square is validated, otherwise it's a cheat square
							if (IsASquareAtPos(new Vector2(position.x - 1, position.y)) && IsASquareAtPos(new Vector2(position.x + 1, position.y)))
								buttonList[j].GetComponent<Button>().interactable = true;
						}
						// If the square is on the same y axis
						else if (i == 0)
						{
							// if there is a square on the top AND the bottom of the player the square is validated, otherwise it's a cheat square
							if (IsASquareAtPos(new Vector2(position.x, position.y - 1)) && IsASquareAtPos(new Vector2(position.x, position.y + 1)))
								buttonList[j].GetComponent<Button>().interactable = true;
						}
						else
							buttonList[j].GetComponent<Button>().interactable = true;
					}
					else
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

	bool IsASquareAtPos(Vector2 pos)
	{
		for (int i = 0; i < buttonList.Length; i++)
		{
			if (buttonList[i].GetComponent<Squares>().GetSquarePosition() == pos)
			{
				return true;
			}
		}
		return false;
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
