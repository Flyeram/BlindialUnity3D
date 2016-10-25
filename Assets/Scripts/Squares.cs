using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Squares : MonoBehaviour {

	private GameController gameController;
	private Vector2 squarePosition;
	private int type;

	public void ClickOnSquare(int type)
	{
		DisplayPlayer();
		gameController.GetCurrentPlayer().playerPosition = squarePosition;
		gameController.ChangeTurn();
		gameController.PlayTurn();
	}

	void DisplayPlayer()
	{
		float x = this.GetComponent<RectTransform>().anchorMin.x;
		float y = this.GetComponent<RectTransform>().anchorMin.y;
		gameController.GetCurrentPlayer().PlayerSetDisplayPosition(x, y);
	}

	public void SetGameController(GameController controller)
	{
		gameController = controller;
	}

	public void SetSquarePosition(Vector2 pos)
	{
		squarePosition = pos;
	}

	public Vector2 GetSquarePosition()
	{
		return squarePosition;
	}

	public void SetSquareType(int number)
	{
		type = number;
	}

	public int GetSquareType()
	{
		return type;
	}

	public bool IsNotEmpty()
	{
		if (type == 0)
			return false;
		return true;
	}
}
