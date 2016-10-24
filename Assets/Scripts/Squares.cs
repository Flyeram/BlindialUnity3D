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
		Debug.Log("WIP");
		Debug.Log(this.transform.position.x);
	}

	public void SetGameController(GameController controller)
	{
		gameController = controller;
	}
}
