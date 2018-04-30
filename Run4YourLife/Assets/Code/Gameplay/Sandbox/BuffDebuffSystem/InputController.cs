using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Run4YourLife.Input;

public enum InputModifierType
{
    Override,
    Plain
}

public class InputController : MonoBehaviour {

    protected Dictionary<Action, List<InputStatusEffect>> m_inputStatusEffects = new Dictionary<Action, List<InputStatusEffect>>();
    private ControlScheme m_controlScheme;

    private void Awake()
    {
        m_controlScheme = GetComponent<ControlScheme>();
        Debug.Assert(m_controlScheme != null);

        foreach(Action action in m_controlScheme.Actions)
        {
            m_inputStatusEffects.Add(action, new List<InputStatusEffect>());
        }
    }


    public bool Started(Action action)
    {
        Debug.Assert(action.InputSource.inputSoruceType == InputSourceType.Button);
        bool started = action.Started();

        foreach(InputStatusEffect inputStatusEffect in m_inputStatusEffects[action])
        {
            switch(inputStatusEffect.inputModifierType)
            {
                case InputModifierType.Override:
                    started = inputStatusEffect.bool_value;
                    break;
                case InputModifierType.Plain:
                    started = started || inputStatusEffect.bool_value;
                    break;
            }
        }
        return started;
    }

    public float Value(Action action)
    {
        Debug.Assert(action.InputSource.inputSoruceType == InputSourceType.Button);
        float value = action.Value();

        foreach (InputStatusEffect inputStatusEffect in m_inputStatusEffects[action])
        {
            switch (inputStatusEffect.inputModifierType)
            {
                case InputModifierType.Override:
                    value = inputStatusEffect.float_value;
                    break;
                case InputModifierType.Plain:
                    value = value + inputStatusEffect.float_value;
                    break;
            }
        }
        return value;
    }

    public void Remove(InputStatusEffect inputStatusEffect)
    {
        Action action = m_controlScheme.GetByName(inputStatusEffect.name);
        Debug.Assert(action != null);
        List<InputStatusEffect> inputStatusEffects = m_inputStatusEffects[action];
        Debug.Assert(inputStatusEffects != null);
        inputStatusEffects.Remove(inputStatusEffect);
    }

    public void Add(InputStatusEffect inputStatusEffect)
    {
        Action action = m_controlScheme.GetByName(inputStatusEffect.name);
        Debug.Assert(action != null);
        List<InputStatusEffect> inputStatusEffects = m_inputStatusEffects[action];
        Debug.Assert(inputStatusEffects != null);
        inputStatusEffects.Remove(inputStatusEffect);
    }
}
