using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * See CharacterAttrubute
 **/
[CreateAssetMenu(fileName = nameof(BaseAttributes), menuName = "Custom/Enums/" + nameof(BaseAttributes) + "/Types")]
public class BaseAttributes : A_EnumList<BaseAttribute, BaseAttributes>
{}

