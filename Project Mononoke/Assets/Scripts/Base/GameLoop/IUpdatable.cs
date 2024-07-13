using Cysharp.Threading.Tasks;

namespace Base.GameLoop
{
    public interface IUpdatable
    {
        public UpdatableCategories Category { get; }
        public UniTask FrameUpdate();
        public UniTask PhysicsUpdate();
        public UniTask AtEndFrameUpdate();
    }
}