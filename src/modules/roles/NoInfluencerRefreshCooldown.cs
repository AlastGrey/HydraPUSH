using AmongUs.GameOptions;

namespace HydraMenu.modules.roles
{
	internal class NoInfluencerRefreshCooldown : Module
	{
		public NoInfluencerRefreshCooldown() : base("NoInfluencerRefreshCooldown")
		{
			base.Enabled = true;
		}

		private void OnUseRoleAbility(bool isSecondary)
		{
			if(!isSecondary && Utilities.IsAnticheatPresent()) return;

			// have to use TryCast here
			SpiritGuideRole role = PlayerControl.LocalPlayer.Data.Role.TryCast<SpiritGuideRole>();
			if(role == null) return;

			role.cooldownSecondsRemaining = 0.0f;
		    // this is realistically only for the zeroing of the message ability, which is only in modded lobbies. The refresh already do the ungrey itself for some reasons
			if(isSecondary) HudManager.Instance.SecondaryAbilityButton.SetCoolDown(0.0f, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
		}

		protected override void OnEnable()
		{
			EventCoordinator.OnUseRoleAbility += OnUseRoleAbility;
		}

		protected override void OnDisable()
		{
			EventCoordinator.OnUseRoleAbility -= OnUseRoleAbility;
		}
	}
}