﻿

#pragma checksum "E:\OneDrive\Programming\Personal\C#\TestMe\TestMeWP\3.GraphPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7CE3267CEFD5CB668FC96CBD5E0D201A"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TestMe
{
    partial class GraphPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 14 "..\..\3.GraphPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Loaded += this.pageRoot_Loaded;
                 #line default
                 #line hidden
                #line 15 "..\..\3.GraphPage.xaml"
                ((global::Windows.UI.Xaml.FrameworkElement)(target)).Unloaded += this.pageRoot_Unloaded;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 33 "..\..\3.GraphPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.AppBarButton_Click_2;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 48 "..\..\3.GraphPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Menu_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 71 "..\..\3.GraphPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.Button_Click2;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}

