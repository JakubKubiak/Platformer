using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class LevelMenager : MonoBehaviour 
{
	public static LevelMenager Instance { set; get; }
	private int hitpoint = 3;
	private int score = 0;	

	public Transform spawnPosition;
	public Transform playerTransform;

	public Text scoreText;
	public Text hitpointText;



	private void Awake()// to samo co private void start() ale przed tamtym
	{
		Instance = this;
		scoreText.text = "Wynik : " + score.ToString();
		hitpointText.text = "Zycie : " + hitpoint.ToString ();
	}

	//every single frame
	private void Update()
	{
		if (playerTransform.position.y < -10) 
		{
			playerTransform.position = spawnPosition.position;
			hitpoint--;// hitpoint = hitpoint -1;
			hitpointText.text = "Zycie : " + hitpoint.ToString ();
			if (hitpoint <= 0) 
			{
				Debug.Log ("koniec gry");
			}
		}
	}
	public void Win()
	{
		Debug.Log ("Wygrana");
	}

	public void CollectCoin()
	{
		score++;
		scoreText.text = "Wynik : " + score.ToString();
	}

}
