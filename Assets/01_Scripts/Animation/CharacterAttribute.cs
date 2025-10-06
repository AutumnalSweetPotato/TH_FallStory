
using UnityEngine;
[System.Serializable]
public struct CharacterAttribute
{
    public CharacterPartAnimator characterPart;
    public PartVarinatColour partVarinatColour;
    public PartVarinatType partVarinatType;
    public CharacterAttribute(CharacterPartAnimator characterPart, PartVarinatColour partVarinatColour, PartVarinatType partVarinatType)
    {
        this.characterPart = characterPart;
        this.partVarinatColour = partVarinatColour;
        this.partVarinatType = partVarinatType;
    }
}

