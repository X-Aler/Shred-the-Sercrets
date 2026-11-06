# Platform integration

Use Unity menu Build > Platform > Select Yandex Games or Select CrazyGames.
Wait for script compilation and asset import, then choose Build selected platform.
Outputs go to Builds/YandexGames and Builds/CrazyGames.

The selector clears the active Unity Build Profile to avoid its embedded PlayerSettings overriding the platform symbol or HTML template. Select the platform again after activating an existing profile.

PLATFORM_CRAZYGAMES selects CrazyGames; its absence selects Yandex. SDK assemblies and JavaScript plugins use matching define constraints. The inactive SDK Resources directory is renamed to PlatformResources so Unity does not automatically include it. The selector reverses the rename when switching back; keep the resulting resource folder moves with the selected project configuration.

Game scripts depend on PlatformServices and IPlatformService. Only adapters access SDK APIs. Existing Yandex coins and upgradeLevels fields are preserved. CrazyGames uses a separate JSON record shred.progress.v1 through SDK Data. No cross-platform save transfer is performed.

Rewarded bonuses require a successful SDK callback. CrazyGames pauses time and audio during ads and restores their previous values. Interstitial requests have a 180-second minimum interval on CrazyGames. The game remains on the loading screen if SDK initialization cannot finish, preventing defaults from overwriting platform saves.

SDK updates: preserve the vendor assembly definitions, plugin define constraints and UNITY_EDITOR guards around vendor editor scripts. Verify both player builds after updating either SDK. Optional packages used by CrazyGames editor tooling may require additional assembly references when installed.
