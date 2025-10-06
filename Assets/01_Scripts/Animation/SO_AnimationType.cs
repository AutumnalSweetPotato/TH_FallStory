
using UnityEngine;

[CreateAssetMenu(fileName = "so_AnimationType", menuName = "ScriptableObject/Animation Type")]
public class SO_AnimationType : ScriptableObject
{
    public AnimationClip animationClip;
    public AnimationName animationName;
    public CharacterPartAnimator characterPart;
    public PartVarinatColour partVarinatColour;
    public PartVarinatType partVarinatType;
}
