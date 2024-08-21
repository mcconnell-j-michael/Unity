public class PartyRow : A_EnumSO<PartyRow, PartyRows>
{
    public PartyRow forward;
    public PartyRow backward;

    public int rangePenalty;

    public int enemySortingOrder;
    public int enemyAnimationSortingOrder;
}
