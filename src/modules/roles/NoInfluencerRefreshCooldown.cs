namespace HydraMenu.modules.roles
{
	internal class NoInfluencerRefreshCooldown : Module
	{
		public NoInfluencerRefreshCooldown() : base("NoInfluencerRefreshCooldown")
		{
			base.Enabled = true;
		}

		private void OnUseRoleAbility(PlayerControl player)
		{
			// have to use TryCast here
			SpiritGuideRole role = player.Data.Role.TryCast<SpiritGuideRole>();
			if(role == null) return;

			role.cooldownSecondsRemaining = 0.0f;
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