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

        public MainView()
        {
            InitializeComponent();
            itemsControl.SizeChanged += ItemsControlSizeChanged;
            itemsControl.PropertyChanged += ItemsControlPropertyChanged;
        }

        private void ItemsControlPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
        {
            Debug.WriteLine("Property changed");
        }

        private void ItemsControlSizeChanged(object? sender, SizeChangedEventArgs e)
        {
            Debug.WriteLine("Size changed");
        }
    }
}