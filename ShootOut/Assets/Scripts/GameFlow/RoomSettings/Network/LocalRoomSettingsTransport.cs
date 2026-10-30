using System;

//테스트용 클래스 (로컬 서버)
public sealed class LocalRoomSettingsTransport : IRoomSettingsTransport
{
    private RoomSettingsService service;
    private readonly RoomSettingsAuthority authority;

    //로컬 플레이어 ID
    private readonly long localPlayerId;

    public bool IsServer
    {
        get
        {
            return true; //실제로 사용할 경우 서버인지 확인하는 로직 추가해야 함
        }
    }

    public bool IsLocalHost
    {
        get
        {
            return authority.IsHost(localPlayerId);
        }
    }

    public bool IsGameStarted
    {
        get
        {
            return authority.Phase != RoomPhase.PreGame;
        }
    }

    public event Action<RoomSettingsData> OnSettingsReceive;

    public LocalRoomSettingsTransport(RoomSettingsAuthority authority, long localPlayerId)
    {
        this.authority = authority;
        this.localPlayerId = localPlayerId;
    }

    //서비스 연결
    public void Bind(RoomSettingsService service)
    {
        this.service = service;
    }

    public void SendChangeRequest(RoomSettingsChange change)
    {
        if (service == null)
            return;

        if (!authority.CanEdit(localPlayerId))
            return;

        service.TryApplyAuthoritative(change);
    }

    public void PublishSettings(RoomSettingsData settings)
    {
        OnSettingsReceive?.Invoke(settings);
    }
}