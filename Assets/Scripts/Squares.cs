using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Squares : MonoBehaviour {

	private GameController gameController;
	[SerializeField]
	private Vector2 squarePosition;

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

	public Vector2 GetSquarePosition()
	{
		return squarePosition;
	}
}
