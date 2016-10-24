using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Squares : MonoBehaviour {

	private GameController gameController;

	public void ClickOnSquare(int type)
	{
		DisplayPlayer();
	}

	void DisplayPlayer()
	{
		float x = this.GetComponent<RectTransform>().anchorMin.x;
		float y = this.GetComponent<RectTransform>().anchorMin.y;
		gameController.GetCurrentPlayer().PlayerSetPosition(x, y);
		gameController.ChangeTurn();
	}

	public void SetGameController(GameController controller)
	{
		gameController = controller;
	}
}
