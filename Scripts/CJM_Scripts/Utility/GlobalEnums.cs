public class GlobalEnums
{
    public enum SetActive
    {
        Inactive,
        Active,
    }

    public enum RoomFSMstatus
    {
        Inactive,
        Active,
        DoorLock,
    }
    public enum InputKeys
    {
        RightKey,
        LeftKey,
        JumpKey,
        AttackKey,
        DashKey,
        InventoryKey,
        WorldMapKey,
        WeaponAKey,
        WeaponBKey,
        InteractKey,
        ReloadKey,
        SkillAKey,
        SkillBKey,
    }
    public enum IconObjType
    {
        Player,
        Enemy,
    }

    public enum RoomType
    {
        Normal,
        Shop,
        Start,
        Portal,
    }
    
    public enum SceneName
    {
        LoadingScene,
        MainMenu,
        Town,
        SinkHole,
        BossRoom,
    }

    public enum OptionStatus
    {
        None,
        Menu,
        OptionSet,
        KeySet,
        KeyChanging,
    }

}