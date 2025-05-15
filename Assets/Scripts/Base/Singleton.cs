using System;
using UnityEngine;

namespace Entities.Base
{
    public abstract class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T m_Instance;

        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType<T>();
                    if (m_Instance == null)
                    {
                        GameObject obj = new GameObject(typeof(T).Name);
                        m_Instance = obj.AddComponent<T>();
                    }
                }

                return m_Instance;
            }
        }

        protected void Awake()
        {
            if (m_Instance == null)
            {
                m_Instance = this as T;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}