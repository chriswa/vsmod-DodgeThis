using Vintagestory.API.Common;

namespace DodgeThis {
  public class ModConfig {

    public bool DisableWelcomeMessage = false;
    public RandomSelection<double> Speeds = new RandomSelection<double>() {
      Selections = new ChanceAndResultPair<double>[] {
        new ChanceAndResultPair<double>() { Chance = 0.25, Value = 1 },
        new ChanceAndResultPair<double>() { Chance = 0.25, Value = 0.8 },
        new ChanceAndResultPair<double>() { Chance = 0.25, Value = 0.6 },
        new ChanceAndResultPair<double>() { Chance = 0.25, Value = 0.4 },
      }
    };
    public float RangeBonusMultiplier = 1.25F;

    // static helper methods
    public static ModConfig Load(ICoreAPI api) {
      var config = api.LoadModConfig<ModConfig>("DodgeThis.json");
      if (config == null) {
        config = new ModConfig();
        api.StoreModConfig(config, "DodgeThis.json");
      }
      return config;
    }
    public static void Save(ICoreAPI api, ModConfig config) {
      api.StoreModConfig(config, "DodgeThis.json");
    }
  }
}
