using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : Single<VFXManager>
{
    private WaitForSeconds twoSecond;
    [SerializeField] private GameObject reapingPrefab;
    [SerializeField] private GameObject LeavesPrefab;
    [SerializeField] private GameObject ChoppingTrunkPrefab;
    [SerializeField] private GameObject pineConesFallingPrefab;
    [SerializeField] private GameObject breakingStonePrefab;

    protected override void Awake()
    {
        base.Awake();
        twoSecond = new WaitForSeconds(2f);
    }

    private void OnEnable()
    {
        EventHandler.HarvestActionEffectEvent += displayHarvestActionEffect;
    }


    private void OnDisable()
    {
        EventHandler.HarvestActionEffectEvent -= displayHarvestActionEffect;
    }

    private IEnumerator DisableHarvestActionEffect(GameObject effectGameObject,WaitForSeconds waitForSeconds)
    {
        yield return waitForSeconds;
        effectGameObject.SetActive(false);
    }
    private void displayHarvestActionEffect(Vector3 effectPosition, HarvestActionEffect effect)
    {
        switch (effect)
        {
            case HarvestActionEffect.reaping:
                GameObject reaping = PoolManager.Instance.ReuseObject(reapingPrefab, effectPosition, Quaternion.identity);
                reaping.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(reaping, twoSecond));
                break;
            case HarvestActionEffect.deciduousLeavesFalling:
                GameObject Leaves = PoolManager.Instance.ReuseObject(LeavesPrefab, effectPosition, Quaternion.identity);
                Leaves.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(Leaves, twoSecond));
                break;
            case HarvestActionEffect.choppingTreeTrunk:
                GameObject ChoppingTrunk = PoolManager.Instance.ReuseObject(ChoppingTrunkPrefab, effectPosition, Quaternion.identity);
                ChoppingTrunk.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(ChoppingTrunk, twoSecond));
                break;
            case HarvestActionEffect.pineConesFalling:
                GameObject pineConesFalling = PoolManager.Instance.ReuseObject(pineConesFallingPrefab, effectPosition, Quaternion.identity);
                pineConesFalling.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(pineConesFalling, twoSecond));
                break;
            case HarvestActionEffect.breakingStone:
                GameObject breakingStone = PoolManager.Instance.ReuseObject(breakingStonePrefab, effectPosition, Quaternion.identity);
                breakingStone.SetActive(true);
                StartCoroutine(DisableHarvestActionEffect(breakingStone, twoSecond));
                break;
        }
    }
}
