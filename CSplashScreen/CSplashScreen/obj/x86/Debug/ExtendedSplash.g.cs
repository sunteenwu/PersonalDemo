﻿#pragma checksum "D:\GitHub\PersonalDemo\CSplashScreen\CSplashScreen\ExtendedSplash.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2CBFD9213896FB9A84CC96CC24707BAF"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CSplashScreen
{
    partial class ExtendedSplash : 
        global::Windows.UI.Xaml.Controls.Grid, 
        global::Windows.UI.Xaml.Markup.IComponentConnector,
        global::Windows.UI.Xaml.Markup.IComponentConnector2
    {
        /// <summary>
        /// Connect()
        /// </summary>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                {
                    this.ImageFlipView = (global::Windows.UI.Xaml.Controls.FlipView)(target);
                }
                break;
            case 2:
                {
                    this.BtnDismissSplash = (global::Windows.UI.Xaml.Controls.Button)(target);
                    #line 16 "..\..\..\ExtendedSplash.xaml"
                    ((global::Windows.UI.Xaml.Controls.Button)this.BtnDismissSplash).Click += this.BtnDismissSplash_Click;
                    #line default
                }
                break;
            default:
                break;
            }
            this._contentLoaded = true;
        }

        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 14.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public global::Windows.UI.Xaml.Markup.IComponentConnector GetBindingConnector(int connectionId, object target)
        {
            global::Windows.UI.Xaml.Markup.IComponentConnector returnValue = null;
            return returnValue;
        }
    }
}
