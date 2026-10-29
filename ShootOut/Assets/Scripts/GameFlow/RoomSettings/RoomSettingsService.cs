using System;

//방 설정 관리 서비스
public sealed class RoomSettingsService
{
    //현재 데이터 (private 필드)
    private RoomSettingsData current;
    //
    //private readonly IRoomSettingsTransport transport;

    //현재 데이터 (public 속성)
    public RoomSettingsData Current
    {
        get
        {
            return current;
        }
    }

    //변경 가능한지 검사
    //public bool CanEdit => transport.IsLocalHost && !transport.IsGameStarted;

    //설정 변경 이벤트
    public event Action<RoomSettingsData> OnSettingsChange;

    /*//
    public RoomSettingsService(IRoomSettingsTransport transport)
    {
        this.transport = transport;

        current = RoomSettingsCatalog.CreateDefaultData();

        transport.SettingsReceived += ApplySnapshot;
    }

    //
    public void RequestChange(RoomSettingChange change)
    {
        if (!CanEdit)
            return;

        transport.SendChangeRequest(change);
    }

    // 서버 권한 계층에서만 호출해야 하는 메서드
    public RoomSettingsResult TryApplyAuthoritative(
        RoomSettingsChange change)
    {
        if (!transport.IsServer)
            return RoomSettingsResult.NoAuthority;

        if (transport.IsGameStarted)
            return RoomSettingsResult.GameAlreadyStarted;

        var updated = current;

        if (!RoomSettingsValidator.TryApply(
            ref updated, change))
        {
            return RoomSettingsResult.InvalidValue;
        }

        if (!updated.Equals(current))
        {
            current = updated;
            OnSettingsChange?.Invoke(current);
            transport.PublishSettings(current);
        }

        return RoomSettingsResult.Success;
    }*/

    //설정 데이터 적용
    public void ApplySnapshot(RoomSettingsData snapshot)
    {
        //같은 데이터면 작업 불필요
        if (current.Equals(snapshot))
        {
            return;
        }
        current = snapshot;
        OnSettingsChange?.Invoke(current);
    }

    //이벤트 호출
    public void NotifyCurrent()
    {
        OnSettingsChange?.Invoke(current);
    }
    
    /*//이벤트 제거
    public void Dispose()
    {
        transport.SettingsReceived -= ApplySnapshot;
    }*/
}