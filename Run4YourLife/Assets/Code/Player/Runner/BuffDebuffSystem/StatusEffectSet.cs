using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StatusEffectSet", menuName = "Custom/StatusEffect/StatusEffectSet")]
public class StatusEffectSet : ScriptableObject {

    public List<StatusEffect> statusEffects = new List<StatusEffect>();
}
