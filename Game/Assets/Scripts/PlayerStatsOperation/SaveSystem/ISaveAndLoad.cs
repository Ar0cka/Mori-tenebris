namespace DefaultNamespace.PlayerStatsOperation.SaveSystem
{
    public interface ISaveAndLoad
    {
        void SaveData(object saveObject, string filePath);
        T LoadData<T>(string filePath);
    }
}