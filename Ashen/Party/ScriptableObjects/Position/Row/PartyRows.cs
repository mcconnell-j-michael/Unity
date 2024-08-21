public class PartyRows : A_EnumList<PartyRow, PartyRows>
{
    public PartyRow First()
    {
        return EnumList[0];
    }
}
