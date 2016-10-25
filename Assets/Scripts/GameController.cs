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

		//SetSquareInteractable(false);
		diceValue = diceScript.DiceRoll();
		//CheckSquarePossible(diceValue);
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

	//void CheckSquarePossible(int diceValue)
	//{
	//	Vector2 position = GetCurrentPlayer().playerPosition;

	//	//We check from x = -diceValue to x = diceValue
	//	for (int i = -diceValue; i < (diceValue + 1); i++)
	//	{
	//		//We browse all the squares to see if one has a matching position
	//		for (int j = 0; j < buttonList.Length; j++)
	//		{
	//			Vector2 squarePosition = buttonList[j].GetComponent<Squares>().GetSquarePosition();
	//			/* this offset is here to check squares that are not on the same y axis
	//			 * Exemple : if the player position is (4, 4) and the dice rolls 3
	//			 * x = -3, so offset = 0 (3 - abs(-3)), so we check is a square is at [1, 4] ((4 - 3, 4 - offset) == (4 - 3, 4 + offset))
	//			 * x = -2, so offset = 1 (3 - abs(-2)), so we check is a square is either at [2, 3] (4 - 2, 4 - offset) or [2, 5] (4 - 2, 4 + offset)
	//			 */
	//			int yOffset = diceValue - Mathf.Abs(i);
	//			if (squarePosition == new Vector2(position.x + i, position.y - yOffset) || squarePosition == new Vector2(position.x + i, position.y + yOffset))
	//			{
	//				//if the dice value equal 4 we have to remove the squares that can be accessed by using non existing squares
	//				if (diceValue == 4)
	//				{
	//					// If the square is on the same x axis
	//					if (i == -4 || i == 4)
	//					{
	//						// if there is a square on the right AND the left of the player the square is validated, otherwise it's a cheat square
	//						if (IsASquareAtPos(new Vector2(position.x - 1, position.y)) && IsASquareAtPos(new Vector2(position.x + 1, position.y)))
	//							buttonList[j].GetComponent<Button>().interactable = true;
	//					}
	//					// If the square is on the same y axis
	//					else if (i == 0)
	//					{
	//						// if there is a square on the top AND the bottom of the player the square is validated, otherwise it's a cheat square
	//						if (IsASquareAtPos(new Vector2(position.x, position.y - 1)) && IsASquareAtPos(new Vector2(position.x, position.y + 1)))
	//							buttonList[j].GetComponent<Button>().interactable = true;
	//					}
	//					else
	//						buttonList[j].GetComponent<Button>().interactable = true;
	//				}
	//				else
	//					buttonList[j].GetComponent<Button>().interactable = true;
	//			}
	//		}
	//	}
	//}

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
