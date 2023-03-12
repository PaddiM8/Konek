using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using DynamicData;
using DynamicData.Binding;
using Konek.Client;
using Konek.Desktop.ViewModels;
using Konek.Desktop.Views.Dialogs;
using Microsoft.Extensions.DependencyInjection;

namespace Konek.Desktop.Views.Controls.RoutineBuilder;

public partial class RoutineBuilderControl : UserControl
{
    public static readonly DirectProperty<RoutineBuilderControl, IList<Effect>> EffectsProperty =
        AvaloniaProperty.RegisterDirect<RoutineBuilderControl, IList<Effect>>(
            nameof(Effects),
            o => o.Effects,
            (o, v) => o.Effects = v);

    private IList<Effect> _effects = new AvaloniaList<Effect>();

    public IList<Effect> Effects
    {
        get => _effects;
        set
        {
            SetAndRaise(EffectsProperty, ref _effects, value);
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (value != null)
                _sourceCache.AddOrUpdate(value);
        }
    }

    private readonly ReadOnlyObservableCollection<Effect> _sortedEffects;

    public ReadOnlyObservableCollection<Effect> SortedEffects
        => _sortedEffects;

    private readonly IServiceProvider _services;
    private readonly SourceCache<Effect, TimeSpan> _sourceCache = new (x => x.StartTime);

    public RoutineBuilderControl(IServiceProvider services)
    {
        _services = services;

        _sourceCache.Connect()
            .Sort(SortExpressionComparer<Effect>.Ascending(x => x.StartTime))
            .Bind(out _sortedEffects)
            .Subscribe();

        InitializeComponent();
    }

    private async Task EditEffect(Effect effectToEdit)
    {
        var effectDialog = new EffectDialog
        {
            DataContext = ActivatorUtilities.CreateInstance<EffectViewModel>(_services, effectToEdit),
        };

        var result = await effectDialog.ShowDialog<DialogResult<Effect>>(App.MainWindow);
        if (result.Status == DialogResultStatus.Delete)
            Effects.Remove(effectToEdit);

        EffectList.Items = Effects;
        EffectBlocks.Items = Effects;
    }

    private IEnumerable<Control> FindControlChildren(ItemsControl control)
    {
        return control
            .GetLogicalChildren()
            .Select(x => x.LogicalChildren[0])
            .Cast<Control>();
    }

    private void ResetVisuals()
    {
        var rectangles = FindControlChildren(EffectBlocks);
        var listItems = FindControlChildren(EffectList);
        foreach (var (rectangle, listItem) in rectangles.Zip(listItems))
        {
            rectangle.Classes.Remove("inactive");
            listItem.Classes.Remove("hovered");
            ((Border)listItem).Background = EffectList.Background;
        }
    }

    private Control? FindHoveredControl(RoutedEventArgs pointerEventArgs)
    {
        var item = pointerEventArgs.Source;
        while (item is not ContentPresenter && item != null)
            item = item.InteractiveParent;

        if (item is not ContentPresenter presenter)
            return null;

        return presenter.GetLogicalChildren().FirstOrDefault() as Control;
    }

    private void HighlightHovered(PointerEventArgs eventArgs)
    {
        var content = FindHoveredControl(eventArgs);
        if (content == null)
        {
            ResetVisuals();
            return;
        }

        var effects = EffectBlocks.Items.Cast<Effect>();
        var blocks = FindControlChildren(EffectBlocks);
        var listItems = FindControlChildren(EffectList);
        foreach (var (effect, block, listItem) in effects.Zip(blocks, listItems))
        {
            if (effect.EffectId == content.Tag as int?)
            {
                block.Classes.Remove("inactive");
                ((Border)listItem).Background = new SolidColorBrush(Colors.LightGray);
            }
            else
            {
                block.Classes.Add("inactive");
                ((Border)listItem).Background = EffectList.Background;
            }
        }
    }

    private void EffectList_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        HighlightHovered(e);
    }

    private void EffectList_OnPointerLeave(object? sender, PointerEventArgs e)
    {
        ResetVisuals();
    }

    private void EffectBlocks_OnPointerMoved(object? sender, PointerEventArgs e)
    {
        HighlightHovered(e);
    }

    private void EffectBlocks_OnPointerLeave(object? sender, PointerEventArgs e)
    {
        ResetVisuals();
    }

    private async Task ShowDialogForPressed(PointerPressedEventArgs eventArgs)
    {
        var item = FindHoveredControl(eventArgs);
        if (item == null)
            return;

        var effectToEdit = Effects.FirstOrDefault(x => x.EffectId == (int)item.Tag!);
        if (effectToEdit != null)
            await EditEffect(effectToEdit);
    }

    private async void EffectList_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        await ShowDialogForPressed(e);
    }

    private async void EffectBlocks_OnPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        await ShowDialogForPressed(e);
    }
}