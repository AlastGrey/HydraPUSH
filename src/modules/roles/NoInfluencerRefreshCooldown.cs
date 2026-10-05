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
			ActionButton button = isSecondary ? HudManager.Instance.SecondaryAbilityButton : HudManager.Instance.AbilityButton;
			button.SetCoolDown(0.0f, GameManager.Instance.LogicOptions.GetRoleFloat(FloatOptionNames.SpiritGuideCooldownSeconds));
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