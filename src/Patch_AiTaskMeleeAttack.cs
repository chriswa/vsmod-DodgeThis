using System;
using System.Collections.Generic;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DodgeThis {

  public class MeleeSettingsStorage {
    public static Dictionary<AiTaskMeleeAttack, MeleeSettingsStorage> cache = new Dictionary<AiTaskMeleeAttack, MeleeSettingsStorage>();
    public int attackDurationMs;
    public int damagePlayerAtMs;
    public float minDist;
    public float minVerDist;
    public float animationSpeed;
  }

  [HarmonyPatch(typeof(AiTaskMeleeAttack))]
  [HarmonyPatch("LoadConfig")]
  public class Patch__ {
    static void Postfix(
      AiTaskMeleeAttack __instance,
      ref int ___attackDurationMs,
      ref int ___damagePlayerAtMs,
      ref float ___minDist,
      ref float ___minVerDist,
      ref AnimationMetaData ___animMeta
    ) {
      if (MeleeSettingsStorage.cache.ContainsKey(__instance)) { MeleeSettingsStorage.cache.Remove(__instance); }
      // cache the original values for this instance
      MeleeSettingsStorage.cache.Add(__instance, new MeleeSettingsStorage() {
        attackDurationMs = ___attackDurationMs,
        damagePlayerAtMs = ___damagePlayerAtMs,
        minDist = ___minDist,
        minVerDist = ___minVerDist,
        animationSpeed = ___animMeta.AnimationSpeed
      });
    }
  }

  [HarmonyPatch(typeof(AiTaskMeleeAttack))]
  [HarmonyPatch("StartExecute")]
  public class Patch_AiTaskMeleeAttack_StartExecute {
    static void Prefix(
      AiTaskMeleeAttack __instance,
      ref int ___attackDurationMs,
      ref int ___damagePlayerAtMs,
      ref float ___minDist,
      ref float ___minVerDist,
      EntityAgent ___entity,
      AnimationMetaData ___animMeta,
      IWorldAccessor ___world
    ) {
      // pull original values from cache
      MeleeSettingsStorage.cache.TryGetValue(__instance, out var originalSettings);

      // randomly select a speedFactor
      double speedFactor = DodgeThisMod.Config.Speeds.Select(__instance.rand.NextDouble());

      // adjust speed by speedFactor
      ___attackDurationMs = (int)(originalSettings.attackDurationMs * speedFactor);
      ___damagePlayerAtMs = (int)(originalSettings.damagePlayerAtMs * speedFactor);
      ___animMeta.AnimationSpeed = originalSettings.animationSpeed * (float)(1 / speedFactor);

      // boost attack range
      ___minDist = originalSettings.minDist * DodgeThisMod.Config.RangeBonusMultiplier;
      ___minVerDist = originalSettings.minVerDist * DodgeThisMod.Config.RangeBonusMultiplier;
    }
  }
  [HarmonyPatch(typeof(AiTaskMeleeAttack))]
  [HarmonyPatch("ContinueExecute")]
  public class Patch_AiTaskMeleeAttack_ContinueExecute {

    static void Postfix(
      AiTaskMeleeAttack __instance,
      ref float ___minDist,
      ref float ___minVerDist,
      ref bool __result
    ) {
      if (__result == false) {
        // pull original values from cache
        MeleeSettingsStorage.cache.TryGetValue(__instance, out var originalSettings);
        
        // reset minDist and minVerDist when the attack is complete, since they are also used to determine when to start an attack
        ___minDist = originalSettings.minDist;
        ___minVerDist = originalSettings.minVerDist;
      }
    }
  }
}