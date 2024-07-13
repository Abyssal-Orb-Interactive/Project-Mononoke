using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Base.GameLoop
{
    public class GameLoop : MonoBehaviour
    {
        [SerializeField] private List<UpdatableCategories> _frameUpdateOrder = null;
        [SerializeField] private List<UpdatableCategories> _physicsUpdateOrder = null;
        [SerializeField] private List<UpdatableCategories> _atEndOfFrameUpdateOrder = null;

        private Dictionary<UpdatableCategories, HashSet<IUpdatable>> _categorizedUpdatables = new();

        public void RegisterUpdatable(IUpdatable updatable)
        {
            var updatableCategory = updatable.Category;
            CreateListForNewCategoryIfNecessary(updatableCategory);
            _categorizedUpdatables[updatableCategory].Add(updatable);
        }

        public void UnregisterUpdatable(IUpdatable updatable)
        {
            var category = updatable.Category;
            if (CategorizedUpdatablesDoesNotContains(category) || _categorizedUpdatables == null) return;
            var updatables = _categorizedUpdatables[category];
            if (!updatables.Contains(updatable)) return;
            updatables.Remove(updatable);
        }

        private void CreateListForNewCategoryIfNecessary(UpdatableCategories updatableCategory)
        {
            if (CategorizedUpdatablesDoesNotContains(updatableCategory))
                _categorizedUpdatables.Add(updatableCategory, new HashSet<IUpdatable>());
        }

        private bool CategorizedUpdatablesDoesNotContains(UpdatableCategories updatableCategory)
        {
            return !CategorizedUpdatablesContains(updatableCategory);
        }
        
        private bool CategorizedUpdatablesContains(UpdatableCategories updatableCategory)
        {
            return _categorizedUpdatables.ContainsKey(updatableCategory);
        }

        private void Update()
        {
            foreach (var updatable in _frameUpdateOrder.Where(CategorizedUpdatablesContains).SelectMany(category => _categorizedUpdatables[category]))
            {
                RunFrameUpdate(updatable);
            }
        }

        private async UniTaskVoid RunFrameUpdate(IUpdatable updatable)
        {
            await updatable.FrameUpdate();
        }

        private void LateUpdate()
        {
            foreach (var updatable in _atEndOfFrameUpdateOrder.Where(CategorizedUpdatablesContains).SelectMany(category => _categorizedUpdatables[category]))
            {
                RunAtEndFrameUpdate(updatable);
            }
        }
        
        private async UniTaskVoid RunAtEndFrameUpdate(IUpdatable updatable)
        {
            await updatable.AtEndFrameUpdate();
        }
        
        private void FixedUpdate()
        {
            foreach (var updatable in _physicsUpdateOrder.Where(CategorizedUpdatablesContains).SelectMany(category => _categorizedUpdatables[category]))
            {
                RunPhysicsUpdate(updatable);
            }
        }
        
        private async UniTaskVoid RunPhysicsUpdate(IUpdatable updatable)
        {
            await updatable.PhysicsUpdate();
        }
    }
}