using System.Collections;
using UnityEngine;

namespace Tridi
{
    public sealed class CoroutineManager : MonoBehaviour
    {
        private static CoroutineManager _instance;
        private static CoroutineManager Instance
        {
            get
            {
                if (!_instance) InitManager();
                return _instance;
            }
        }

        private static void InitManager()
        {
            _instance = FindObjectOfType<CoroutineManager>();
            if (_instance) return;
            
            var go = new GameObject("CoroutineManager");
            _instance = go.AddComponent<CoroutineManager>();
            DontDestroyOnLoad(_instance);
        }
        
        public new static Coroutine StartCoroutine(string methodName, object value = null)
        {
            return ((MonoBehaviour)Instance).StartCoroutine(methodName, value);
        }
        
        public new static Coroutine StartCoroutine(IEnumerator routine)
        {
            return ((MonoBehaviour)Instance).StartCoroutine(routine);
        }

        public new static void StopCoroutine(string methodName)
        {
            ((MonoBehaviour)Instance).StopCoroutine(methodName);
        }

        public new static void StopCoroutine(IEnumerator routine)
        {
            ((MonoBehaviour)Instance).StopCoroutine(routine);
        }
    }
}