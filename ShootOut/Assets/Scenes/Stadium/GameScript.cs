using UnityEngine;

public class GameScript : MonoBehaviour
{
    [SerializeField] private ScriptManager manager;

    private bool isGoal, isSave;

    public bool IsGoal
    {
        get => isGoal;
        set
        {
            isGoal = value;
            if (value) isSave = false;
        }
    }
    public bool IsSave
    {
        get => isSave;
        set
        {
            if (value) isSave = !isGoal;
        }
    }

    public void ResetValues()
    {
        isGoal = false;
        isSave = false;
    }
}
