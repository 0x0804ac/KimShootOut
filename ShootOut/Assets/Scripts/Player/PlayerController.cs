using System;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    UserInputs userInputs;
    UserInputs.PlayerActions playerActions;

    [SerializeReference]
    ITypeController typeController;

    [SerializeField]
    MouseController mouseController = new MouseController();

    [SerializeField] float speed = 5f;

    [SerializeField]
    MeshRenderer meshRenderer;
    
    [SerializeField]
    Material[] materials;

    int index = 0;

    public void Init(UserInputs userInputs)
    {
        this.userInputs = new UserInputs();
    }

    private void Awake()
    {
        userInputs = new UserInputs();
        Assert.IsTrue(mouseController != null);
        playerActions = userInputs.Player;
        typeController = new AttackerController();
        // typeController = new DefenderController();
    }

    private void OnEnable()
    {
        userInputs.Player.Enable();
    }

    private void OnDestroy()
    {
        userInputs?.Dispose();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MovementUpdate();
        TypeControllerUpdate();
        TeamUpdate();
        MouseControllerUpdate();
    }

    ERESULT MovementUpdate()
    {
        Vector2 moveInput = playerActions.Move.ReadValue<Vector2>();

        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y) * Time.deltaTime * speed;

        transform.position += moveDirection;

        return ERESULT.SUCCESS;
    }

    ERESULT MouseControllerUpdate()
    {
        if (typeController.IsAimComfirmed())
        {
            mouseController.Update();
        }

        return ERESULT.SUCCESS;
    }

    ERESULT TeamUpdate()
    {
        if (userInputs.Player.V.WasPressedThisFrame())
        {
            index = (index + 1) % materials.Length;
            meshRenderer.sharedMaterial = materials[index];
        }

        return ERESULT.SUCCESS;
    }

    ERESULT TypeControllerUpdate()
    {
        typeController.Update(this, playerActions);

        return ERESULT.SUCCESS;
    }
}
