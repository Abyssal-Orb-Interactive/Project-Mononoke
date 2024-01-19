using System;

namespace Source.BuildingSystem
{
    public static partial class BuildRequestersRegister
    {
        public class BuildRequesterRegistrationEventArgs
        {
            public IBuildRequester BuildRequester { get; }

            public BuildRequesterRegistrationEventArgs(IBuildRequester buildRequester)
            {
                BuildRequester = buildRequester;
            }
        }
    }
}
