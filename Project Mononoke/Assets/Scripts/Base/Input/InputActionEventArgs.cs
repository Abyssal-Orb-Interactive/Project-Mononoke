namespace Base.Input
{
    public partial class InputHandler
    {
        public struct InputActionEventArgs
        {
            public enum ActionType
            {
                Movement = 0,
            }

            public enum ActionStatus
            {
                Started = 0,
                Ended = 1
            }

            public ActionType Action { get; }
            public object ActionData { get; }
            
            public ActionStatus Status { get; }

            public InputActionEventArgs(ActionType action, MovementDirection actionData, ActionStatus status)
            {
                Action = action;
                ActionData = actionData;
                Status = status;
            }
        }   
    }
}