namespace Tridi
{
    public class Armor : Health
    {
        public override bool CanBeHealed(DamageInfo damageInfo)
        {
            return true;
        }
    }
}

