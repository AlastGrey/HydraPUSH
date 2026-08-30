using HarmonyLib;

namespace HydraMenu.modules.roles
{
	internal class NoKillChecks : Module
	{
		public NoKillChecks() : base("NoKillChecks") { }

		private static NoKillChecks Instance
		{
			get { return ModuleManager.noKillChecks; }
		}

		public bool NoKillCooldown { get; set; } = false;
		public bool KillOtherImpostors { get; set; } = false;
		public bool KillGhosts { get; set; } = false;

		[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.SetKillTimer))]
		class KillTimer
		{
			static void Prefix(PlayerControl __instance, ref float time)
			{
				if(!Instance.Enabled || !Instance.NoKillCooldown || __instance != PlayerControl.LocalPlayer) return;

				time = 0.0f;
			}
		}

		[HarmonyPatch(typeof(ImpostorRole), nameof(ImpostorRole.IsValidTarget))]
		class NoImpKillChecks
		{
			static bool Prefix(NetworkedPlayerInfo target, ref bool __result)
			{
				if(!Instance.Enabled) return true;

				__result = IsValidTarget(target);
				return false;
			}
		}

		[HarmonyPatch(typeof(PhantomRole), nameof(PhantomRole.IsValidTarget))]
		class NoPhantomKillChecks
		{
			static bool Prefix(PhantomRole __instance, NetworkedPlayerInfo target, ref bool __result)
			{
				if(!Instance.Enabled) return true;

				__result = IsValidTarget(target) && !__instance.isInvisible;
				return false;
			}
		}

		private static bool IsValidTarget(NetworkedPlayerInfo target)
		{
			return target != null &&
			       target != PlayerControl.LocalPlayer.Data &&
			       !target.Disconnected &&
			       (!target.IsDead || Instance.KillGhosts) &&
			       (!RoleManager.IsImpostorRole(target.RoleType) || Instance.KillOtherImpostors);
		}

		// The CheckMurder RPC handler has checks against killing ghosts
		// so we need to directly send the MurderPlayer RPC to get around it
		[HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.CmdCheckMurder))]
		class KillBypass
		{
			static bool Prefix(PlayerControl __instance, PlayerControl target)
			{
				if(!Instance.Enabled || (!AmongUsClient.Instance.AmHost && !Utilities.IsAnticheatPresent())) return true;

				__instance.RpcMurderPlayer(target, true);
				return false;
			}
		}
	}
}