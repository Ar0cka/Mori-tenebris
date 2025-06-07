namespace Actors.Enemy.Monsters.Slime.Data.Scripts
{
    public class SlimeShield
    {
        public float StartShield { get; private set; }
        
        public SlimeShield(float startShield)
        {
            StartShield = startShield;
        }

        public void DamageAfterJump(float damage)
        {
            StartShield -= damage;
        }
    }
}