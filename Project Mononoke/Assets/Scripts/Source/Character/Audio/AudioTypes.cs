using FluentAssertions.Extensions;

namespace Source.Character.Audio
{
    public enum AudioTypes
    {
        Footstep = 0,
        UIMenuOpens = 1,
        UIItemChoose = 2,
        UIItemMenuOpens = 3,
        UIButtonClick = 4,
        UIItemBeginDrag = 5,
        UIItemEndDrag = 6,
        UIItemsSwap = 7,
        UIItemThrow = 11,
        EquipItem = 8,
        StashItem = 9,
        PickUpItem = 10,
        ShadowBallCast = 12,
        ShadowBallHit = 13,
        MonkeyHit = 14
    }
}