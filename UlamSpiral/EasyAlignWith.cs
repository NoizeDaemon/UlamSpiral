using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UlamSpiral.Models;

namespace UlamSpiral
{
    public class EasyAlignWith : RelativePanel
    {
        static EasyAlignWith()
        {
            AffectsParentArrange<RelativePanel>(DirectionProperty, TargetProperty, EasyRightOfProperty, EasyLeftOfProperty, EasyAboveProperty, EasyBelowProperty);
            AffectsParentMeasure<RelativePanel>(DirectionProperty, TargetProperty, EasyRightOfProperty, EasyLeftOfProperty, EasyAboveProperty, EasyBelowProperty);

            DirectionProperty.Changed.AddClassHandler<AvaloniaObject>(OnDirectionTargetPropertyChanged);
            TargetProperty.Changed.AddClassHandler<AvaloniaObject>(OnDirectionTargetPropertyChanged);
            EasyRightOfProperty.Changed.AddClassHandler<AvaloniaObject>(OnEasyRightOfPropertyChanged);
            EasyLeftOfProperty.Changed.AddClassHandler<AvaloniaObject>(OnEasyLeftOfPropertyChanged);
            EasyAboveProperty.Changed.AddClassHandler<AvaloniaObject>(OnEasyAbovePropertyChanged);
            EasyBelowProperty.Changed.AddClassHandler<AvaloniaObject>(OnEasyBelowPropertyChanged);
        }

        private static void OnDirectionTargetPropertyChanged(AvaloniaObject obj, AvaloniaPropertyChangedEventArgs e)
        {
            Direction direction = obj.GetValue(DirectionProperty);
            var targetValue = obj.GetValue(TargetProperty);

            if (direction != Direction.Unset && targetValue != AvaloniaProperty.UnsetValue && targetValue != null)
            {
                if (direction is Direction.RightOf)
                {
                    if (obj.GetValue(LeftOfProperty) == targetValue) RelativePanel.SetLeftOf(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(AboveProperty) == targetValue) RelativePanel.SetAbove(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(BelowProperty) == targetValue) RelativePanel.SetBelow(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(AlignHorizontalCenterWithProperty) == targetValue) RelativePanel.SetAlignHorizontalCenterWith(obj, AvaloniaProperty.UnsetValue);

                    RelativePanel.SetAlignVerticalCenterWith(obj, targetValue);
                    RelativePanel.SetRightOf(obj, targetValue);
                }
                else if (direction is Direction.LeftOf)
                {
                    if (obj.GetValue(RightOfProperty) == targetValue) RelativePanel.SetRightOf(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(AboveProperty) == targetValue) RelativePanel.SetAbove(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(BelowProperty) == targetValue) RelativePanel.SetBelow(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(AlignHorizontalCenterWithProperty) == targetValue) RelativePanel.SetAlignHorizontalCenterWith(obj, AvaloniaProperty.UnsetValue);

                    RelativePanel.SetAlignVerticalCenterWith(obj, targetValue);
                    RelativePanel.SetLeftOf(obj, targetValue);
                }
                else if (direction is Direction.Above)
                {
                    if (obj.GetValue(RightOfProperty) == targetValue) RelativePanel.SetRightOf(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(LeftOfProperty) == targetValue) RelativePanel.SetLeftOf(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(BelowProperty) == targetValue) RelativePanel.SetBelow(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(AlignVerticalCenterWithProperty) == targetValue) RelativePanel.SetAlignVerticalCenterWith(obj, AvaloniaProperty.UnsetValue);


                    RelativePanel.SetAlignHorizontalCenterWith(obj, targetValue);
                    RelativePanel.SetAbove(obj, targetValue);
                }
                else if (direction is Direction.Below)
                {
                    if (obj.GetValue(RightOfProperty) == targetValue) RelativePanel.SetRightOf(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(LeftOfProperty) == targetValue) RelativePanel.SetLeftOf(obj, AvaloniaProperty.UnsetValue);
                    if (obj.GetValue(BelowProperty) == targetValue) RelativePanel.SetBelow(obj, targetValue);
                    if (obj.GetValue(AlignVerticalCenterWithProperty) == targetValue) RelativePanel.SetAlignVerticalCenterWith(obj, AvaloniaProperty.UnsetValue);
  

                    RelativePanel.SetAlignHorizontalCenterWith(obj, targetValue);
                    RelativePanel.SetBelow(obj, targetValue);
                }
                else
                {
                    Debug.WriteLine(e.Sender + "\n" + "Direction not valid");
                }

            }
        }

