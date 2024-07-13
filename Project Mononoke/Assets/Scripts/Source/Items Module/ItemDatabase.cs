using System;
using System.Collections.Generic;
using System.Text;
using Base.Databases;
using UnityEngine;

namespace Source.ItemsModule
{
    public delegate void UseBehaviour(object context);

    [Serializable]
    public abstract class ItemsDatabase : DatabaseSO<ItemData>
    {
    }
}
