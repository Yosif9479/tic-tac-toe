using UnityEngine;

namespace Runtime.Basics
{
    [DefaultExecutionOrder(-1)]
    public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance == null || !Instance.Equals(this)) Instance = this as T;
        }
    }
}