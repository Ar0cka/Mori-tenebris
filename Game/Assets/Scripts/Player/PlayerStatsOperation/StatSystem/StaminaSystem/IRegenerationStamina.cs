namespace PlayerNameSpace
{
    public interface IRegenerationStamina
    {
        public int CurrentStamina { get; }
        public int GetMaxStamina();

        void RegenerationStamina(int value);
    }
}