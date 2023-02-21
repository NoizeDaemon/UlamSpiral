using Avalonia.Markup.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using UlamSpiral.Models;

namespace UlamSpiral.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        [ObservableProperty]
        private double itemsControlWidth, itemsControlHeight, itemRadius;

        [ObservableProperty]
        private int upperLimit;

        [ObservableProperty]
        private bool isUpdating = false;

        private List<int> primeList = new();
        private int highestUpperLimit = 1;
        private int lastUpperLimit = 1;
        private int direction = 1;  //Direction=0=>Unset,1=>Right,2=>Above,3=>Left,4=>Below
        private int lastMaxDirection = 1;
        private int maxStepsInDirection = 1;  //Increments every 2 direction changes (after UP & DOWN)
        private int lastMaxStepsInDirection = 1;
        private int currentStepInDirection = 0;
        private int lastMaxCurrentStepInDirection = 0;

        private readonly SourceCache<NumberItem, int> numberItemsSourceCache = new(NumberItem => NumberItem.Number);
        private readonly ReadOnlyObservableCollection<NumberItem> numberItems;
        public ReadOnlyObservableCollection<NumberItem> NumberItems => numberItems;

        private SourceCache<NumberItem, int> backUpSourceCache = new(NumberItem => NumberItem.Number);

        public MainViewModel()
        {
            ItemRadius = 30;

            numberItemsSourceCache
                .Connect()
                //.AutoRefresh(new TimeSpan((long)1000))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out numberItems)
                .Subscribe();

            primeList.Add(2);
        }

        bool IsPrimeCheck(int n)
        {
            if (n is 2) return true;
            //int guide = primeList.Count > 5 ? primeList.FindLastIndex(x => x < n / 2) : primeList.Count;

            for (int p = 0; p < primeList.Count; p++)
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
            Task task = Task.Factory.StartNew(UpdateNumberItems);
        }

        async Task OldCalculation()
        {
            IsUpdating = true;

            int startNumber = 2;

            if (UpperLimit > lastUpperLimit)
            {
                if (highestUpperLimit == 0)
                {
                    numberItemsSourceCache.AddOrUpdate(new NumberItem
                    {
                        Name = "N1",
                        Number = 1,
                        IsPrime = false,
                        Direction = Direction.Unset,
                        NextDirection = Direction.RightOf
                    });
                    await Task.Delay(1);
                }
                else
                {


                    startNumber = highestUpperLimit + 1;
                }

                if (UpperLimit > highestUpperLimit) highestUpperLimit = UpperLimit;
                lastUpperLimit = UpperLimit;

                
            }
            else
            {   
                for(int i = numberItemsSourceCache.Count; i > upperLimit; i--)
                {
                    numberItemsSourceCache.RemoveKey(i);
                    await Task.Delay(1);
                }
                
            }



            IsUpdating = false;
        }

        private void UpdateNumberItems()
        {
            IsUpdating = false;
            if (highestUpperLimit == 1) 
            {
                numberItemsSourceCache.AddOrUpdate(new NumberItem
                {
                    Name = "N1",
                    Number = 1,
                    IsPrime = false,
                    Direction = Direction.Unset,
                    NextDirection = Direction.RightOf
                });
                Thread.Sleep(10);
            }
            
            if (upperLimit > lastUpperLimit)
            {
                if (lastUpperLimit < highestUpperLimit)
                {
                    if (upperLimit < highestUpperLimit)
                    {
                        RetrieveNumberItems(upperLimit);
                    }                       
                    else
                    {
                        RetrieveNumberItems(highestUpperLimit);
                        CalculateNewNumbers(highestUpperLimit + 1, lastMaxCurrentStepInDirection, lastMaxStepsInDirection);
                    }               
                }
                else 
                {
                    CalculateNewNumbers(highestUpperLimit + 1, lastMaxCurrentStepInDirection, lastMaxStepsInDirection);
                }
            }
            else if (UpperLimit < lastUpperLimit) 
            {
                RemoveNumberItems();
            }

            UpdateBackUp();
            Thread.Sleep(1000);
            IsUpdating = false;
        }

        private void RemoveNumberItems()
        {
            for (int i = numberItemsSourceCache.Count; i > upperLimit; i--)
            {
                numberItemsSourceCache.RemoveKey(i);
                Thread.Sleep(10);
            }
        }

        private void RetrieveNumberItems(int lastItemToRetrieveFromBackup)
        {
            for (int i = lastUpperLimit + 1; i <= lastItemToRetrieveFromBackup; i++)
            {
                numberItemsSourceCache.AddOrUpdate((NumberItem)backUpSourceCache.Lookup(i));
                Thread.Sleep(10);
            }
        }

        private void CalculateNewNumbers(int startNumber, int currentStepInDirection, int maxStepsInDirection)
        {
            for (int i = startNumber; i <= UpperLimit; i++)
            {
                numberItemsSourceCache.AddOrUpdate(new NumberItem
                {
                    Name = "N" + i,
                    Number = i,
                    IsPrime = IsPrimeCheck(i),
                    Neighbor = "N" + (i - 1),
                    Direction = (Direction)direction
                });

                Thread.Sleep(10);
                Debug.WriteLine(i);

                NumberItems[i - 2].NextDirection = (Direction)direction;

                currentStepInDirection++;

                if (currentStepInDirection == maxStepsInDirection)
                {
                    currentStepInDirection = 0;
                    if ((Direction)direction is Direction.Above or Direction.Below) maxStepsInDirection++;
                    if ((Direction)direction is Direction.Below) direction = 1;
                    else direction++;
                }
            }

            lastMaxCurrentStepInDirection = currentStepInDirection;
            lastMaxStepsInDirection = maxStepsInDirection;
        }

        private void UpdateBackUp()
        {
            if (UpperLimit > highestUpperLimit)
            {
                highestUpperLimit = UpperLimit;

                backUpSourceCache.AddOrUpdate(numberItemsSourceCache.Items);
                Thread.Sleep(100);
                Debug.WriteLine($"Highest UpperLimit: {highestUpperLimit}, BackUpCount: {backUpSourceCache.Count}");
            }
               
            lastUpperLimit = UpperLimit;
        }
    }
}