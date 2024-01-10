using UnityEngine;
using UnityEngine.UI.Extensions;
using UnityEngine.UI;

public class CharacterCell : FancyCell<CharacterItemData, CharacterContext>
{
    [SerializeField] Animator animator = default;
    //[SerializeField] Text message = default;
    [SerializeField] Image image = default;
    [SerializeField] Button button = default;
    [SerializeField] ScrollCharacter character;
    [SerializeField] GameObject versionAlarm;
    public int characterId = 1;

    static class AnimatorHash
    {
        public static readonly int Scroll = Animator.StringToHash("scroll");
    }

    public override void Initialize()
    {
        button.onClick.AddListener(() => Context.OnCellClicked?.Invoke(Index));
        //image.color = new Color32(0, 0, 0, 0);
    }

    public override void UpdateContent(CharacterItemData itemData)
    {
        //message.text = itemData.Message;
        character.CreateCharacter(itemData.Index);
        characterId = itemData.Index;
        if (itemData.Index == Statics.clearCharacterCount + 1)
        {
            character.BlackCharacter();
        }
        else
        {
            character.WhiteCharacter();
        }
        image.color = new Color32(0, 0, 0, 0);

        if (characterId == 28)
        {
            versionAlarm.SetActive(true);
        }
        else
        {
            versionAlarm.SetActive(false);
        }
    }

    public override void UpdatePosition(float position)
    {
        currentPosition = position;

        if (animator.isActiveAndEnabled)
        {
            animator.Play(AnimatorHash.Scroll, -1, position);
        }

        animator.speed = 0;
    }

    // GameObject が非アクティブになると Animator がリセットされてしまうため
    // 現在位置を保持しておいて OnEnable のタイミングで現在位置を再設定します
    float currentPosition = 0;

    void OnEnable() => UpdatePosition(currentPosition);


    public void ClickedCharacter()
    {
        Statics.selectedCharacter = characterId;
        MainSceneController.Instance.ShowPlayScene();
    }
}
