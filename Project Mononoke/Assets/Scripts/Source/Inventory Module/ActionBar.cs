using Source.ItemsModule;

namespace Source.InventoryModule
{
    public class ActionBar
    {
        private readonly object[,] _actionBar = null;

        public ActionBar(int numberOfRows,int RowSizes)
        {
            _actionBar = new IUsable[numberOfRows, RowSizes];
        }

        public void AddToHotBarSlot(int row, int slot, object actionScenario)
        {
            _actionBar[row, slot] = actionScenario;
        }

        public void InvokeAction(int row, int slot)
        {
            var actionScenario = _actionBar[row, slot];

            if(actionScenario == null) return;

            switch(actionScenario.GetType())
            {
                case IUsable: 
                var usableItem = (IUsable)actionScenario;
                usableItem.Use();
                break;
                default:
                return;
            }
        }
    }
}
