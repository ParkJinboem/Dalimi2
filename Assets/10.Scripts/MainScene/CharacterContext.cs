using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterContext
{
    public int SelectedIndex = -1;
    public Action<int> OnCellClicked;
}
