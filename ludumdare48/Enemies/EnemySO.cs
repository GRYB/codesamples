using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/NewEnemy")]
public class EnemySO : ScriptableObject
{
    public Sprite sprite;
    public int hp;
    public float speed;
    public float damage;
    public AnimationClip idleAnimationClip;
}
