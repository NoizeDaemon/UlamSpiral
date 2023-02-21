using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.VisualTree;
using DynamicData.Binding;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using UlamSpiral.ViewModels;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Input;
using System.Linq;

namespace UlamSpiral.Views
{
    public partial class MainView : UserControl   
    {
        public double Radius = 5;
        bool goBtnIsReset = false;

        public MainView()
        {
            InitializeComponent();
            //itemsControl.SizeChanged += ItemsControlSizeChanged;
            //itemsControl.PropertyChanged += ItemsControlPropertyChanged;
            //goBtn.CommandParameter = "";
        }

        //private void ItemsControlPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        //{
        //    Debug.WriteLine("Property changed");
        //}

        //private void ItemsControlSizeChanged(object? sender, SizeChangedEventArgs e)
        //{
        //    Debug.WriteLine("Size changed");
        //}

        void OnGoBtnClick(object? sender, RoutedEventArgs e)
        {
            //if (goBtnIsReset == false)
            //{
            //    upperLimitInput.IsEnabled = false;
            //    goBtn.Content = "Reset";
            //    goBtn.CommandParameter = "Reset";
            //    goBtnIsReset = true;
            //}
            //else
            //{
            //    upperLimitInput.IsEnabled = true;
            //    goBtn.Content = "Calculate!";
            //    goBtn.CommandParameter = "";
            //    goBtnIsReset = false;
            //}
        }
    }
}