using Avalonia.Data;
using ReactiveUI;
using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using UlamSpiral.Models;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Reactive.Linq;

namespace UlamSpiral.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private int upperLimit;

        private List<int> primeList = new();

        private readonly SourceList<NumberItemModel> numberItemsSourceList = new();
        private readonly ReadOnlyObservableCollection<NumberItemModel> numberItems;
        public ReadOnlyObservableCollection<NumberItemModel> NumberItems => numberItems;

        public MainViewModel()
        {
            numberItemsSourceList
                .Connect()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out numberItems)
                .Subscribe();

            primeList.Add(2);

            numberItemsSourceList.Add(new NumberItemModel
            {
                Name = "N1",
                Number = 1,
                IsPrime = false,
                Direction = Direction.Unset
            });
        }

        bool IsPrimeCheck(int n)
        {
            int guide = primeList.Count > 5 ? primeList.FindLastIndex(x => x < n / 2) : primeList.Count;

            for (int p = 0; p < guide; p++)
            {
                if (n % primeList[p] == 0)
                {
                    return false;
                }
            }
            primeList.Add(n);
            return true;
        }

        [RelayCommand]
        private void StartCalculation()
        {
            int direction = 1;  //Direction=0=>Unset,1=>Right,2=>Above,3=>Left,4=>Below
            int maxStepsInDirection = 1;  //Increments every 2 direction changes (after UP & DOWN)
            int currentStepInDirection = 0;

            //r.Next(2) > .5

            for (int i = 2; i <= upperLimit; i++)
            {
                var n = new NumberItemModel
                {
                    Name = "N" + i,
                    Number = i,
                    IsPrime = IsPrimeCheck(i),
                    Neighbor = "N" + (i - 1),
                    Direction = (Direction)direction
                };

                Debug.WriteLine($"{n.Name} (IsPrime={n.IsPrime}) added {n.Direction} {n.Neighbor}");
                numberItemsSourceList.Add(n);

                currentStepInDirection++;

                if (currentStepInDirection == maxStepsInDirection)
                {
                    currentStepInDirection = 0;
                    if ((Direction)direction is Direction.Above or Direction.Below) maxStepsInDirection++;
                    if ((Direction)direction is Direction.Below) direction = 1;
                    else direction++;
                }
            }
        }
    }
}