using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Dialogue Script", menuName = "Dialogue Script")]
public class DialogueScript : ScriptableObject
{
    // names, charImages, envTags, and sentences are parallel arrays
    public string[] names;
    public Sprite[] charImages;

    /* 
        tags for dialogue
        [0] C, K - CHANGE, KEEP the background sprite

       tags for cutscene
        [0] C - CHANGE the cutscene sprite
        [1] P - PAUSE for x seconds
        [2] 2 - SECOND char animate in
    */


    public string[] envTags;
    public Sprite[] envImages;

    [TextArea(5, 50)]
    public string[] sentences;
}
