using UnityEngine;

namespace Base.Input
{
    public partial class InputHandler
    {
        public class InputActionEventArgs
        {
            public enum ActionType
            {
                Movement = 0,
            }

            public ActionType Action { get; }
            public object ActionData { get; }

            public InputActionEventArgs(ActionType action, MovementDirection actionData)
            {
                Action = action;
                ActionData = actionData;
            }
        }   
    }
}