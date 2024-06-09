using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Source.PickUpModule
{
    public class ParabolicMotionAnimationPlayer : MonoBehaviour
    {
        [SerializeField] private AnimationCurve _animation = null;
        [SerializeField] private float _animationDuration = 0f;
        [SerializeField] private float _maxHieght = 0f;

        private FallingCurveVertexesGenerator _vertexesGenerator = null;

        public void Initialize()
        {
            _vertexesGenerator = new FallingCurveVertexesGenerator(_animation, _animationDuration);
        }

        public async void PlayAnimationBetween(Vector3 startPosition, Vector3 targetPosition)
        {
            var anchors = GetAnimationsAnchorsFor(startPosition, targetPosition);
            await PlayAnimationAsync(anchors);
        }

        private IEnumerable<Vector3> GetAnimationsAnchorsFor(Vector3 startPosition,Vector3 targetPosition)
        {
            return _vertexesGenerator.GetCurveVertexesBetween(startPosition, targetPosition, _maxHieght);
        }
        
        private async UniTask PlayAnimationAsync(IEnumerable<Vector3> anchors)
        {
            foreach (var anchor in anchors)
            {
                transform.position = anchor;
                await UniTask.Yield();
            }
        }
    }
}