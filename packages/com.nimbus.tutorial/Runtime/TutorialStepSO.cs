using UnityEngine;

namespace Nimbus.Tutorial;

public abstract class TutorialStepSO : ScriptableObject
{
    [SerializeField] private string _id = string.Empty;

    public string Id => _id;
}
