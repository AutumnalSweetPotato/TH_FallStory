using UnityEngine;

/// <summary>
/// 单例模式父类，适用于Unity的MonoBehaviour组件
/// 继承此类的子类将自动实现单例功能
/// </summary>
/// <typeparam name="T">子类类型</typeparam>
public class Single<T> : MonoBehaviour where T : MonoBehaviour
{
    // 单例实例
    private static T _instance;

    
    public static T Instance
    {
       get { return _instance; }
    }

    /// <summary>
    /// 确保在Awake阶段初始化
    /// </summary>
    protected virtual void Awake()
    {
        if(_instance == null)
            _instance = this as T;
        else
            Destroy(this.gameObject);
    }

   


}
