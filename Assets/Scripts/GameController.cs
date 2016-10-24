using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {
	[SerializeField]
	private GameObject[] buttonList;
	[SerializeField]
	private GameObject canvaRef;
	[SerializeField]
	private GameObject playerGameObject;

	private Player PlayerList;
	
	void Awake()
	{
		SetControllerOnButtons();
		PlayerList = new Player("Flyeram");
	}

	void Start()
	{
		SetPlayer();
	}

	void SetPlayer()
	{
		GameObject tmp = (GameObject) Instantiate(playerGameObject, new Vector3(0, 0, 0), Quaternion.identity);
		tmp.name = "Player1";
		PlayerList.PlayerSetImage(tmp, "Player1", canvaRef);
	}

	void SetControllerOnButtons()
	{
		for (int i = 0; i < buttonList.Length; i++)
		{
			buttonList[i].GetComponent<Squares>().SetGameController(this);
		}
	}


}
