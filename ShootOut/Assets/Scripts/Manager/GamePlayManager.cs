using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public int currmode = 0; // -1 : pve, 0 : default, 1 : pvp

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void GetGamemode(int mode)
    {
        currmode = mode;
        Debug.Log("PVE mode had been selected");
    }

    public int pve_setfirstATK() // 1 is player in pve 2 is CPU in pve
    {
        int result = Random.Range(1,2);
        return result;
    }

    public void ChangeAtkDef()// change ATK and DEF 
    {

    }

    public void GetAbility(){ } // get random ability
}
