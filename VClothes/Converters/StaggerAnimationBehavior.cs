using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace VClothes.Converters;

/// <summary>
/// Attached behavior that animates each item in an ItemsControl with a staggered fade+slide effect.
/// Usage: local:StaggerAnimationBehavior.IsEnabled="True"
/// </summary>
public static class StaggerAnimationBehavior
{
    public static readonly DependencyProperty IsEnabledProperty =
        DependencyProperty.RegisterAttached("IsEnabled", typeof(bool), typeof(StaggerAnimationBehavior),
            new PropertyMetadata(false, OnIsEnabledChanged));

    public static bool GetIsEnabled(DependencyObject obj) => (bool)obj.GetValue(IsEnabledProperty);
    public static void SetIsEnabled(DependencyObject obj, bool value) => obj.SetValue(IsEnabledProperty, value);

    private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ItemsControl itemsControl && (bool)e.NewValue)
        {
            itemsControl.ItemContainerGenerator.StatusChanged += (s, args) =>
            {
                if (itemsControl.ItemContainerGenerator.Status == System.Windows.Controls.Primitives.GeneratorStatus.ContainersGenerated)
                {
                    AnimateItems(itemsControl);
                }
            };

            // Also animate when items source changes
            itemsControl.LayoutUpdated += (s, args) =>
            {
                AnimateNewItems(itemsControl);
            };
        }
    }

    private static bool _isAnimating;

    private static void AnimateItems(ItemsControl itemsControl)
    {
        if (_isAnimating) return;
        _isAnimating = true;

        var panel = FindVisualChild<Panel>(itemsControl);
        if (panel == null) { _isAnimating = false; return; }

        for (int i = 0; i < panel.Children.Count && i < 50; i++)
        {
            var child = panel.Children[i] as FrameworkElement;
            if (child == null) continue;

            child.Opacity = 0;
            child.RenderTransform = new TranslateTransform(0, 12);

            var delay = TimeSpan.FromMilliseconds(i * 30);

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(250))
            {
                BeginTime = delay,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var slideUp = new DoubleAnimation(12, 0, TimeSpan.FromMilliseconds(250))
            {
                BeginTime = delay,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            child.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            ((TranslateTransform)child.RenderTransform).BeginAnimation(TranslateTransform.YProperty, slideUp);
        }

        // Reset flag after animations complete
        var timer = new System.Windows.Threading.DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(600)
        };
        timer.Tick += (s, e) => { _isAnimating = false; timer.Stop(); };
        timer.Start();
    }

    private static readonly HashSet<int> _animatedHashes = new();

    private static void AnimateNewItems(ItemsControl itemsControl)
    {
        var panel = FindVisualChild<Panel>(itemsControl);
        if (panel == null) return;

        for (int i = 0; i < panel.Children.Count; i++)
        {
            var child = panel.Children[i] as FrameworkElement;
            if (child == null) continue;

            var hash = child.GetHashCode();
            if (_animatedHashes.Contains(hash)) continue;
            _animatedHashes.Add(hash);

            child.Opacity = 0;
            child.RenderTransform = new TranslateTransform(0, 10);

            var delay = TimeSpan.FromMilliseconds(i * 25);

            var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(200))
            {
                BeginTime = delay,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var slideUp = new DoubleAnimation(10, 0, TimeSpan.FromMilliseconds(200))
            {
                BeginTime = delay,
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            child.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            ((TranslateTransform)child.RenderTransform).BeginAnimation(TranslateTransform.YProperty, slideUp);
        }

        // Cleanup old hashes periodically
        if (_animatedHashes.Count > 500) _animatedHashes.Clear();
    }

    private static T? FindVisualChild<T>(DependencyObject parent) where T : DependencyObject
    {
        for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T result) return result;
            var found = FindVisualChild<T>(child);
            if (found != null) return found;
        }
        return null;
    }
}
