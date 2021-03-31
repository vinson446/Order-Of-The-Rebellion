using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TacticsUnit : MonoBehaviour
{
    public enum Type { Power, Speed, Tech};

    [Header("General")]
    public string Name;
    public Sprite charFullSprite;
    public Sprite charFaceSprite;
    public Sprite charFaceBodySprite;
    public Type type;

    [Header("Level Settings")]
    public int level;
    public int upgradeLvl;
    public int exp;

    [Header("HP Settings")]
    public int currentHP;
    public int maxHP;
    public int currentNRG;
    public int maxNRG;

    [Header("Combat Settings")]
    public int ATK;
    public int DEF;
    public int ACC;
    public int EVA;
    public int CRIT;
    public int MOV;

    [Header("Back End Settings")]
    public int moveSpeed;
}
