﻿#pragma checksum "..\..\ListarDesenhos.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "25EBA6E50C82AC86B0A89229E67D26B4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;
using Trabalho_BD_IHC;


namespace Trabalho_BD_IHC {
    
    
    /// <summary>
    /// ListarDesenhos
    /// </summary>
    public partial class ListarDesenhos : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 48 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button registarDesenhoBase;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button editarDesenhoBase;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button removerDesenhoBase;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label desenhosBaseLabel;
        
        #line default
        #line hidden
        
        
        #line 65 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView desenhosBaseLista;
        
        #line default
        #line hidden
        
        
        #line 113 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button registarDesenhoPersonalizado;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button editarDesenhoPersonalizado;
        
        #line default
        #line hidden
        
        
        #line 119 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button removerDesenhoPersonalizado;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label desenhosPersonalizadosLabel;
        
        #line default
        #line hidden
        
        
        #line 130 "..\..\ListarDesenhos.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView desenhosPersonalizadosLista;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Trabalho_BD_IHC;component/listardesenhos.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ListarDesenhos.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            
            #line 18 "..\..\ListarDesenhos.xaml"
            ((System.Windows.Controls.TabItem)(target)).Loaded += new System.Windows.RoutedEventHandler(this.getDesenhosBase);
            
            #line default
            #line hidden
            return;
            case 2:
            this.registarDesenhoBase = ((System.Windows.Controls.Button)(target));
            return;
            case 3:
            this.editarDesenhoBase = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.removerDesenhoBase = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.desenhosBaseLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 6:
            this.desenhosBaseLista = ((System.Windows.Controls.ListView)(target));
            
            #line 65 "..\..\ListarDesenhos.xaml"
            this.desenhosBaseLista.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.desenhos_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 82 "..\..\ListarDesenhos.xaml"
            ((System.Windows.Controls.TabItem)(target)).Loaded += new System.Windows.RoutedEventHandler(this.getDesenhosPers);
            
            #line default
            #line hidden
            return;
            case 8:
            this.registarDesenhoPersonalizado = ((System.Windows.Controls.Button)(target));
            return;
            case 9:
            this.editarDesenhoPersonalizado = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.removerDesenhoPersonalizado = ((System.Windows.Controls.Button)(target));
            return;
            case 11:
            this.desenhosPersonalizadosLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 12:
            this.desenhosPersonalizadosLista = ((System.Windows.Controls.ListView)(target));
            
            #line 130 "..\..\ListarDesenhos.xaml"
            this.desenhosPersonalizadosLista.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.desenhos_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

