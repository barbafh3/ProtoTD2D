﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowProjectileController : ArcProjectileController
{

  void FixedUpdate()
  {
    var distance = Vector2.Distance(transform.localPosition, _targetPosition);
    if (distance < radius)
    {
      var effectParams = new EffectParams();
      effectParams.damage = baseDamage;
      effectParams.value = _effectValue;
      effectParams.duration = _duration;
      OnTargetReached(Effects.Slow, effectParams);
    }
  }
}
