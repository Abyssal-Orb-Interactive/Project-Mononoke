using System.Collections.Generic;
using UnityEngine;

namespace Base.UnityExtensions
{
    public class MonoBehavioursPool<T> : MonoBehaviour where T : MonoBehaviour
    {
        private readonly Queue<T> _monoBehaviours = new();
        private T _prefab = null;

        public int Count => _monoBehaviours.Count;

        public void Initialize(T prefab, int startCapacity = 0)
        {
            _prefab = prefab;

            Preload(startCapacity);
        }
        
        public void Preload(int count)
        {
            for (var i = 0; i < count; i++)
            {
                Add(CreateNewEmptyMonoBehaviour());
            } 
        }
        
        private T CreateNewEmptyMonoBehaviour()
        {
            var emptyMonoBehaviour = Instantiate(_prefab, Vector3.zero, Quaternion.identity, transform);
            emptyMonoBehaviour.gameObject.SetActive(false);
            return emptyMonoBehaviour;
        }

        public void Add(T monoBehaviour)
        {
            _monoBehaviours.Enqueue(monoBehaviour);
            monoBehaviour.gameObject.SetActive(false);
        }

        public T GetMonoBehaviour()
        {
            if (_monoBehaviours.Count <= 0) return CreateNewEmptyMonoBehaviour();
            
            var mono = _monoBehaviours.Dequeue();
            mono.gameObject.SetActive(true);
            return mono;
        }
    }
}