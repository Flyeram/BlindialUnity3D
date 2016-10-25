using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {
	[SerializeField]
	private GameObject gameboard;
	[SerializeField]
	private GameObject playerGameObject;
	[SerializeField]
	private Dice diceScript;
	[SerializeField]
	private GameObject squareGameObject;

	private GameObject[][] buttonList;
	private Player[] playerList;
	private int turns;

	/*
	 * Functions to initialize the game
	 */
	 	
	void Awake()
	{
		CreateSquares();
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
		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
				buttonList[i][j].GetComponent<Squares>().SetGameController(this);
		}
	}

	int[] CreateArraySquareType()
	{
		/* Legend
		 * 1 = green
		 * 2 = red
		 * 3 = yellow
		 * 4 = orange
		 * 5 = replay
		 * 1* = GoToPlectrum (* for the color)
		 * 10* = Plectrum (*for the color)
		 */
		int[] array = new int[81] {101, 3, 14, 2, 5, 1, 3, 4, 102,
									2, 0, 0, 0, 4, 0, 0, 0, 1,
									4, 0, 0, 0, 3, 0, 0, 0, 13,
									3, 0, 0, 0, 1, 0, 0, 0, 4,
									5, 2, 4, 3, 5, 2, 1, 3, 5,
									1, 0, 0, 0, 4, 0, 0, 0, 2,
									12, 0, 0, 0, 2, 0, 0, 0, 1,
									4, 0, 0, 0, 1, 0, 0, 0, 3,
									103, 1, 2, 4, 5, 3, 11, 2, 104};
		return array;
	}

	void CreateSquares()
	{
		//Array which define the type of each square
		int[] squareType = CreateArraySquareType();
		//Percentage of the gameboard size 
		float spaceSize = 0.00833f;
		float squareSize = 0.10f;
		float startOffset = 0.01667f;
		buttonList = new GameObject[9][];

		for (int i = 0; i < 9; i++)
		{
			buttonList[i] = new GameObject[9];
			for (int j = 0; j < 9; j++)
			{
				buttonList[i][j] = (GameObject)Instantiate(squareGameObject, new Vector3(0, 0, 0), Quaternion.identity);
				RectTransform transform = buttonList[i][j].GetComponent<RectTransform>();
				transform.SetParent(gameboard.transform);
				transform.offsetMax = new Vector2(0, 0);
				transform.offsetMin = new Vector2(0, 0);
				/*
				 * the min anchor for each square is
				 * X = StartOffset + (Square Size + Space Size) * each square from the beginning
				 * Y = StartOffset + (Square Size + Space Size) * (8 - each square from the bottom) (because it's anchored relatively to the min anchor of the parent or the list is from the top)
				 * The max anchor is the same plus 1 square size (logical)
				 */ 
				transform.anchorMin = new Vector2(startOffset + (squareSize + spaceSize) * j, startOffset + (squareSize + spaceSize) * (8 - i));
				transform.anchorMax = new Vector2(startOffset + squareSize + (squareSize + spaceSize) * j, startOffset + squareSize + (squareSize + spaceSize) * (8 - i));
				buttonList[i][j].GetComponent<Squares>().SetSquarePosition(new Vector2(j, i));
				buttonList[i][j].GetComponent<Squares>().SetSquareType(squareType[i * 9 + j]);
				buttonList[i][j].GetComponent<Image>().sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Resources/Images/Squares/Square" + squareType[i * 9 + j].ToString() + ".png", typeof(Sprite));
			}
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
		EnableSquarePossible(diceValue);
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

	void EnableSquarePossible(int diceValue)
	{
		Vector2 position = GetCurrentPlayer().playerPosition;
		int yOffset;
		int ypos;
		int yneg;
		int x;

		//We check from x = -diceValue to x = diceValue
		for (int i = -diceValue; i < (diceValue + 1); i++)
		{
			/* this offset is here to check squares that are not on the same y axis
			 * Exemple : if the player position is (4, 4) and the dice rolls 3
			 * x = -3, so offset = 0 (3 - abs(-3)), so we check is a square is at [1, 4] ((4 - 3, 4 - offset) == (4 - 3, 4 + offset))
			 * x = -2, so offset = 1 (3 - abs(-2)), so we check is a square is either at [2, 3] (4 - 2, 4 - offset) or [2, 5] (4 - 2, 4 + offset)
			 */
			yOffset = diceValue - Mathf.Abs(i);
			ypos = (int)(position.y + yOffset);
			yneg = (int)(position.y - yOffset);
			x = (int)(position.x + i);
			// We check is a square is not reachable by using invisible Square. 
			// We check if there is a Square on the left of the player to decide if he can go 4 on the left (Same for other directions)
			if (diceValue == 4)
			{
				//Left
				if (i == -4)
				{
					if (IIAR((int)position.x - 1) && IIAR(ypos) && IIAR(x) && buttonList[(int)position.y][(int)position.x - 1].GetComponent<Squares>().IsNotEmpty())
						buttonList[ypos][x].GetComponent<Button>().interactable = true;
				}
				//Right
				else if (i == 4)
				{
					if (IIAR((int)position.x + 1) && IIAR(ypos) && IIAR(x) && buttonList[(int)position.y][(int)position.x + 1].GetComponent<Squares>().IsNotEmpty())
						buttonList[ypos][x].GetComponent<Button>().interactable = true;
				}
				else if (i == 0)
				{
					//Down
					if (IIAR((int)position.y + 1) && IIAR(ypos) && IIAR(x) && buttonList[(int)position.y + 1][(int)position.x].GetComponent<Squares>().IsNotEmpty())
						buttonList[ypos][x].GetComponent<Button>().interactable = true;
					//Up
					if (IIAR((int)position.y - 1) && IIAR(yneg) && IIAR(x) && buttonList[(int)position.y - 1][(int)position.x].GetComponent<Squares>().IsNotEmpty())
						buttonList[yneg][x].GetComponent<Button>().interactable = true;
				}
				else
					CheckSquares(ypos, yneg, x);
			}
			else
			{
				CheckSquares(ypos, yneg, x);
			}
		}
	}
	/*
	 * Tools function
	 */

	void SetSquareInteractable(bool toggle)
	{
		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
				buttonList[i][j].GetComponent<Button>().interactable = toggle;
		}
	}

	bool IIAR(int value)
	{
		//Is In Array Range
		if (value >= 0 && value <= 8)
			return true;
		return false;
	}

	void CheckSquares(int ypos, int yneg, int x)
	{
		if (IIAR(x))
		{
			if (IIAR(ypos) && buttonList[ypos][x].GetComponent<Squares>().IsNotEmpty())
				buttonList[ypos][x].GetComponent<Button>().interactable = true;
			if (IIAR(yneg) && buttonList[yneg][x].GetComponent<Squares>().IsNotEmpty())
				buttonList[yneg][x].GetComponent<Button>().interactable = true;
		}			
	}


	/*
	 * Function to create players and initialise them
	 */

	void CreatePlayerList()
	{
		Vector2[] offsets = new Vector2[4] {new Vector2(0.00833f, 0.00833f), new Vector2(0.00833f, 0.0583f), new Vector2( 0.0583f, 0.00833f), new Vector2( 0.0583f,  0.0583f)};

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
			playerList[i].PlayerSetImage(tmp, tmp.name, gameboard);
		}
	}

}
