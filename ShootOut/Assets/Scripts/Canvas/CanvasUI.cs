using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : Singleton<CanvasUI>
{
    [SerializeField] 
    Image mouseCursor;

    [SerializeField]
    Image chargeGuage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override EResult Init()
    {
        if (mouseCursor == null || chargeGuage == null)
        {
            return EResult.Failed;
        }

        return EResult.Success;
    }

    public Image GetMouseCursor()
    {
        return mouseCursor;
    }

    public Image GetChargeGuage()
    {
        return chargeGuage;
    }
}
