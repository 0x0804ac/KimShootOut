using Unity.Entities;
using UnityEngine;



public class GameManager : Singleton<GameManager>
{   
    [SerializeField]
    private Stadium stadium;

    [SerializeField]
    private CanvasUI canvasUI;

    private void Awake()
    {
        Result.Check(Init());
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private GameManager() { }

    public override EResult Init()
    {
        if(stadium == null || canvasUI == null)
        {
            return EResult.Failed;
        }

        Result.Check(MakeInstance());
        Result.Check(stadium.Init());
        Result.Check(canvasUI.Init());

        return EResult.Success;
    }
}
