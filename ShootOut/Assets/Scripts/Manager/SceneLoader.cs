using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader: MonoBehaviour
{
    public string scenename;

    public string[] scenelist = new string[] {"Main", "Multi", "Campaign", "Achivement", "Collections", "Customize", "CampaignPlay", "MultiPlay"};

    public void Start()
    {

    }

    public void callScene(string name){
        scenename = name;
        SceneManager.LoadScene(scenename);
    }
    public void loadMainmenu()
    {
        SceneManager.LoadScene("Main");
    }
    public void loadMultigame()
    {
        SceneManager.LoadScene("Multi");
    }
    public void loadCampaign()
    {
        SceneManager.LoadScene("Campaign");
    }
    public void loadAchivement()
    {
        SceneManager.LoadScene("Achivement");
    }
    public void loadCollectin()
    {
        SceneManager.LoadScene("Collections");
    }
    public void loadCustomize()
    {
        SceneManager.LoadScene("Customize");
    } 
//    public string GetcurrentScene(){ return scenename }
}
