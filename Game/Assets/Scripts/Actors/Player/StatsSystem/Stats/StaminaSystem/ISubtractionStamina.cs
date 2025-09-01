namespace PlayerNameSpace
{
    public interface ISubtractionStamina
    {
        public int CurrentStamina { get; }
        void SubtractionStamina(int value); 
    }
}