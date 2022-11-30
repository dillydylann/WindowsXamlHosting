// Part of the control wrappers of the Windows XAML Hosting API
// Copyright (c) 2022 Dylan Briedis <dylan@dylanbriedis.com>

using Windows.ApplicationModel.Activation;

namespace Windows.UI.Xaml.Hosting.Controls
{
    internal class NormalLaunchActivatedEventArgs : ILaunchActivatedEventArgs2
    {
        public string Arguments { get; }

        public NormalLaunchActivatedEventArgs(string arguments)
        {
            Arguments = arguments;
        }

        public TileActivatedInfo TileActivatedInfo => null;
        public string TileId => null;
        public ActivationKind Kind => ActivationKind.Launch;
        public ApplicationExecutionState PreviousExecutionState => ApplicationExecutionState.NotRunning;
        public SplashScreen SplashScreen => null;
    }
}