        /////////////////
        /// Direction ///
        /////////////////

        public static Direction GetDirection(AvaloniaObject obj)
        {
            return obj.GetValue(DirectionProperty);
        }

        public static void SetDirection(AvaloniaObject obj, Direction value)
        {
            obj.SetValue(DirectionProperty, value);
        }

        public static readonly AttachedProperty<Direction> DirectionProperty =
            AvaloniaProperty.RegisterAttached<RelativePanel, Layoutable, Direction>("Direction");


        //////////////
        /// Target ///
        //////////////

        [ResolveByName]
        public static object GetTarget(AvaloniaObject obj)
        {
            return obj.GetValue(TargetProperty);
        }

        [ResolveByName]
        public static void SetTarget(AvaloniaObject obj, object value)
        {
            obj.SetValue(TargetProperty, value);
        }

        public static readonly AttachedProperty<object> TargetProperty =
            AvaloniaProperty.RegisterAttached<RelativePanel, Layoutable, object>("Target");

        public static Func<AvaloniaObject, object, object> CoerceTarget = delegate (AvaloniaObject obj, object valueIn)
        {
            if (valueIn is string)
            {
                var target = ((Control)obj).FindControl<Control>((string)valueIn);
                return target ?? AvaloniaProperty.UnsetValue;
            }
            else return AvaloniaProperty.UnsetValue;
        };

        /////////////
        /// RIGHT ///
        /////////////

        [ResolveByName]
        public static object GetEasyRightOf(AvaloniaObject obj)
        {
            return obj.GetValue(EasyRightOfProperty);
        }

        [ResolveByName]
        public static void SetEasyRightOf(AvaloniaObject obj, object value)
        {
            obj.SetValue(EasyRightOfProperty, value);
        }

        public static readonly AttachedProperty<object> EasyRightOfProperty =
            AvaloniaProperty.RegisterAttached<RelativePanel, Layoutable, object>("EasyRightOf");

