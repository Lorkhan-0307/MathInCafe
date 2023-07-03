using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
{
    private static T instance;

    public static T Instance
    {
        get
        {
            // 인스턴스가 이미 존재하는 경우 반환
            if (instance != null)
                return instance;

            // 인스턴스가 없는 경우 씬에서 찾아서 할당
            instance = FindObjectOfType<T>();

            // 씬에서 찾을 수 없는 경우 새로 생성하여 할당
            if (instance == null)
            {
                GameObject obj = new GameObject(typeof(T).Name);
                instance = obj.AddComponent<T>();
            }

            // 인스턴스를 유지하도록 설정
            DontDestroyOnLoad(instance.gameObject);

            return instance;
        }
    }

    protected virtual void Awake()
    {
        // 인스턴스가 이미 존재하는 경우 중복 생성 방지
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // 인스턴스를 할당
        instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        // 인스턴스가 파괴되면 참조 해제
        if (instance == this)
            instance = null;
    }
}