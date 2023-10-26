using UnityEngine;

public enum EnemyType
{
   SmallGround,
   SmallFly,
   LargeGround,
   LargeFly,
   MiniBoss,
   StageBoss,
}
public class EnemyCreateInfo
{
    public Vector3 CreatePosition;
    public EnemyType CreateType;
}

