using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UlamSpiral.Models
{
    public partial class NumberItem : ObservableObject
    {
        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private int _number;

        [ObservableProperty]
        private bool _isPrime;

        [ObservableProperty]
        private Direction _direction;

        [ObservableProperty]
        private Direction _nextDirection;

        [ObservableProperty]
        private string? _neighbor;

        [ObservableProperty]
        private bool _visible = true;
    }

    public enum Direction
    {
        Unset,
        RightOf,
        Above,
        LeftOf,
        Below
    }
}