        private static void OnEasyRightOfPropertyChanged(AvaloniaObject obj, AvaloniaPropertyChangedEventArgs e)
        {
            object value = e.NewValue;

            if (value != AvaloniaProperty.UnsetValue)
            {
                if (obj.GetValue(LeftOfProperty) == value) obj.SetValue(LeftOfProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(AboveProperty) == value) obj.SetValue(AboveProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(BelowProperty) == value) obj.SetValue(BelowProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(AlignHorizontalCenterWithProperty) == value) obj.SetValue(AlignHorizontalCenterWithProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyLeftOfProperty) == value) SetEasyLeftOf(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyAboveProperty) == value) SetEasyAbove(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyBelowProperty) == value) SetEasyBelow(obj, AvaloniaProperty.UnsetValue);
            }

            obj.SetValue(AlignVerticalCenterWithProperty, value);
            obj.SetValue(RightOfProperty, value);
        }


        ////////////
        /// LEFT ///
        ////////////

        [ResolveByName]
        public static object GetEasyLeftOf(AvaloniaObject obj)
        {
            return obj.GetValue(EasyLeftOfProperty);
        }

        [ResolveByName]
        public static void SetEasyLeftOf(AvaloniaObject obj, object value)
        {
            obj.SetValue(EasyLeftOfProperty, value);
        }

        public static readonly AttachedProperty<object> EasyLeftOfProperty =
            AvaloniaProperty.RegisterAttached<RelativePanel, Layoutable, object>("EasyLeftOf");

        private static void OnEasyLeftOfPropertyChanged(AvaloniaObject obj, AvaloniaPropertyChangedEventArgs e)
        {
            object value = e.NewValue;

            if (value != AvaloniaProperty.UnsetValue)
            {
                if (obj.GetValue(RightOfProperty) == value) obj.SetValue(RightOfProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(AboveProperty) == value) obj.SetValue(AboveProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(BelowProperty) == value) obj.SetValue(BelowProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(AlignHorizontalCenterWithProperty) == value) obj.SetValue(AlignHorizontalCenterWithProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyRightOfProperty) == value) SetEasyRightOf(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyAboveProperty) == value) SetEasyAbove(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyBelowProperty) == value) SetEasyBelow(obj, AvaloniaProperty.UnsetValue);
            }

            obj.SetValue(AlignVerticalCenterWithProperty, value);
            obj.SetValue(LeftOfProperty, value);
        }


        /////////////
        /// Above ///
        /////////////

        [ResolveByName]
        public static object GetEasyAbove(AvaloniaObject obj)
        {
            return obj.GetValue(EasyAboveProperty);
        }

        [ResolveByName]
        public static void SetEasyAbove(AvaloniaObject obj, object value)
        {
            obj.SetValue(EasyAboveProperty, value);
        }

        public static readonly AttachedProperty<object> EasyAboveProperty =
            AvaloniaProperty.RegisterAttached<RelativePanel, Layoutable, object>("EasyAbove");

        private static void OnEasyAbovePropertyChanged(AvaloniaObject obj, AvaloniaPropertyChangedEventArgs e)
        {
            object value = e.NewValue;

            if (value != AvaloniaProperty.UnsetValue)
            {
                if (obj.GetValue(RightOfProperty) == value) obj.SetValue(RightOfProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(LeftOfProperty) == value) obj.SetValue(LeftOfProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(BelowProperty) == value) obj.SetValue(BelowProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(AlignVerticalCenterWithProperty) == value) obj.SetValue(AlignVerticalCenterWithProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyRightOfProperty) == value) SetEasyRightOf(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyLeftOfProperty) == value) SetEasyLeftOf(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyBelowProperty) == value) SetEasyBelow(obj, AvaloniaProperty.UnsetValue);
            }

            obj.SetValue(AlignHorizontalCenterWithProperty, value);
            obj.SetValue(AboveProperty, value);
        }


        /////////////
        /// Below ///
        /////////////

        [ResolveByName]
        public static object GetEasyBelow(AvaloniaObject obj)
        {
            return obj.GetValue(EasyBelowProperty);
        }

        [ResolveByName]
        public static void SetEasyBelow(AvaloniaObject obj, object value)
        {
            obj.SetValue(EasyBelowProperty, value);
        }

        public static readonly AttachedProperty<object> EasyBelowProperty =
            AvaloniaProperty.RegisterAttached<RelativePanel, Layoutable, object>("EasyBelow");

        private static void OnEasyBelowPropertyChanged(AvaloniaObject obj, AvaloniaPropertyChangedEventArgs e)
        {
            object value = e.NewValue;

            if (value != AvaloniaProperty.UnsetValue)
            {
                if (obj.GetValue(RightOfProperty) == value) obj.SetValue(RightOfProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(LeftOfProperty) == value) obj.SetValue(LeftOfProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(BelowProperty) == value) obj.SetValue(BelowProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(AlignVerticalCenterWithProperty) == value) obj.SetValue(AlignVerticalCenterWithProperty, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyRightOfProperty) == value) SetEasyRightOf(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyLeftOfProperty) == value) SetEasyLeftOf(obj, AvaloniaProperty.UnsetValue);
                if (obj.GetValue(EasyAboveProperty) == value) SetEasyAbove(obj, AvaloniaProperty.UnsetValue);
            }

            obj.SetValue(AlignHorizontalCenterWithProperty, value);
            obj.SetValue(BelowProperty, value);
        }
    }
}
