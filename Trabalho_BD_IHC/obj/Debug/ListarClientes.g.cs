﻿#pragma checksum "..\..\ListarClientes.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "7E2A664F96629C8FF3B6D391B1BC60E0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Transitions;
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
    /// ListarClientes
    /// </summary>
    public partial class ListarClientes : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 44 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button registarCliente;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button editarCliente;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button detalhesCliente;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtnomeCl;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pesquisaNOME;
        
        #line default
        #line hidden
        
        
        #line 116 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pesquisaNCLIENTE;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pesquisaNIF;
        
        #line default
        #line hidden
        
        
        #line 118 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pesquisaMAIL;
        
        #line default
        #line hidden
        
        
        #line 123 "..\..\ListarClientes.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid clientes;
        
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
            System.Uri resourceLocater = new System.Uri("/Trabalho_BD_IHC;component/listarclientes.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ListarClientes.xaml"
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
            
            #line 10 "..\..\ListarClientes.xaml"
            ((Trabalho_BD_IHC.ListarClientes)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.registarCliente = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\ListarClientes.xaml"
            this.registarCliente.Click += new System.Windows.RoutedEventHandler(this.registarCliente_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.editarCliente = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\ListarClientes.xaml"
            this.editarCliente.Click += new System.Windows.RoutedEventHandler(this.editarCliente_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.detalhesCliente = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\ListarClientes.xaml"
            this.detalhesCliente.Click += new System.Windows.RoutedEventHandler(this.verdetalhesCliente);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 90 "..\..\ListarClientes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.txtnomeCl = ((System.Windows.Controls.TextBox)(target));
            
            #line 93 "..\..\ListarClientes.xaml"
            this.txtnomeCl.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtnomeCl_KeyUp);
            
            #line default
            #line hidden
            return;
            case 8:
            
            #line 98 "..\..\ListarClientes.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.searchButton_Click);
            
            #line default
            #line hidden
            return;
            case 9:
            this.pesquisaNOME = ((System.Windows.Controls.RadioButton)(target));
            
            #line 115 "..\..\ListarClientes.xaml"
            this.pesquisaNOME.Checked += new System.Windows.RoutedEventHandler(this.pesquisaNOME_Checked);
            
            #line default
            #line hidden
            return;
            case 10:
            this.pesquisaNCLIENTE = ((System.Windows.Controls.RadioButton)(target));
            
            #line 116 "..\..\ListarClientes.xaml"
            this.pesquisaNCLIENTE.Checked += new System.Windows.RoutedEventHandler(this.pesquisaNCLIENTE_Checked);
            
            #line default
            #line hidden
            return;
            case 11:
            this.pesquisaNIF = ((System.Windows.Controls.RadioButton)(target));
            
            #line 117 "..\..\ListarClientes.xaml"
            this.pesquisaNIF.Checked += new System.Windows.RoutedEventHandler(this.pesquisaNIF_Checked);
            
            #line default
            #line hidden
            return;
            case 12:
            this.pesquisaMAIL = ((System.Windows.Controls.RadioButton)(target));
            
            #line 118 "..\..\ListarClientes.xaml"
            this.pesquisaMAIL.Checked += new System.Windows.RoutedEventHandler(this.pesquisaMAIL_Checked);
            
            #line default
            #line hidden
            return;
            case 13:
            this.clientes = ((System.Windows.Controls.DataGrid)(target));
            
            #line 123 "..\..\ListarClientes.xaml"
            this.clientes.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.clientes_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 123 "..\..\ListarClientes.xaml"
            this.clientes.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.clientes_MouseDoubleClick);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 5:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.Controls.Control.MouseDoubleClickEvent;
            
            #line 62 "..\..\ListarClientes.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.verdetalhesCliente);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

