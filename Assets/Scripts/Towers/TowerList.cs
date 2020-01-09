public enum Towers
{
  ArrowTower,
  CannonTower,
  SlowTower
}

public enum TowerButtons
{
  ArrowTowerButton,
  CannonTowerButton,
  SlowTower
}

public enum Effects
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