﻿#pragma checksum "..\..\ListarEmpregados.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "38099C80FDBBD6D3A451F537FC3C22EF"
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
using Xceed.Wpf.Toolkit;


namespace Trabalho_BD_IHC {
    
    
    /// <summary>
    /// ListarEmpregados
    /// </summary>
    public partial class ListarEmpregados : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 47 "..\..\ListarEmpregados.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button registarEmpregado;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\ListarEmpregados.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button editarEmpregado;
        
        #line default
        #line hidden
        
        
        #line 49 "..\..\ListarEmpregados.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button detalhesEmpregado;
        
        #line default
        #line hidden
        
        
        #line 90 "..\..\ListarEmpregados.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtnameSearch;
        
        #line default
        #line hidden
        
        
        #line 133 "..\..\ListarEmpregados.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid empregados;
        
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
            System.Uri resourceLocater = new System.Uri("/Trabalho_BD_IHC;component/listarempregados.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ListarEmpregados.xaml"
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
            
            #line 11 "..\..\ListarEmpregados.xaml"
            ((Trabalho_BD_IHC.ListarEmpregados)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.registarEmpregado = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\ListarEmpregados.xaml"
            this.registarEmpregado.Click += new System.Windows.RoutedEventHandler(this.registarEmpregado_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.editarEmpregado = ((System.Windows.Controls.Button)(target));
            
            #line 48 "..\..\ListarEmpregados.xaml"
            this.editarEmpregado.Click += new System.Windows.RoutedEventHandler(this.editarEmpregado_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.detalhesEmpregado = ((System.Windows.Controls.Button)(target));
            
            #line 49 "..\..\ListarEmpregados.xaml"
            this.detalhesEmpregado.Click += new System.Windows.RoutedEventHandler(this.verDetalhesEmpregado);
            
            #line default
            #line hidden
            return;
            case 6:
            this.txtnameSearch = ((System.Windows.Controls.TextBox)(target));
            
            #line 91 "..\..\ListarEmpregados.xaml"
            this.txtnameSearch.KeyUp += new System.Windows.Input.KeyEventHandler(this.txtsearchCl_KeyUp);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 93 "..\..\ListarEmpregados.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.SearchButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.empregados = ((System.Windows.Controls.DataGrid)(target));
            
            #line 133 "..\..\ListarEmpregados.xaml"
            this.empregados.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.empregados_SelectionChanged);
            
            #line default
            #line hidden
            
            #line 134 "..\..\ListarEmpregados.xaml"
            this.empregados.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.empregados_MouseDoubleClick);
            
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
            
            #line 59 "..\..\ListarEmpregados.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.verDetalhesEmpregado);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}

