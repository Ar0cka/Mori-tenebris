namespace PlayerNameSpace
{
    public interface IRegenerationHealth
    {
        public int CurrentHitPoint { get; }
        public int GetMaxHealth();
        void Regeneration(int coutnRegeneration)
        {
            
        }
    }
}