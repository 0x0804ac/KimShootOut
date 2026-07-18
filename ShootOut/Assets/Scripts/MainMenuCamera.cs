using UnityEngine;

public class MainMenuCamera
{
    static readonly Vector3 movement = new(0f, 0f, 6f);

    public static void MoveCameraLeft()
    {
        Camera.main.transform.position -= movement;
    }

    public static void MoveCameraRight()
    {
        Camera.main.transform.position += movement;
    }
}
