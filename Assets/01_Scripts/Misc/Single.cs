using UnityEngine;

/// <summary>
/// ����ģʽ���࣬������Unity��MonoBehaviour���
/// �̳д�������ཫ�Զ�ʵ�ֵ�������
/// </summary>
/// <typeparam name="T">��������</typeparam>
public class Single<T> : MonoBehaviour where T : MonoBehaviour
{
    // ����ʵ��
    private static T _instance;

    
    public static T Instance
    {
       get { return _instance; }
    }

    /// <summary>
    /// ȷ����Awake�׶γ�ʼ��
    /// </summary>
    protected virtual void Awake()
    {
        if(_instance == null)
            _instance = this as T;
        else
            Destroy(this.gameObject);
    }

   


}
