﻿#pragma checksum "..\..\CriarEncomenda.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6963CBA4CFEBDEF6C4177082E564B495"
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
    /// CriarEncomenda
    /// </summary>
    public partial class CriarEncomenda : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 39 "..\..\CriarEncomenda.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button confirmar;
        
        #line default
        #line hidden
        
        
        #line 40 "..\..\CriarEncomenda.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancelar;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\CriarEncomenda.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtClienteNif;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\CriarEncomenda.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtRefProd;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\CriarEncomenda.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtNIF;
        
        #line default
        #line hidden
        
        
        #line 76 "..\..\CriarEncomenda.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtGestVendas;
        
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
            System.Uri resourceLocater = new System.Uri("/Trabalho_BD_IHC;component/criarencomenda.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\CriarEncomenda.xaml"
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
            this.confirmar = ((System.Windows.Controls.Button)(target));
            
            #line 39 "..\..\CriarEncomenda.xaml"
            this.confirmar.Click += new System.Windows.RoutedEventHandler(this.confirmar_Click);
            
            #line default
            #line hidden
            return;
            case 2:
            this.cancelar = ((System.Windows.Controls.Button)(target));
            
            #line 40 "..\..\CriarEncomenda.xaml"
            this.cancelar.Click += new System.Windows.RoutedEventHandler(this.cancelar_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.txtClienteNif = ((System.Windows.Controls.TextBox)(target));
            return;
            case 4:
            this.txtRefProd = ((System.Windows.Controls.TextBox)(target));
            return;
            case 5:
            this.txtNIF = ((System.Windows.Controls.TextBox)(target));
            return;
            case 6:
            this.txtGestVendas = ((System.Windows.Controls.TextBox)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

