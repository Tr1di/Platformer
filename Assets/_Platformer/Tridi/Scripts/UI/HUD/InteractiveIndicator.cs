using TMPro;
using Tridi;
using UnityEngine;

public class InteractiveScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Interactor interactor;

    private void Update()
    {
        text.enabled = interactor.HasInteractions();
    }
}
