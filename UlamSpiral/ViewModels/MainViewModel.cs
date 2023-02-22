using Avalonia.Markup.Data;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DynamicData;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using UlamSpiral.Models;

namespace UlamSpiral.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public Dispatcher viewDispatcher;

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
        private int lastMaxStepsInDirection = 1;
        private int lastMaxCurrentStepInDirection = 0;

        //private readonly SourceCache<NumberItem, int> numberItemsSourceCache = new(NumberItem => NumberItem.Number);
        //private readonly ReadOnlyObservableCollection<NumberItem> numberItems;
        //public ReadOnlyObservableCollection<NumberItem> NumberItems => numberItems;
        private ObservableCollection<NumberItem> numberItems = new();
        public ObservableCollection<NumberItem> NumberItems => numberItems;

        //private SourceCache<NumberItem, int> backUpSourceCache = new(NumberItem => NumberItem.Number);

        public MainViewModel()
        {
            ItemRadius = 30;

            //numberItemsSourceCache
            //    .Connect()
            //    //.AutoRefresh(new TimeSpan((long)1000))
            //    .ObserveOn(RxApp.MainThreadScheduler)
            //    .Bind(out numberItems)
            //    .Subscribe();

            primeList.Add(2);
        }

        bool IsPrimeCheck(int n)
        {
            if (n is 2) return true;

            for (int p = 0; p < primeList.Count; p++)
            {
                if (n % primeList[p] == 0)
                {
                    return false;
                }
                else if (primeList[p] > n / 2)
                {
                    break;
                }
            }
            primeList.Add(n);
            Debug.WriteLine(n);
            return true;
        }

        [RelayCommand]
        private async void StartCalculation()
        {
            Dispatcher.UIThread.InvokeAsync(UpdateNumberItems);
        }

        //async Task OldCalculation()
        //{
        //    IsUpdating = true;

        //    int startNumber = 2;

        //    if (UpperLimit > lastUpperLimit)
        //    {
        //        if (highestUpperLimit == 0)
        //        {
        //            numberItemsSourceCache.AddOrUpdate(new NumberItem
        //            {
        //                Name = "N1",
        //                Number = 1,
        //                IsPrime = false,
        //                Direction = Direction.Unset,
        //                NextDirection = Direction.RightOf
        //            });
        //            await Task.Delay(1);
        //        }
        //        else
        //        {


        //            startNumber = highestUpperLimit + 1;
        //        }

        //        if (UpperLimit > highestUpperLimit) highestUpperLimit = UpperLimit;
        //        lastUpperLimit = UpperLimit;

                
        //    }
        //    else
        //    {   
        //        for(int i = numberItemsSourceCache.Count; i > upperLimit; i--)
        //        {
        //            //numberItemsSourceCache.RemoveKey(i);
        //            await Task.Delay(1);
        //        }
                
        //    }



        //    IsUpdating = false;
        //}

        private async Task UpdateNumberItems()
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            IsUpdating = false;
            if (highestUpperLimit == 1) 
            {
                //numberItemsSourceCache.AddOrUpdate(new NumberItem
                NumberItems.Add(new NumberItem
                {
                    Name = "N1",
                    Number = 1,
                    IsPrime = false,
                    Direction = Direction.Unset,
                    NextDirection = Direction.RightOf
                });
                await Task.Delay(1);
            }
            
            if (UpperLimit > lastUpperLimit)
            {
                if (lastUpperLimit < highestUpperLimit)
                {
                    if (UpperLimit < highestUpperLimit)
                    {
                        await RetrieveNumberItems(UpperLimit);
                    }                       
                    else
                    {
                        await RetrieveNumberItems(highestUpperLimit);
                        await AsyncCalculateNewNumbers(highestUpperLimit + 1, lastMaxCurrentStepInDirection, lastMaxStepsInDirection);
                    }               
                }
                else 
                {
                    await AsyncCalculateNewNumbers(highestUpperLimit + 1, lastMaxCurrentStepInDirection, lastMaxStepsInDirection);
                }
            }
            else if (UpperLimit < lastUpperLimit) 
            {
                await RemoveNumberItems();
            }

            UpdateBackUp();
            stopwatch.Stop();
            Debug.WriteLine($"Finished calculation in {stopwatch.Elapsed.TotalMilliseconds} ms.");
        }

        private async Task RemoveNumberItems()
        {
            //for (int i = numberItemsSourceCache.Count; i > upperLimit; i--)
            for (int i = NumberItems.Count - 1; i >= UpperLimit; i--)
            {
                //numberItemsSourceCache.RemoveKey(i);
                NumberItems.RemoveAt(i);
                await Task.Delay(1);
            }
        }

        private async Task RetrieveNumberItems(int lastItemToRetrieveFromBackup)
        {
            for (int i = lastUpperLimit; i < lastItemToRetrieveFromBackup; i++)
            {
                //numberItemsSourceCache.AddOrUpdate((NumberItem)backUpSourceCache.Lookup(i));
                //NumberItems.Add((NumberItem)backUpSourceCache.Lookup(i));
                NumberItems[i].Visible = true;
                await Task.Delay(1);
            }
        }

        private async Task AsyncCalculateNewNumbers(int startNumber, int currentStepInDirection, int maxStepsInDirection)
        {
            for (int i = startNumber; i <= UpperLimit; i++)
            {
                //numberItemsSourceCache.AddOrUpdate(new NumberItem
                NumberItems.Add(new NumberItem
                {
                    Name = "N" + i,
                    Number = i,
                    IsPrime = IsPrimeCheck(i),
                    Neighbor = "N" + (i - 1),
                    Direction = (Direction)direction
                });

                //Debug.WriteLine(i);
                await Task.Delay(1);

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
                //backUpSourceCache.AddOrUpdate(NumberItems);
                highestUpperLimit = UpperLimit;
                //Debug.WriteLine($"Highest UpperLimit: {highestUpperLimit}, BackUpCount: {backUpSourceCache.Count}");
            }             
            lastUpperLimit = UpperLimit;
            IsUpdating = false;
        }
    }
}