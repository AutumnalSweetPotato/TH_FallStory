using Cinemachine;

using UnityEngine;

public class SwitchSceneBound : MonoBehaviour
{
    private CinemachineConfiner2D confiner2D;
    private void Start()
    {
        confiner2D = GetComponent<CinemachineConfiner2D>();
    }
    private void OnEnable()
    {
        EventHandler.SceneLoadAfterEvent += SelectBound;
    }
    private void OnDisable()
    {
        EventHandler.SceneLoadAfterEvent -= SelectBound;
    }
    private void SelectBound()
    {
        PolygonCollider2D polygonCollider2D = 
            GameObject.FindGameObjectWithTag(Settings.Tags.SceneBound).GetComponent<PolygonCollider2D>();
        if(polygonCollider2D != null)
        confiner2D.m_BoundingShape2D = polygonCollider2D;
    }
}
