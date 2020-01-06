public enum TowerList
{
  ArrowTower,
  CannonTower,
  SlowTower
}

public enum TowerButtonList
{
  ArrowTowerButton,
  CannonTowerButton,
  SlowTower
}

public enum EffectList
{
  Slow,
  Dot,
  Damage
}

public class EffectParams
{
  public float damage = 0f;
  public float value = 0f;
  public float duration = 0f;
}