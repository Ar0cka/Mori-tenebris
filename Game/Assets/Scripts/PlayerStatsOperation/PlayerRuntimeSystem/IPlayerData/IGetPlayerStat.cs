namespace DefaultNamespace.PlayerStatsOperation.IPlayerData
{
    public interface IGetPlayerStat
    {
        public PlayerDataStats GetPlayerDataStats();
        public PlayerDataDontChangableStats GetPlayerDataStaticStats();
    }
}