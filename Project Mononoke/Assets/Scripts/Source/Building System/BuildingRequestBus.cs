using System;
using System.Collections.Generic;
using UnityEngine;
using static Source.BuildingSystem.IBuildRequester;

namespace Source.BuildingSystem
{
    public static class BuildingRequestsBus
    {
        private static readonly HashSet<Action<BuildRequestEventArgs>> _handleActions = new();
        
        public static void Subscribe(Action<BuildRequestEventArgs> handleAction)
        {
            var subscriberType = handleAction.Target?.GetType();

            if(subscriberType == null || subscriberType != typeof(OnGridBuilder))
            {
                 Debug.LogWarning("Attempted to subscribe a non-OnGridBuilder object to BuildingRequestsBus.");
                 return;
            }

            _handleActions.Add(handleAction);
        }

        public static void Unsubscribe(Action<BuildRequestEventArgs> handleAction)
        {
            _handleActions.Remove(handleAction);
        }

        public static void MakeRequest(IBuildRequester requester, BuildRequestEventArgs buildRequestEventArg)
        {
            if(_handleActions.Count == 0) return;

            foreach(var action in _handleActions) action?.Invoke(buildRequestEventArg);
        }
    }
}
