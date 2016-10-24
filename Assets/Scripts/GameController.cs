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
		SetSquareInteractable(true);
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
		diceValue = diceScript.DiceRoll();
		ChangeTurn();
	}

	public Player GetCurrentPlayer()
	{
		return playerList[turns];
	}

	void ChangeTurn()
	{
		if (turns < 3)
			turns++;
		else
			turns = 0;
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
