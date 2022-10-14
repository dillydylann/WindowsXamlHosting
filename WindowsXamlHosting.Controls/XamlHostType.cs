// Part of the internals of the Windows XAML Hosting API
// Copyright (c) 2020 Dylan Briedis <dylan@dylanbriedis.com>

namespace Windows.UI.Xaml.Hosting.Controls
{
    /// <summary>
    /// Specifies what XAML host class to use when hosting UWP XAML content.
    /// </summary>
    public enum XamlHostType
    {
        /// <summary>
        /// Automatically chooses which host class is appropriate for the current Windows version. Your program must have an
        /// app manifest that specifies Windows 10 compatibility and a 'maxversiontested' value (only in version 1903 and higher) 
        /// that controls the XAML resources loaded for app compatibility.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Hosts the content like in a normal UWP app. Your program must have an app manifest that specifies Windows 10 compatibility
        /// and a 'maxversiontested' value (only in version 1903 and higher) that controls the XAML resources loaded for app compatibility.
        /// </summary>
        FrameworkView,

        /// <summary>
        /// Hosts the content in a presenter that is used for the credentials and UAC dialogs and ignores the app manifest completely.
        /// </summary>
        XamlPresenter,
    }
}
