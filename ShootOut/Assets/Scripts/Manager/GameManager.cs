using UnityEngine;



public class GameManager : MonoBehaviour
{
    GameManager instance = null;

    private void Awake()
    {
        EResult result = MakeInstance();
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

    private EResult MakeInstance()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return EResult.Failed;
        }

        instance = this;

        return EResult.Success;
    }
}
