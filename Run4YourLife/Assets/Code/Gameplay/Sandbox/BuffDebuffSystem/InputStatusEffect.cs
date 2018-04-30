using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum InputModifierType
{
    Plain, // Value to be added to the input
    Maximize, // Input gravitates to -1 or 1 depending on last input
    Override // Overrides input value
}

[CreateAssetMenu(fileName = "InputStatusEffect", menuName = "Custom/StatusEffect/InputStatusEffect")]
public class InputStatusEffect : StatusEffect
{
    public override StatusEffectType StatusEffectType { get { return StatusEffectType.Input; } }
    public InputModifierType inputModifierType;
    public string actionName;
    public float float_value;
    public bool bool_value;

    public override void Apply(GameObject gameObject)
    {
        InputController inputController = gameObject.GetComponent<InputController>();
        Debug.Assert(inputController != null);
        inputController.Add(this);
    }

    public override void Unapply(GameObject gameObject)
    {
        InputController inputController = gameObject.GetComponent<InputController>();
        Debug.Assert(inputController != null);
        inputController.Remove(this);
    }
}
