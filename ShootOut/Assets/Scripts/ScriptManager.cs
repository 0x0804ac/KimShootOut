using UnityEngine;

public class ScriptManager : MonoBehaviour
{
    [SerializeField] private GameScript gameScript;
    [SerializeField] private GoalScript goalScript;

    public GameScript Game => gameScript;
    public GoalScript Goal => goalScript;
}
