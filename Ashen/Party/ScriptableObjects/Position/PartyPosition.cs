using Ashen.EnumSystem;

public class PartyPosition : A_DependentEnumSO<PartyPosition, PartyPositions, PartyRow, PartyRows, PartyColumn, PartyColumns>
{
    public PartyRow partyRow { get { return firstDependency; } }
    public PartyColumn partyColumn { get { return secondDependency; } }

    public PartyPosition right;
    public PartyPosition left;
    public PartyPosition up;
    public PartyPosition down;
}
