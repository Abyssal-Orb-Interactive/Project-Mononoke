using System;
using System.Collections.Generic;

namespace Source.BuildingSystem
{
    public static partial class BuildRequestersRegister
    {
        private static event EventHandler<BuildRequesterRegistrationEventArgs> _buildRequesterRegistered = null;
        private static event EventHandler<BuildRequesterRegistrationEventArgs> _buildRequesterUnregistered = null;

        private static List<IBuildRequester> _registeredBuildRequesters = new();

        public static void RegisterBuildRequester(IBuildRequester buildRequester)
        {
            if(_registeredBuildRequesters.Contains(buildRequester)) return;

            _registeredBuildRequesters.Add(buildRequester);
            _buildRequesterRegistered?.Invoke(buildRequester , new BuildRequesterRegistrationEventArgs(buildRequester));
        }

        public static void UnregisterBuildRequester(IBuildRequester buildRequester)
        {
            if(!_registeredBuildRequesters.Contains(buildRequester)) return;

            _registeredBuildRequesters.Remove(buildRequester);
            _buildRequesterUnregistered?.Invoke(buildRequester, new BuildRequesterRegistrationEventArgs(buildRequester));
        }

        public static void AddBuildRequesterRegisteredHandler(EventHandler<BuildRequesterRegistrationEventArgs> handler)
        {
            _buildRequesterRegistered += handler;
        }

        public static void AddBuildRequesterUnregisteredHandler(EventHandler<BuildRequesterRegistrationEventArgs> handler)
        {
            _buildRequesterUnregistered += handler;
        }
    }
}
