using UnityEngine;

//방 설정 시스템 초기화
public class RoomSettingsBootstrap : MonoBehaviour
{
    private LocalRoomSettingsTransport transport;
    private RoomSettingsAuthority authority;

    public RoomSettingsService Service { get; private set; }

    private void Awake()
    {
        //테스트용 플레이어 ID
        long localPlayerId = RoomSettingsConstants.testPlayer1ID;

        authority = new RoomSettingsAuthority(localPlayerId);
        transport = new LocalRoomSettingsTransport(authority, localPlayerId);
        Service = new RoomSettingsService(transport);

        transport.Bind(Service);
    }

    public void StartGame()
    {
        if (authority.TryStartGame(RoomSettingsConstants.testPlayer1ID))
        {
            print("게임 시작");
            RoomSettingsData settings = Service.Current;
            //GameManager에 settings 전달
        }
    }

    private void OnDestroy()
    {
        Service?.Dispose();
    }
}