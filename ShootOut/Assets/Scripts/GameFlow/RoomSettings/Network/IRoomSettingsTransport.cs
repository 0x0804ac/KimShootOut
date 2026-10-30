using System;

//네트워크 전달 인터페이스
public interface IRoomSettingsTransport
{
    //서버 여부
    bool IsServer { get; }
    //호스트 여부
    bool IsLocalHost { get; }
    //게임 시작 여부
    bool IsGameStarted { get; }

    //설정 수신 이벤트
    event Action<RoomSettingsData> OnSettingsReceive;

    //설정 변경 요청 전송
    void SendChangeRequest(RoomSettingsChange change);

    //설정 데이터 전송
    void PublishSettings(RoomSettingsData data);
}
