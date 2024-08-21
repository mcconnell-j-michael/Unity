namespace Ashen.EnumSystem
{
    public class A_DependentEnumSO<DependentEnumSO, DependentEnumList, DependsOnEnumOne, DependsOnEnumListOne, DependsOnEnumTwo, DependsOnEnumListTwo> :
        A_EnumSO<DependentEnumSO, DependentEnumList>
        where DependentEnumSO : A_DependentEnumSO<DependentEnumSO, DependentEnumList, DependsOnEnumOne, DependsOnEnumListOne, DependsOnEnumTwo, DependsOnEnumListTwo>
        where DependentEnumList : A_DependentEnumList<DependentEnumSO, DependentEnumList, DependsOnEnumOne, DependsOnEnumListOne, DependsOnEnumTwo, DependsOnEnumListTwo>
        where DependsOnEnumOne : A_EnumSO<DependsOnEnumOne, DependsOnEnumListOne>
        where DependsOnEnumListOne : A_EnumList<DependsOnEnumOne, DependsOnEnumListOne>
        where DependsOnEnumTwo : A_EnumSO<DependsOnEnumTwo, DependsOnEnumListTwo>
        where DependsOnEnumListTwo : A_EnumList<DependsOnEnumTwo, DependsOnEnumListTwo>
    {
        public DependsOnEnumOne firstDependency;
        public DependsOnEnumTwo secondDependency;
    }
}