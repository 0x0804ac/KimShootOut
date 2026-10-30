using System;
using UnityEngine;

//방 설정 관리 서비스
public sealed class RoomSettingsService
{
    //현재 데이터 (private 필드)
    private RoomSettingsData current;
    //네트워크 전달 인터페이스
    private readonly IRoomSettingsTransport transport;

    //현재 데이터 (public 속성)
    public RoomSettingsData Current
    {
        get
        {
            return current;
        }
    }

    //변경 가능한지 검사
    public bool CanEdit
    {
        get
        {
            return transport.IsLocalHost && !transport.IsGameStarted;
        }
    }

    //설정 변경 이벤트
    public event Action<RoomSettingsData> OnSettingsChange;

    //생성자
    public RoomSettingsService(IRoomSettingsTransport transport)
    {
        this.transport = transport;
        current = RoomSettingsCatalog.CreateDefaultData();
        transport.OnSettingsReceive += ApplySnapshot;
    }

    //설정 변경 요청
    public void RequestChange(RoomSettingsChange change)
    {
        //변경 가능한지 검사
        if (!CanEdit)
        {
            return;
        }
        transport.SendChangeRequest(change);
    }

    //서버 권한 계층에서만 호출해야 함
    public RoomSettingsResult TryApplyAuthoritative(RoomSettingsChange change)
    {
        //권한 검사
        if (!transport.IsServer)
        {
            Debug.Log("서버 권한 없음");
            return RoomSettingsResult.NoAuthority;
        }
        //게임 시작 여부 검사
        if (transport.IsGameStarted)
        {
            Debug.Log("게임 시작됨");
            return RoomSettingsResult.GameAlreadyStarted;
        }
        RoomSettingsData updated = current;
        //설정 변경 값 검사
        if (!RoomSettingsValidator.TryApply(ref updated, change))
        {
            Debug.Log("유효하지 않은 값");
            return RoomSettingsResult.InvalidValue;
        }
        //설정 변경 이벤트 호출 및 데이터 전송
        if (!updated.Equals(current))
        {
            current = updated;
            OnSettingsChange?.Invoke(current);
            transport.PublishSettings(current);
        }
        return RoomSettingsResult.Success;
    }

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
    
    //이벤트 제거
    public void Dispose()
    {
        transport.OnSettingsReceive -= ApplySnapshot;
    }
}