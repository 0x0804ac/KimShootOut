using UnityEngine;

public class PlayerRankManager : MonoBehaviour // for pvp rank management and pve campaign achivement managing
{
    public int currmode = 0; // -1 : pve, 0 : default, 1 : pvp

    public int playercurrmmr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //get player current MMR from database
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Getgamemode(int mode)    {    currmode = mode;    }
    public void updateMMR(int score)    {    playercurrmmr += score;    }
    public void savePlayerdata(){ } // save user data to database
}
