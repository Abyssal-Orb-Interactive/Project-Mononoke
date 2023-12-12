using UnityEngine;

namespace Source.Input
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

            public InputActionEventArgs(ActionType action, Vector2 actionData)
            {
                Action = action;
                ActionData = actionData;
            }
        }   
    }
}