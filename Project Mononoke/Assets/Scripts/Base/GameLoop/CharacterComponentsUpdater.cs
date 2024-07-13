using Cysharp.Threading.Tasks;
using Source.Character;
using Source.Character.Movement;

namespace Base.GameLoop
{
    public class CharacterComponentsUpdater : IUpdatable
    {
        private readonly IsoCharacterMover _mover = null;
        private readonly CharacterLogicIsometric2DCollider _logicCollider = null;

        public UpdatableCategories Category => UpdatableCategories.Characters;

        public CharacterComponentsUpdater(IsoCharacterMover mover, CharacterLogicIsometric2DCollider logicCollider)
        {
            _mover = mover;
            _logicCollider = logicCollider;
        }
        
        
        public async UniTask FrameUpdate()
        {
            _mover.UpdatePosition();
            _logicCollider.FrameByFrameCalculate();
            await UniTask.Yield();
        }

        public async UniTask PhysicsUpdate()
        {
            await UniTask.Yield();
        }

        public async UniTask AtEndFrameUpdate()
        {
            await UniTask.Yield();
        }
    }
}