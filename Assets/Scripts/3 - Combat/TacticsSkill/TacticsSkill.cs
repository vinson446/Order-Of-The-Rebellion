using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TacticsSkill : MonoBehaviour
{
    [SerializeField] protected bool thirdSkillUnlocked;

    TacticsUnit unit;

    private void Awake()
    {
        unit = GetComponent<TacticsUnit>();    
    }

    public void UnlockThirdSkill()
    {
        thirdSkillUnlocked = true;
    }

    protected abstract void FirstSkill();

    protected abstract void SecondSkill();

    protected abstract void ThirdSkill();
}
