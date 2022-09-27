using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Animation", menuName = "AnimationData")]
public class AnimationData : ScriptableObject
{
    public int from, to;
    public bool loop;
    public int framesPerSecond;
    public int attackFrame;
}
