
using UnityEngine;

public class SceneTeleport : MonoBehaviour
{
    [SerializeField]private string nextSceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            SceneControllerManager.Instance.GoToNextScene(nextSceneName, Vector2.zero,false);
        }
    }
    
}
