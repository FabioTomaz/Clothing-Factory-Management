﻿#pragma checksum "..\..\ListarMateriais.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "45A13981AA32804A63013E3A6C989B57"
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
    /// ListarMateriais
    /// </summary>
    public partial class ListarMateriais : System.Windows.Controls.Page, System.Windows.Markup.IComponentConnector {
        
        
        #line 44 "..\..\ListarMateriais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button registarMaterial;
        
        #line default
        #line hidden
        
        
        #line 47 "..\..\ListarMateriais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button adicionarMaterial;
        
        #line default
        #line hidden
        
        
        #line 50 "..\..\ListarMateriais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button editarMaterial;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\ListarMateriais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button removerMaterial;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\ListarMateriais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button detalhesMaterial;
        
        #line default
        #line hidden
        
        
        #line 66 "..\..\ListarMateriais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Label materiaisLabel;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\ListarMateriais.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListView materiais;
        
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
            System.Uri resourceLocater = new System.Uri("/Trabalho_BD_IHC;component/listarmateriais.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ListarMateriais.xaml"
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
            
            #line 9 "..\..\ListarMateriais.xaml"
            ((Trabalho_BD_IHC.ListarMateriais)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Page_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.registarMaterial = ((System.Windows.Controls.Button)(target));
            
            #line 44 "..\..\ListarMateriais.xaml"
            this.registarMaterial.Click += new System.Windows.RoutedEventHandler(this.Encomenda_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.adicionarMaterial = ((System.Windows.Controls.Button)(target));
            return;
            case 4:
            this.editarMaterial = ((System.Windows.Controls.Button)(target));
            return;
            case 5:
            this.removerMaterial = ((System.Windows.Controls.Button)(target));
            
            #line 53 "..\..\ListarMateriais.xaml"
            this.removerMaterial.Click += new System.Windows.RoutedEventHandler(this.removerMaterial_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.detalhesMaterial = ((System.Windows.Controls.Button)(target));
            
            #line 56 "..\..\ListarMateriais.xaml"
            this.detalhesMaterial.Click += new System.Windows.RoutedEventHandler(this.detalhesMaterial_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.materiaisLabel = ((System.Windows.Controls.Label)(target));
            return;
            case 8:
            this.materiais = ((System.Windows.Controls.ListView)(target));
            
            #line 67 "..\..\ListarMateriais.xaml"
            this.materiais.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.materiais_SelectionChanged);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

