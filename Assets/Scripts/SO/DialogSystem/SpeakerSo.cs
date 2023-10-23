using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class SpeakerSo : ScriptableObject
{
    public SpeakerName speakerName;
    public Sprite speakerHead;
}

public enum SpeakerName
{
    LWH, Cube,
}