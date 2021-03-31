using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllyUnit : TacticsUnit
{
    [Header("Ally Char Unit Stuff")]
    public Sprite[] battlePrepSprites;
    public Sprite leaderSprite;

    [Header("Ally Skill Stuff")]
    public string[] skillName;
    public string[] skillType;
    public string[] skillPWR;
    public string[] skillNRG;
    [TextArea(5, 50)]
    public string[] skillDescript;
}
