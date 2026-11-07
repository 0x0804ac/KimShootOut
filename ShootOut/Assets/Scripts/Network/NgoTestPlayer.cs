using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;



public class NgoTestPlayer : NetworkBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    private Vector2 localInput;
    private Vector2 serverInput;

    private readonly NetworkVariable<int> score = new NetworkVariable<int>(0);

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            int column = (int)(OwnerClientId % 5);
            int row = (int)(OwnerClientId / 5);

            transform.position = new Vector3(
                column * 2f - 4f,
                1f,
                row * 2f);
        }

        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material.color = IsOwner ? Color.green : Color.red;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
     
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsSpawned || !IsOwner)
        {
            return;
        }

        Keyboard keyboard = Keyboard.current;

        if (keyboard != null)
        {
            Debug.Log("True");
            return;
        }

        Vector2 input = Vector2.zero;

        if (keyboard.wKey.isPressed)
        {
            input.y += 1;
        }
        if (keyboard.sKey.isPressed)
        {
            input.y -= 1;
        }
        if (keyboard.aKey.isPressed)
        {
            input.x -= 1;
        }
        if (keyboard.dKey.isPressed)
        {
            input.x += 1;
        }


        input = input.normalized;

        if (input != localInput)
        {
            localInput = input;
            SubmitInputRpc(input);
        }

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            AddScoreRpc();
        }
    }

    private void FixedUpdate()
    {
        if (!IsSpawned || !IsOwner)
        {
            return;
        }
        
        Vector3 direction = new Vector3(serverInput.x, 0f, serverInput.y);

        transform.position += direction * moveSpeed * Time.fixedDeltaTime;
    }

    [Rpc(SendTo.Server)]
    private void SubmitInputRpc(Vector2 input)
    {
        serverInput = input;
    }

    [Rpc(SendTo.Server)]
    private void AddScoreRpc()
    {
        score.Value++;
    }

    private void OnGUI()
    {
        if (!IsSpawned)
        {
            return;
        }


        Camera camera = Camera.main;

        if (camera == null)
        {
            return;
        }

        Vector3 screen = camera.WorldToScreenPoint(transform.position + Vector3.up * 1.5f);

        if (screen.z <= 0f)
        {
            return;
        }

        GUI.Label(
            new Rect(
                screen.x - 55f,
                Screen.height - screen.y - 20f,
                140f,
                25f
            ),
            $"P{OwnerClientId}: {score.Value}"
        );

    }
}
