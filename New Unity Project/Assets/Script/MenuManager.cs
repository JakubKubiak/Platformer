using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour 
{
	public void ToGame()
	{
		SceneManager.LoadScene("1");
	}

}
