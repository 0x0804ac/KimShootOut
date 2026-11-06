//방 설정 권한 확인
public static class RoomSettingsAuthority
{
    public static ERoomSettingsResult CheckEdit(
        ERoomPhase phase, long hostUserID, long requesterID)
    {
        if (phase != ERoomPhase.Waiting)
        {
            return ERoomSettingsResult.Locked;
        }

        if (hostUserID != requesterID)
        {
            return ERoomSettingsResult.NotHost;
        }

        return ERoomSettingsResult.Success;
    }
}
