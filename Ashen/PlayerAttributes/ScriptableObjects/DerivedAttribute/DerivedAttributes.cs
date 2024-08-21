using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * See CharacterAttrubute
 **/
[CreateAssetMenu(fileName = nameof(DerivedAttributes), menuName = "Custom/Enums/" + nameof(DerivedAttributes) + "/Types")]
public class DerivedAttributes : A_EnumList<DerivedAttribute, DerivedAttributes>
{
    public DerivedAttribute threat;
}


