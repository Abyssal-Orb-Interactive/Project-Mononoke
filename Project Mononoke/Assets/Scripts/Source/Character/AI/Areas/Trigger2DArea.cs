using System;
using UnityEngine;

namespace Source.Character.AI.Areas
{
    [RequireComponent(typeof(Collider2D))]
    public class Trigger2DArea<T> : MonoBehaviour, IDisposable
    {
        public event Action<T> TargetEnteredInArea, TargetExitFromArea, TargetStayInArea = null;

        private static bool SomethingHasNotNecessaryComponent(Component something, out T targetComponent)
        {
            return !something.TryGetComponent(out targetComponent);
        }
        
        private void OnTriggerEnter2D(Collider2D something)
        {
            if(something.isTrigger) return;
            if(SomethingHasNotNecessaryComponent(something, out var targetComponent)) return;
            TargetEnteredInArea?.Invoke(targetComponent);
        }

        private void OnTriggerExit2D(Collider2D something)
        {
            if(SomethingHasNotNecessaryComponent(something, out var targetComponent)) return;
            TargetExitFromArea?.Invoke(targetComponent);
        }

        private void OnTriggerStay2D(Collider2D something)
        {
            if(SomethingHasNotNecessaryComponent(something, out var targetComponent)) return;
            TargetStayInArea?.Invoke(targetComponent);
        }

        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            TargetEnteredInArea = null;
            TargetExitFromArea = null;
            TargetStayInArea = null;
            GC.SuppressFinalize(this);
        }
    }
}