using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System.Collections;

public class CharacterScrollManager : MonoBehaviour
{
    [SerializeField] CharacterScrollView scrollView = default;
    [SerializeField] Button prevCellButton = default;
    [SerializeField] Button nextCellButton = default;
    [SerializeField] Text selectedItemInfo = default;
    
    public void Start()
    {
        prevCellButton.onClick.AddListener(scrollView.SelectPrevCell);
        nextCellButton.onClick.AddListener(scrollView.SelectNextCell);
        scrollView.OnSelectionChanged(OnSelectionChanged);

        var items = Enumerable.Range(1, Statics.clearCharacterCount + 1)
            .Select(i => new CharacterItemData($"Cell {i}", i))
            .ToArray();

        scrollView.UpdateData(items);
        scrollView.SelectCell(0);
    }

    void OnSelectionChanged(int index)
    {
        selectedItemInfo.text = $"Selected item info: index {index}";
        Statics.selectedCharacter = index + 1;

        if(Statics.selectedCharacter == 1)
        {
            prevCellButton.gameObject.SetActive(false);
            nextCellButton.gameObject.SetActive(true);
        }
        else if (Statics.selectedCharacter == Statics.clearCharacterCount + 1)
        {
            nextCellButton.gameObject.SetActive(false);
            prevCellButton.gameObject.SetActive(true);
        }
        else
        {
            prevCellButton.gameObject.SetActive(true);
            nextCellButton.gameObject.SetActive(true);
        }
    }

    public void ContentUpdate()
    {
        CharacterItemData[] items = Enumerable.Range(1, Statics.clearCharacterCount + 1)
            .Select(i => new CharacterItemData($"Cell {i}", i))
            .ToArray();

        scrollView.UpdateData(items);
    }

    public void CharacterRePosition(int characterIndex)
    {
        scrollView.SelectCell(characterIndex - 1);
    }

    public void EndCharacterRePosition()
    {
        scrollView.SelectCell(Statics.clearCharacterCount - 1);
    }
}
