using TMPro;

public class BattleLogUIManager : SingletonMonoBehaviour<BattleLogUIManager>, I_LogMessageChangedListener
{
    public TextMeshProUGUI log;
    public TextMeshProUGUI turnValue;
    public TextMeshProUGUI time;

    private void Start()
    {
        CombatLog.Instance.RegisterListener(this);
        string lastMessage = CombatLog.Instance.GetLastMessage();
        log.text = lastMessage ?? "";
    }

    public void OnLogMessageChanged()
    {
        string lastMessage = CombatLog.Instance.GetLastMessage();
        string pastMessage = CombatLog.Instance.GetMessage(-2);
        string message = (pastMessage ?? "") + "\n" + (lastMessage ?? "");
        log.text = message ?? "";
    }

    public void UpdateTurn(int turnValue)
    {
        this.turnValue.text = turnValue.ToString();
    }
}
