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
        public MainView()
        {
            InitializeComponent();
            
        }

        void OnResetButtonClick(object sender, RoutedEventArgs e)
        {
            var visuals = this.GetVisualDescendants();
            var panel = (RelativePanel)visuals.Where(x => x is RelativePanel).First();
            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }
    }
}