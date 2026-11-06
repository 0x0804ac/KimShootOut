using UnityEngine;

public static class RoomSettingsBootstrap
{
    private static RoomSettingsService sService;
    private static LocalRoomSettingsTransport sLocalTransport; //실제 네트워크에서 테스트 시 교체할 것

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetOnPlay()
    {
        sService?.Dispose();
        sService = null;
        sLocalTransport = null;
        GameLocalization.Reset(); //테스트 전용 (실제로는 여기에서 언어가 바뀌지는 않음)
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeBeforeSceneLoad()
    {
        InitializeLocal(1L, 1L); //Override with InstallTransport online
    }

    public static void InitializeLocal(long localUserID, long hostUserID)
    {
        LocalRoomSettingsTransport transport = new LocalRoomSettingsTransport(localUserID);
        InstallTransport(transport, hostUserID);
        sLocalTransport = transport;
    }

    public static void InstallTransport(IRoomSettingsTransport transport, long initialHostUserID)
    {
        if (transport == null)
        {
            throw new System.ArgumentNullException(nameof(transport));
        }

        sService?.Dispose();
        sLocalTransport = null;
        sService = new RoomSettingsService(transport, initialHostUserID);
    }

    public static RoomSettingsService GetService()
    {
        if (sService == null)
        {
            throw new System.InvalidOperationException("방 설정이 아직 초기화되지 않았습니다.");
        }
        return sService;
    }

    public static ERoomSettingsResult TryStartLocalGame()
    {
        if (sService == null || sLocalTransport == null)
        {
            return ERoomSettingsResult.NoAuthority;
        }
        return sService.TryStartGameOnAuthority(sLocalTransport.GetLocalUserID());
    }
}
