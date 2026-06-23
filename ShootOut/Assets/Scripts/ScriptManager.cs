using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    public GameScript gameScript;
    public ButtonScript buttonScript;
    public GoalScript goalScript;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public GameScript GetGameScript() { return gameScript; }
    public ButtonScript GetButtonScript() { return buttonScript; }
    public GoalScript GetGoalScript() { return goalScript; }
}
