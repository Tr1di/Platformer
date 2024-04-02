using Tridi.HealthEvents;

namespace Tridi
{
    namespace HealthEvents
    {
        public delegate void OnDamage(IHealth component, DamageInfo damageInfo);
        public delegate void OnDeath(IHealth component, DamageInfo damageInfo);
    }

    public interface IHealth
    {
        float Max { get; }
        float Ratio { get; }
        bool IsAlive { get; }

        event OnDamage onDamage;
        event OnDamage onHeal;
        event OnDeath onDeath;

        bool CanBeDamaged(DamageInfo damageInfo);
        float TakeDamage(DamageInfo damageInfo);

        bool CanBeHealed(DamageInfo damageInfo);
        float TakeHeal(DamageInfo damageInfo);
    }
}