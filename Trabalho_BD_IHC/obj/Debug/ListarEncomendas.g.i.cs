﻿#pragma checksum "..\..\ListarEncomendas.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E6951C01741B76D7336DE7D728F694FB"
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
    /// ListarEncomendas
    /// </summary>
    public partial class ListarEncomendas : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 44 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button registarEncomenda;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button entregarEncomenda;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button editarEncomenda;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button cancelarEncomenda;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button detalhesEncomenda;
        
        #line default
        #line hidden
        
        
        #line 89 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox txtInput;
        
        #line default
        #line hidden
        
        
        #line 110 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pesquisaNENCOMENDA;
        
        #line default
        #line hidden
        
        
        #line 111 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pesquisaNCLIENTE;
        
        #line default
        #line hidden
        
        
        #line 112 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton pesquisaNOMECLIENTE;
        
        #line default
        #line hidden
        
        
        #line 117 "..\..\ListarEncomendas.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid encomendas;
        
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
            System.Uri resourceLocater = new System.Uri("/Trabalho_BD_IHC;component/listarencomendas.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ListarEncomendas.xaml"
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
            
            #line 10 "..\..\ListarEncomendas.xaml"
            ((Trabalho_BD_IHC.ListarEncomendas)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.registarEncomenda = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\ListarEncomendas.xaml"
            this.registarEncomenda.Click += new System.Windows.RoutedEventHandler(this.registarEncomenda_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.entregarEncomenda = ((System.Windows.Controls.Button)(target));
            
            #line 47 "..\..\ListarEncomendas.xaml"
            this.entregarEncomenda.Click += new System.Windows.RoutedEventHandler(this.entregarEncomenda_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.editarEncomenda = ((System.Windows.Controls.Button)(target));
            
            #line 50 "..\..\ListarEncomendas.xaml"
            this.editarEncomenda.Click += new System.Windows.RoutedEventHandler(this.editarEncomenda_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cancelarEncomenda = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\ListarEncomendas.xaml"
            this.cancelarEncomenda.Click += new System.Windows.RoutedEventHandler(this.cancelarEncomenda_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.detalhesEncomenda = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\ListarEncomendas.xaml"
            this.detalhesEncomenda.Click += new System.Windows.RoutedEventHandler(this.detalhesEncomenda_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            
            #line 86 "..\..\ListarEncomendas.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click_1);
            
            #line default
            #line hidden
            return;
            case 8:
            this.txtInput = ((System.Windows.Controls.TextBox)(target));
            
            #line 91 "..\..\ListarEncomendas.xaml"
            this.txtInput.KeyDown += new System.Windows.Input.KeyEventHandler(this.txtsearchEn_KeyUp);
            
            #line default
            #line hidden
            return;
            case 9:
            
            #line 93 "..\..\ListarEncomendas.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            case 10:
            this.pesquisaNENCOMENDA = ((System.Windows.Controls.RadioButton)(target));
            
            #line 110 "..\..\ListarEncomendas.xaml"
            this.pesquisaNENCOMENDA.Checked += new System.Windows.RoutedEventHandler(this.pesquisaNENCOMENDA_Checked);
            
            #line default
            #line hidden
            return;
            case 11:
            this.pesquisaNCLIENTE = ((System.Windows.Controls.RadioButton)(target));
            
            #line 111 "..\..\ListarEncomendas.xaml"
            this.pesquisaNCLIENTE.Checked += new System.Windows.RoutedEventHandler(this.pesquisaNCLIENTE_Checked);
            
            #line default
            #line hidden
            return;
            case 12:
            this.pesquisaNOMECLIENTE = ((System.Windows.Controls.RadioButton)(target));
            
            #line 112 "..\..\ListarEncomendas.xaml"
            this.pesquisaNOMECLIENTE.Checked += new System.Windows.RoutedEventHandler(this.pesquisaNOMECLIENTE_Checked);
            
            #line default
            #line hidden
            return;
            case 13:
            this.encomendas = ((System.Windows.Controls.DataGrid)(target));
            
            #line 118 "..\..\ListarEncomendas.xaml"
            this.encomendas.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(this.encomendas_MouseDoubleClick);
            
            #line default
            #line hidden
            
            #line 118 "..\..\ListarEncomendas.xaml"
            this.encomendas.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.encomendas_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

