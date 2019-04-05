﻿#pragma checksum "..\..\ViewForecastsWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "85D5FA6F0750282B02C13659E95D97C2FF4D4EA9"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using MaterialDesignThemes.Wpf;
using MaterialDesignThemes.Wpf.Converters;
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
using _18003144_Task_1_v2;


namespace _18003144_Task_1_v2 {
    
    
    /// <summary>
    /// ViewForecastsWindow
    /// </summary>
    public partial class ViewForecastsWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid grdMain;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel pnlNavigation;
        
        #line default
        #line hidden
        
        
        #line 23 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnHome;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnViewForecasts;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnAddForecast;
        
        #line default
        #line hidden
        
        
        #line 32 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ListBox lstCities;
        
        #line default
        #line hidden
        
        
        #line 34 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtpFrom;
        
        #line default
        #line hidden
        
        
        #line 36 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DatePicker dtpTo;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnGetForecasts;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TabControl tbctrlForecasts;
        
        #line default
        #line hidden
        
        
        #line 46 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal MaterialDesignThemes.Wpf.Card crdError;
        
        #line default
        #line hidden
        
        
        #line 48 "..\..\ViewForecastsWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBlock lblError;
        
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
            System.Uri resourceLocater = new System.Uri("/18003144_Task 1_v2;component/viewforecastswindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\ViewForecastsWindow.xaml"
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
            
            #line 16 "..\..\ViewForecastsWindow.xaml"
            ((_18003144_Task_1_v2.ViewForecastsWindow)(target)).Loaded += new System.Windows.RoutedEventHandler(this.Window_Loaded);
            
            #line default
            #line hidden
            
            #line 16 "..\..\ViewForecastsWindow.xaml"
            ((_18003144_Task_1_v2.ViewForecastsWindow)(target)).Closed += new System.EventHandler(this.Window_Closed);
            
            #line default
            #line hidden
            return;
            case 2:
            this.grdMain = ((System.Windows.Controls.Grid)(target));
            return;
            case 3:
            this.pnlNavigation = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 4:
            this.btnHome = ((System.Windows.Controls.Button)(target));
            
            #line 23 "..\..\ViewForecastsWindow.xaml"
            this.btnHome.Click += new System.Windows.RoutedEventHandler(this.BtnHome_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnViewForecasts = ((System.Windows.Controls.Button)(target));
            return;
            case 6:
            this.btnAddForecast = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\ViewForecastsWindow.xaml"
            this.btnAddForecast.Click += new System.Windows.RoutedEventHandler(this.BtnAddForecast_Click);
            
            #line default
            #line hidden
            return;
            case 7:
            this.lstCities = ((System.Windows.Controls.ListBox)(target));
            return;
            case 8:
            this.dtpFrom = ((System.Windows.Controls.DatePicker)(target));
            
            #line 34 "..\..\ViewForecastsWindow.xaml"
            this.dtpFrom.SelectedDateChanged += new System.EventHandler<System.Windows.Controls.SelectionChangedEventArgs>(this.DtpFrom_SelectedDateChanged);
            
            #line default
            #line hidden
            return;
            case 9:
            this.dtpTo = ((System.Windows.Controls.DatePicker)(target));
            return;
            case 10:
            this.btnGetForecasts = ((System.Windows.Controls.Button)(target));
            
            #line 37 "..\..\ViewForecastsWindow.xaml"
            this.btnGetForecasts.Click += new System.Windows.RoutedEventHandler(this.BtnGetForecasts_Click);
            
            #line default
            #line hidden
            return;
            case 11:
            this.tbctrlForecasts = ((System.Windows.Controls.TabControl)(target));
            return;
            case 12:
            this.crdError = ((MaterialDesignThemes.Wpf.Card)(target));
            return;
            case 13:
            this.lblError = ((System.Windows.Controls.TextBlock)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

