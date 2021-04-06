using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticsSkill : MonoBehaviour
{
    [SerializeField] protected bool secondSkillUnlocked;
    public bool SecondSkillUnlocked { get => secondSkillUnlocked; }

    TacticsUnit unit;

    private void Awake()
    {
        unit = GetComponent<TacticsUnit>();    
    }

    public void UnlockSecondSkill()
    {
        secondSkillUnlocked = true;
    }

    protected abstract void FirstSkill();

    protected abstract void SecondSkill();

    protected abstract void ThirdSkill();
}
