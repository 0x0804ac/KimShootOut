//방 설정 ID

public enum RoomSettingsID
{
    //팀 당 플레이어 수
    PlayersPerTeam,
    //게임 당 매치 수
    MatchesPerGame,
    //매치 당 라운드 수
    RoundsPerMatch,
    //승부가 이미 결정난 상황에서도 총 라운드 수만큼 강제 진행
    ForceFullRounds,
    //입력 제한 시간
    InputTimeLimit,
    //키커 최대 파워 입력까지 걸리는 시간
    KickerChargeSpeed,
    //골키퍼 최대 속력 입력까지 걸리는 시간
    KeeperChargeSpeed,
    //아이템 활성화 여부
    EnableItems,
    //아이템 선택 제한 시간
    ItemSelectionTimeLimit,
    //관전자 허용 여부
    AllowSpectators
}
