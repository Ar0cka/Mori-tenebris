namespace DefaultNamespace.PlayerStatsOperation.IPlayerData
{
    public interface IGetPlayerStat
    {
        PlayerDataStats TryGetStat(string name);
        public PlayerDataDontChangableStats GetPlayerDataStaticStats();
    }
}