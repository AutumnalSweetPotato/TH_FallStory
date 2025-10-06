using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationOverrides : MonoBehaviour
{
    [SerializeField] private GameObject character;
    [SerializeField] private SO_AnimationType[] SO_AnimationTypes;

    private Dictionary<AnimationClip, SO_AnimationType> animationTypeDictionaryByAnimation;
    private Dictionary<string, SO_AnimationType> animationTypeDictionaryByCompositeAttributeKey;

    private void Start()
    {
        animationTypeDictionaryByAnimation = new Dictionary<AnimationClip, SO_AnimationType>();
        foreach (SO_AnimationType item in SO_AnimationTypes)
        {
            animationTypeDictionaryByAnimation.Add(item.animationClip, item);
        }
        animationTypeDictionaryByCompositeAttributeKey = new Dictionary<string, SO_AnimationType>();
        foreach (SO_AnimationType item in SO_AnimationTypes)
        {
            string compositeAttributeKey = item.characterPart.ToString()  + item.partVarinatColour.ToString() + item.partVarinatType.ToString() + item.animationName.ToString();
            animationTypeDictionaryByCompositeAttributeKey.Add(compositeAttributeKey, item);
        }
    }

    public void ApplyCharacterCustomisationParameters(List<CharacterAttribute> characterAttributes)
    {
        foreach (CharacterAttribute characterAttribute in characterAttributes)
        { 
            Animator currentAnimator = null;
            List<KeyValuePair<AnimationClip,AnimationClip>> animsKeyValuePairList = 
                new List<KeyValuePair<AnimationClip,AnimationClip>>();
            string animatorSOAssetName = characterAttribute.characterPart.ToString();
            Animator[] animatorsArray = character.GetComponentsInChildren<Animator>();
            foreach (Animator animator in animatorsArray)
            {
                if(animator.name == animatorSOAssetName)
                {
                    currentAnimator = animator;
                    break;
                }
            }

            AnimatorOverrideController aoc = new AnimatorOverrideController(currentAnimator.runtimeAnimatorController);
            List<AnimationClip> animationList = new List<AnimationClip>(aoc.animationClips);
            foreach (AnimationClip clip in animationList)
            {
                SO_AnimationType so_AnimationType;
                bool foundAnimation = animationTypeDictionaryByAnimation.TryGetValue(clip, out so_AnimationType);
                if(foundAnimation)
                {
                    string key = characterAttribute.characterPart.ToString() +
                        characterAttribute.partVarinatColour.ToString() + 
                        characterAttribute.partVarinatType.ToString()+
                        so_AnimationType.animationName.ToString();
                    SO_AnimationType swapSO_AnimationType;
                    bool foundSwapAnimation = animationTypeDictionaryByCompositeAttributeKey.
                        TryGetValue(key, out swapSO_AnimationType);
                    if(foundSwapAnimation)
                    {
                        AnimationClip swapAnimationClip = swapSO_AnimationType.animationClip;
                        animsKeyValuePairList.Add(new KeyValuePair<AnimationClip,AnimationClip>(clip,swapAnimationClip));
                    }
                }
            }
            aoc.ApplyOverrides(animsKeyValuePairList);
            currentAnimator.runtimeAnimatorController = aoc;
        }
        
    }
}
