using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//게임 시스템의 전반을 관리 게임 시작과 하위 시스템 접속을 담당
public class GameManager : MonoBehaviour
{
	public GameObject Scenemanager;
	public Button startButton;
	public void Start()
	{
		Button stbtn = startButton.GetComponent<Button>();
	}
	public void StartButton()
	{
		SceneManager.LoadScene("MainMenu");
	}
	public void StartPlayButton()
	{
		SceneManager.LoadScene("");
	}
	public void Update()
	{

	}
}
