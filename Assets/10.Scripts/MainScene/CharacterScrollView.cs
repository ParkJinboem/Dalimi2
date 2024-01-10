using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI.Extensions.EasingCore;
using UnityEngine.UI.Extensions;

public class CharacterScrollView : FancyScrollView<CharacterItemData, CharacterContext>
{
    [SerializeField] CharacterScroll scroller = default;
    [SerializeField] GameObject cellPrefab = default;
    Action<int> onSelectionChanged;

    protected override GameObject CellPrefab => cellPrefab;

    protected override void Initialize()
    {
        base.Initialize();

        Context.OnCellClicked = SelectCell;

        scroller.OnValueChanged(UpdatePosition);
        scroller.OnSelectionChanged(UpdateSelection);
    }

    void UpdateSelection(int index)
    {
        if (Context.SelectedIndex == index)
        {
            return;
        }

        Context.SelectedIndex = index;

        Refresh();
        onSelectionChanged?.Invoke(index);
    }

    public void UpdateData(IList<CharacterItemData> items)
    {
        UpdateContents(items);
        scroller.SetTotalCount(items.Count);
    }

    public void OnSelectionChanged(Action<int> callback)
    {
        onSelectionChanged = callback;
    }

    public void SelectNextCell()
    {
        SoundManager.Instance.OnClickSoundEffect();
        SelectCell(Context.SelectedIndex + 1);
    }

    public void SelectPrevCell()
    {
        SoundManager.Instance.OnClickSoundEffect();
        SelectCell(Context.SelectedIndex - 1);
    }

    public void SelectCell(int index)
    {
        if (index < 0 || index >= ItemsSource.Count || index == Context.SelectedIndex)
        {
            return;
        }

        UpdateSelection(index);
        scroller.ScrollTo(index, 0.35f, Ease.OutCubic);
    }
}

