using System;

[Serializable]
//설정 데이터 구조체
public struct RoomSettingsData : IEquatable<RoomSettingsData>
{
     //팀 당 플레이어 수
    public int PlayersPerTeam;
    //게임 당 매치 수
    public int MatchesPerGame;
    //매치 당 라운드 수
    public int RoundsPerMatch;

    //승부가 이미 결정난 상황에서도 총 라운드 수만큼 강제 진행
    public bool ShouldForceFullRounds;

    //입력 제한 시간
    public int InputTimeLimitInSeconds;
    //키커 최대 파워 입력까지 걸리는 시간
    public float KickerFullChargeTimeInSeconds;
    //골키퍼 최대 속력 입력까지 걸리는 시간
    public float KeeperFullChargeTimeInSeconds;

    //아이템 활성화 여부
    public bool IsItemEnabled;
    //아이템 선택 제한 시간
    public int ItemSelectionTimeLimitInSeconds;

    //관전자 허용 여부
    public bool AllowSpectators;

    //다른 설정 데이터와 같은지 비교
    public bool Equals(RoomSettingsData other)
    {
        return PlayersPerTeam == other.PlayersPerTeam
            && MatchesPerGame == other.MatchesPerGame
            && RoundsPerMatch == other.RoundsPerMatch
            && ShouldForceFullRounds == other.ShouldForceFullRounds
            && InputTimeLimitInSeconds == other.InputTimeLimitInSeconds
            && KickerFullChargeTimeInSeconds == other.KickerFullChargeTimeInSeconds
            && KeeperFullChargeTimeInSeconds == other.KeeperFullChargeTimeInSeconds
            && IsItemEnabled == other.IsItemEnabled
            && ItemSelectionTimeLimitInSeconds == other.ItemSelectionTimeLimitInSeconds
            && AllowSpectators == other.AllowSpectators;
    }

    //인터페이스 구현 (Equals)
    public override bool Equals(object obj)
    {
        return obj is RoomSettingsData other && Equals(other);
    }

    //인터페이스 구현 (HashCode)
    public override int GetHashCode()
    {
        HashCode hash = new HashCode();
        hash.Add(PlayersPerTeam);
        hash.Add(MatchesPerGame);
        hash.Add(RoundsPerMatch);
        hash.Add(ShouldForceFullRounds);
        hash.Add(InputTimeLimitInSeconds);
        hash.Add(KickerFullChargeTimeInSeconds);
        hash.Add(KeeperFullChargeTimeInSeconds);
        hash.Add(IsItemEnabled);
        hash.Add(ItemSelectionTimeLimitInSeconds);
        hash.Add(AllowSpectators);
        return hash.ToHashCode();
    }
}