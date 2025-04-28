using Unity.Entities;
using Unity.Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Entities.Base
{
    public abstract partial class AuthoringSceneSystemGroup : ComponentSystemGroup
    {
        private bool m_Initialized;

        protected abstract string AuthoringSceneName { get; }

        protected override void OnCreate()
        {
            base.OnCreate();
            m_Initialized = false;
        }

        protected override void OnUpdate()
        {
            if (!m_Initialized)
            {
                if (SceneManager.GetActiveScene().isLoaded)
                {
                    var subScene = Object.FindObjectOfType<SubScene>();
                    if (subScene != null)
                    {
                        Enabled = AuthoringSceneName == subScene.SceneName;
                    }
                    else
                    {
                        Enabled = false;
                    }

                    m_Initialized = true;
                }
            }

            base.OnUpdate();
        }
    }
}