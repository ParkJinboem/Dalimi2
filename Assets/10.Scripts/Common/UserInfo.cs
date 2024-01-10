[System.Serializable]
public class UserInfo
{
    public int clearCharacterCount;
    public int playingCharacter;
    public int playingSaveId;
    public GameStage playingStage;
    public GameStep playingStep;
    public GameStep clearStep;
    public string backGroundName;
    public bool buyAdRemove;

    //출석체크
    public AttendanceData attendanceData;
    public OptionData optionData;
}
