using UnityEngine;
using UnityEngine.UI;

namespace Tridi
{
    [RequireComponent(typeof(Slider))]
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Health targetHealth;
        [SerializeField] private Gradient gradient;
        
        private Slider _slider;

        private void Awake()
        {
            _slider = GetComponent<Slider>();
        }

        private void Start()
        {
            OnHealthChange(targetHealth, new DamageInfo());
        }

        private void OnEnable()
        {
            targetHealth.onDamage += OnHealthChange;
            targetHealth.onHeal += OnHealthChange;
        }

        private void OnDisable()
        {
            targetHealth.onDamage -= OnHealthChange;
            targetHealth.onHeal -= OnHealthChange;
        }
        
        private void OnHealthChange(IHealth component, DamageInfo damageinfo)
        {
            _slider.value = component.Ratio;
            _slider.fillRect.GetComponent<Image>().color = gradient.Evaluate(component.Ratio);
        }
    }
}

