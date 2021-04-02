using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;

[assembly: ModInfo("DodgeThis")]

namespace DodgeThis {
  public class DodgeThisMod : ModSystem {

    public override void Start(ICoreAPI api) {
      var harmony = new Harmony("goxmeor.DodgeThis");
      harmony.PatchAll();
    }

    public static ModConfig Config;

    public override void StartServerSide(ICoreServerAPI sapi) {

      Config = ModConfig.Load(sapi);

      if (!Config.DisableWelcomeMessage) {
        sapi.Event.PlayerNowPlaying += (IServerPlayer player) => {
          var message = "DodgeThis is in beta testing! Please share your feedback and/or favourite config settings on the forum. (Disable this message in ModConfig/DodgeThis.json)";
          player.SendMessage(GlobalConstants.GeneralChatGroup, message, EnumChatType.OwnMessage);
        };
      }



      // load config file or write it with defaults
      // config = api.LoadModConfig<ModConfig>("DodgeThisConfig.json");
      // if (config == null) {
      //   config = new ModConfig();
      //   api.StoreModConfig(config, "DodgeThisConfig.json");
      // }

      // api.ModLoader.GetModSystem<SystemTemporalStability>()



    }
  }

}
