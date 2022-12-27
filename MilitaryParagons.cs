using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Enums;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using ModHelperData = MilitaryParagons.ModHelperData;
using BTD_Mod_Helper.Api.ModOptions;
using Il2CppAssets.Scripts.Unity.UI_New.Popups;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using System.Linq;
using UnityEngine;
using BTD_Mod_Helper.Api.Data;
using BTD_Mod_Helper.Api.Components;
using Il2CppAssets.Scripts.Models.Towers.Mods;
using Il2CppAssets.Scripts.Models;
using BTD_Mod_Helper.Api.Helpers;
using Il2Cpp;

[assembly: MelonInfo(typeof(MilitaryParagons.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MilitaryParagons
{
    public class Main : BloonsTD6Mod
    {
        public override void OnNewGameModel(GameModel gameModel, Il2CppSystem.Collections.Generic.List<ModModel> mods)
        {
            gameModel.GetParagonUpgradeForTowerId("DartlingGunner").cost = CostHelper.CostForDifficulty(Settings.DartlingParagonCost, mods);
            gameModel.GetParagonUpgradeForTowerId("HeliPilot").cost = CostHelper.CostForDifficulty(Settings.HeliParagonCost, mods);
            gameModel.GetParagonUpgradeForTowerId("MonkeySub").cost = CostHelper.CostForDifficulty(Settings.SubParagonCost, mods);
            gameModel.GetParagonUpgradeForTowerId("MortarMonkey").cost = CostHelper.CostForDifficulty(Settings.MortarParagonCost, mods);
            gameModel.GetParagonUpgradeForTowerId("SniperMonkey").cost = CostHelper.CostForDifficulty(Settings.SniperParagonCost, mods);

            foreach (var towerModel in gameModel.towers)
            {
                if (towerModel.appliedUpgrades.Contains(ModContent.UpgradeID<SniperParagon.EliteMOABCripplerUpgrade>()))
                {
                    if (Settings.SniperParagonOP == true)
                    {
                        var attackModel = towerModel.GetAttackModel();
                        towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.Cooldown = 1.0f;
                        towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.maximum = 1000000f);
                        towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.minimum = 1000000f);
                        attackModel.weapons[0].Rate = 0.000000000001f;
                        attackModel.weapons[0].projectile.GetDamageModel().damage = 99999999f;
                        attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.GetDamageModel().damage = 9999999f;
                        attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.pierce = 999999f;
                    }
                    else
                    {
                        var attackModel = towerModel.GetAttackModel();
                        towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.Cooldown = 20.0f;
                        towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.maximum = 10000f);
                        towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.minimum = 10000f);
                        attackModel.weapons[0].Rate = 0.02f;
                        attackModel.weapons[0].projectile.GetDamageModel().damage = 40.0f;
                        attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.GetDamageModel().damage = 10.0f;
                        attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.pierce = 100.0f;
                    }
                }
                if (towerModel.appliedUpgrades.Contains(ModContent.UpgradeID<DartlingParagon.RayOfMadUpgrade>()))
                {
                    if (Settings.DartlingParagonOP == true)
                    {
                        var attackModel = towerModel.GetAttackModel();
                        attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 99999999f);
                        towerModel.GetAbilities().ForEach(ability => ability.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.01f));
                    }
                    else
                    {
                        var attackModel = towerModel.GetAttackModel();
                        attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 1.0f);
                        towerModel.GetAbilities().ForEach(ability => ability.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.05f));
                    }
                }
                if (towerModel.appliedUpgrades.Contains(ModContent.UpgradeID<MortarParagon.BlooncinerationAndAweUpgrade>()))
                {
                    if (Settings.MortarParagonOP == true)
                    {
                        var attackModel = towerModel.GetAttackModel();
                        attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 999999f);
                        foreach (var create in Game.instance.model.GetTowerFromId("MortarMonkey-205").Duplicate().GetDescendants<CreateProjectileOnExhaustFractionModel>().ToIl2CppList())
                        {
                            var proj = create.Duplicate();
                            proj.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 999999f);
                            attackModel.weapons[0].projectile.AddBehavior(proj);
                        }
                    }
                    else
                    {
                        var attackModel = towerModel.GetAttackModel();
                        attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 10.0f);
                        foreach (var create in Game.instance.model.GetTowerFromId("MortarMonkey-205").Duplicate().GetDescendants<CreateProjectileOnExhaustFractionModel>().ToIl2CppList())
                        {
                            var proj = create.Duplicate();
                            proj.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 7f);
                            attackModel.weapons[0].projectile.AddBehavior(proj);
                        }
                    }
                }
                if (towerModel.appliedUpgrades.Contains(ModContent.UpgradeID<HeliParagon.ApacheCommanderUpgrade>()))
                {
                    if (Settings.HeliParagonOP == true)
                    {
                       towerModel.GetDescendants<EmissionWithOffsetsModel>().ForEach(emission => emission.projectileCount = 10);
                       towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 9999999f);
                       towerModel.GetBehavior<WeaponModel>().rate *= 0.00000001f;
                    }
                    else
                    {
                        towerModel.GetDescendants<EmissionWithOffsetsModel>().ForEach(emission => emission.projectileCount = 3);
                        towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 3.0f);
                        towerModel.GetBehavior<WeaponModel>().rate *= 0.33f;
                    }
                }
                if (towerModel.appliedUpgrades.Contains(ModContent.UpgradeID<SubParagon.FirstStrikeCommanderUpgrade>()))
                {
                    if (Settings.SubParagonOP == true)
                    {
                        var attackModel = towerModel.GetAttackModel();
                        attackModel.weapons[0].emission.Cast<EmissionWithOffsetsModel>().projectileCount = 10;
                        attackModel.weapons[0].projectile.pierce = 9999999999f;
                        attackModel.weapons[0].projectile.GetDamageModel().damage = 999999f;
                        attackModel.weapons[0].Rate = 0.000000001f;
                        var attackModel2 = towerModel.GetAttackModel(1);
                        attackModel2.weapons[0].Rate = 0.000000001f;
                        attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 999999f;
                    }
                    else
                    {
                        var attackModel = towerModel.GetAttackModel();
                        attackModel.weapons[0].emission.Cast<EmissionWithOffsetsModel>().projectileCount = 4;
                        attackModel.weapons[0].projectile.pierce = 125.0f;
                        attackModel.weapons[0].projectile.GetDamageModel().damage = 150.0f;
                        attackModel.weapons[0].Rate = 0.15f;
                        var attackModel2 = towerModel.GetAttackModel(1);
                        attackModel2.weapons[0].Rate = 0.05f;
                        attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 1000.0f;
                    }
                }
            }
        }
        public override void OnTitleScreen()
        {
            if (Settings.HeliParagonOP == true || Settings.DartlingParagonOP == true || Settings.SniperParagonOP == true || Settings.MortarParagonOP == true || Settings.SubParagonOP == true )
            {
                if (Settings.TogglePopup == true)
                {
                    PopupScreen.instance.ShowOkPopup("OP military paragon's have been loaded, check the console to see which ones are on.");
                }
                if (Settings.DartlingParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "///////////////////////////////////////////////////////////////");
                    if (Settings.SniperParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!         ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the balanced version!   ///");
                    }
                    if (Settings.MortarParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!         ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the balanced version!   ///");
                    }
                    if (Settings.SubParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the OP version!             ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version!       ///");
                    }
                    if (Settings.HeliParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the OP version!            ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the balanced version!      ///");
                    }
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the balanced version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "///////////////////////////////////////////////////////////////");
                }
                else if (Settings.SniperParagonOP == false || Settings.MortarParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////////");
                    if (Settings.SniperParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!       ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the balanced version! ///");
                    }
                    if (Settings.MortarParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!       ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the balanced version! ///");
                    }
                    if (Settings.SubParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the OP version!           ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version!     ///");
                    }
                    if (Settings.HeliParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the OP version!          ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the balanced version!    ///");
                    }
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the OP version!     ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////////");
                }
                else if (Settings.HeliParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "//////////////////////////////////////////////////////////");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!    ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!    ///");
                    if (Settings.SubParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the OP version!        ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version!  ///");
                    }
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the balanced version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the OP version!  ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "//////////////////////////////////////////////////////////");
                }
                else if (Settings.SubParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!   ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!   ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the OP version!      ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the OP version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////");
                }
                else
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "////////////////////////////////////////////////////");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// All military paragons are on OP the version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "////////////////////////////////////////////////////");
                }
                MelonLogger.Msg(System.ConsoleColor.Red, "This chart is made on startup, this is not accurate if you change the settings, click reload start screen message in settings to reload this.");
            }
            else
            {
                if (Settings.TogglePopup == true)
                {
                    PopupScreen.instance.ShowOkPopup("All military paragons have been set to balanced, if you would like to change this, check the mod settings.");
                }
            }
        }
        public override void OnApplicationStart()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "Military Paragons Loaded!");
        }
        public class DartlingParagon
        {
            public class RayOfMad : ModVanillaParagon
            {
                public override string BaseTower => "DartlingGunner-250";
                public override string Name => "DartlingGunner";
            }
            public class RayOfMadUpgrade : ModParagonUpgrade<RayOfMad>
            {
                public override int Cost => 1700000;
                public override string Description => "A machine so powerful not even Dr Monkey could make it at full power. The explosive MAD bullets had to be less powerful for it to even be possible to make.";
                public override string DisplayName => "Ray Of MAD";

                public override string Icon => "RayOfMAD_Icon";
                public override string Portrait => "RayOfMAD_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    attackModel.GetDescendants<DamageModifierForTagModel>().ForEach(damage => damage.damageMultiplier = 0.25f);
                    attackModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.005f);
                    attackModel.GetDescendants<ProjectileModel>().ForEach(proj => proj.ApplyDisplay<DartlingGunnerParagonDisplayProj>());
                    towerModel.GetAbilities().ForEach(ability => ability.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.05f));
                    towerModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("EPASEM")));
                    attackModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-025").GetWeapon().projectile.GetBehavior<KnockbackModel>().Duplicate()));
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                }
            }
            public class RayOfMadDisplay : ModTowerDisplay<RayOfMad>
            {
                public override string BaseDisplay => GetDisplay(TowerType.DartlingGunner, 2, 5, 0);
                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshOutlineColor(node, new Color(135f / 255, 7f / 255, 7f / 255));
                    SetMeshTexture(node, "RayOfMAD_Display");
                    //node.GetMeshRenderer().material.mainTexture = GetTexture("FirstStrikeCommander_Display");
                }
            }
            public class DartlingGunnerParagonDisplayProj : ModDisplay
            {
                public override string BaseDisplay => "17d97a491cfa0154095f42ec1c5dae2d";
                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshTexture(node, "RayOfMADProj_Display");
                    //node.SaveMeshTexture();

                }
            }
        }
        public class SniperParagon
        {
            public class EliteMOABCrippler : ModVanillaParagon
            {
                public override string BaseTower => "SniperMonkey-025";
                public override string Name => "SniperMonkey";
            }
            public class EliteMOABCripplerUpgrade : ModParagonUpgrade<EliteMOABCrippler>
            {
                public override int Cost => 650000;
                //public override int Cost => 0;
                public override string Description => "A fast firing, smart, MOAB crippling rifle can deal with almost anything.";
                public override string DisplayName => "Elite MOAB Crippler";

                public override string Icon => "EliteMOABCrippler_Icon";
                public override string Portrait => "EliteMOABCrippler_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    attackModel.weapons[0].Rate = 0.02f;
                    attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    attackModel.weapons[0].projectile.GetDamageModel().damage = 40.0f;
                    attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.GetDamageModel().damage = 10.0f;
                    attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.pierce = 100.0f;
                    attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    attackModel.weapons[0].projectile.AddBehavior(Game.instance.model.GetTowerFromId("SniperMonkey-500").GetWeapon().projectile.GetBehavior<SlowMaimMoabModel>().Duplicate());
                    towerModel.AddBehavior(new ActivateAbilityOnRoundStartModel("AAORSM", Game.instance.model.GetTowerFromId("SniperMonkey-250").GetAbility().Duplicate()));
                    towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.Cooldown = 20.0f;
                    towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.resetCooldownOnTierUpgrade = false;
                    towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.maximum = 10000f);
                    towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel.GetDescendants<CashModel>().ForEach(cash => cash.minimum = 10000f);
                    towerModel.AddBehavior(towerModel.GetBehavior<ActivateAbilityOnRoundStartModel>().abilityModel);
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                }
            }
            public class EliteMOABCripplerDisplay : ModTowerDisplay<EliteMOABCrippler>
            {
                public override string BaseDisplay => GetDisplay(TowerType.SniperMonkey, 0, 2, 5);

                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshOutlineColor(node, new Color(135f / 255, 7f / 255, 7f / 255));
                    SetMeshTexture(node, "EliteMOABCrippler_Display");
                }
            }
        }
        public class SubParagon
        {
            public class FirstStrikeCommander : ModVanillaParagon
            {
                public override string BaseTower => "MonkeySub-205";
                public override string Name => "MonkeySub";
            }
            public class FirstStrikeCommanderUpgrade : ModParagonUpgrade<FirstStrikeCommander>
            {
                public override int Cost => 950000;
                public override string Description => "A submarine that fires 20 deadly missiles every second.What could go wrong?";
                public override string DisplayName => "First Strike Commander";

                public override string Icon => "FirstStrikeCommander_Icon";
                public override string Portrait => "FirstStrikeCommander_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    attackModel.weapons[0].emission.Cast<EmissionWithOffsetsModel>().projectileCount = 4;
                    attackModel.weapons[0].projectile.pierce = 125.0f;
                    attackModel.weapons[0].projectile.GetDamageModel().damage = 150.0f;
                    attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);
                    attackModel.weapons[0].Rate = 0.15f;
                    towerModel.range *= 1.75f;
                    attackModel.range *= 1.75f;
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeySub-050").GetAttackModel(1).Duplicate());
                    var attackModel2 = towerModel.GetAttackModel(1);
                    attackModel2.weapons[0].Rate = 0.05f;
                    attackModel2.range = 1000.0f;
                    attackModel2.weapons[0].projectile.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 1000.0f;
                    attackModel2.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("MonkeySub-050").GetBehavior<PreEmptiveStrikeLauncherModel>().Duplicate());
                    var submergeEffect = Game.instance.model.GetTowerFromId("MonkeySub-502").Duplicate().GetBehavior<SubmergeEffectModel>().effectModel;
                    var submerge = Game.instance.model.GetTowerFromId("MonkeySub-502").Duplicate().GetBehavior<SubmergeModel>();
                    towerModel.AddBehavior(new HeroXpScaleSupportModel("HeroXpScaleSupportModel_", true, submerge.heroXpScale, null));
                    towerModel.AddBehavior(new AbilityCooldownScaleSupportModel("AbilityCooldownScaleSupportModel_", true, submerge.abilityCooldownSpeedScale, true, false, null, submerge.buffLocsName, submerge.buffIconName, false, submerge.supportMutatorPriority));
                    foreach (var attackModels in towerModel.GetAttackModels())
                    {
                        if (attackModels.name.Contains("Submerge"))
                        {
                            attackModels.name = attackModel.name.Replace("Submerged", "");
                            attackModels.weapons[0].GetBehavior<EjectEffectModel>().effectModel.assetId = submerge.attackDisplayPath;
                        }

                        attackModel.RemoveBehavior<SubmergedTargetModel>();
                    }
                    towerModel.AddBehavior(new CreateEffectAfterTimeModel("CreateEffectAfterTimeModel_", submergeEffect, 0f, true));
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                }
            }
            public class FirstStrikeCommanderDisplay : ModTowerDisplay<FirstStrikeCommander>
            {
                public override string BaseDisplay => GetDisplay(TowerType.MonkeySub, 2, 5, 0);

                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshOutlineColor(node, new Color(18f / 255, 163f / 255, 199f / 255));
                    SetMeshTexture(node, "FirstStrikeCommander_Display");
                }
            }
        }
        public class MortarParagon
        {
            public class BlooncinerationAndAwe : ModVanillaParagon
            {
                public override string BaseTower => "MortarMonkey-250";
                public override string Name => "MortarMonkey";
            }
            public class BlooncinerationAndAweUpgrade : ModParagonUpgrade<BlooncinerationAndAwe>
            {
                public override int Cost => 550000;
                public override string Description => "Did you know if you combine huge damage, fast firing, and deadly flames, you get a super powerful monkey?";
                public override string DisplayName => "Blooncineration And Awe";

                public override string Icon => "BlooncinerationAwe_Icon";
                public override string Portrait => "BlooncinerationAwe_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("MortarMonkey-520").GetAttackModel().weapons[0].projectile.Duplicate();
                    attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 10.0f);
                    foreach (var create in Game.instance.model.GetTowerFromId("MortarMonkey-205").Duplicate().GetDescendants<CreateProjectileOnExhaustFractionModel>().ToIl2CppList())
                    {
                        var proj = create.Duplicate();
                        proj.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 7f);
                        attackModel.weapons[0].projectile.AddBehavior(proj);
                    }
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                }
            }
            public class BlooncinerationAndAweDisplay : ModTowerDisplay<BlooncinerationAndAwe>
            {
                public override string BaseDisplay => GetDisplay(TowerType.MortarMonkey, 2, 5, 0);

                public override bool UseForTower(int[] tiers)
                {
                    return IsParagon(tiers);
                }

                public override int ParagonDisplayIndex => 0;

                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshOutlineColor(node, new Color(17f / 255, 121f / 255, 83f / 255));
                    SetMeshTexture(node, "BlooncinerationAwe_Display");
                }
            }
        }
        public class HeliParagon
        {
            public class ApacheCommander : ModVanillaParagon
            {
                public override string BaseTower => "HeliPilot-502";
                public override string Name => "HeliPilot";
            }
            public class ApacheCommanderUpgrade : ModParagonUpgrade<ApacheCommander>
            {
                public override int Cost => 550000;
                public override string Description => "What's stronger than one Apache Prime? Multiple!";
                public override string DisplayName => "Apache Commander";

                public override string Icon => "ApacheCommander_Icon";
                public override string Portrait => "ApacheCommander_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    towerModel.doesntRotate = true;
                    towerModel.GetBehavior<DisplayModel>().ignoreRotation = true;
                    TowerModel backup = Game.instance.model.GetTowerFromId("HeliPilot-502").Duplicate();
                    var attackModel = towerModel.GetAttackModel();
                    towerModel.GetBehavior<AirUnitModel>().display = ModContent.CreatePrefabReference<ApacheCommanderAirUnitDisplay>();
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<CollectCashZoneModel>().Duplicate());
                    towerModel.GetBehavior<CollectCashZoneModel>().collectRange = 9999999;
                    towerModel.GetBehavior<CollectCashZoneModel>().attractRange = 9999999;
                    towerModel.GetBehavior<CollectCashZoneModel>().speed = 1;
                    towerModel.GetDescendants<EmissionWithOffsetsModel>().ForEach(emission => emission.projectileCount = 3);
                    towerModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate *= 0.33f);
                    towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 3.0f);
                    towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities().Last().Duplicate());
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities()[0].Duplicate());
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities()[1].GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());
                    towerModel.GetAttackModels().Last().GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 30.0f);
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities()[1].GetBehavior<ActivateAttackModel>().attacks[1].Duplicate());
                    towerModel.GetAttackModels().Last().GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 50.0f);
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("EngineerMonkey-200").GetAttackModel().Duplicate());
                    var attackModel2 = towerModel.GetAttackModels().Last();
                    var createTower = attackModel2.weapons[0].projectile.GetBehavior<CreateTowerModel>();
                    createTower.tower = backup;
                    createTower.tower.cost = 0.0f;
                    createTower.tower.radius = 0.0f;
                    createTower.tower.isSubTower = true;
                    createTower.tower.display = new() { guidRef = "" };
                    createTower.tower.GetBehavior<AirUnitModel>().display = ModContent.CreatePrefabReference<ApacheCommanderAirUnitSmallDisplay>();
                    createTower.tower.RemoveBehavior<CreateSoundOnTowerPlaceModel>();
                    createTower.tower.RemoveBehavior<CreateSoundOnSellModel>();
                    createTower.tower.name += "ApacheCommander_Apache";
                    createTower.tower.AddBehavior(Game.instance.model.GetTowerFromId("Sentry").GetBehavior<TowerExpireModel>().Duplicate());
                    createTower.tower.GetBehavior<TowerExpireModel>().Lifespan = 60f;
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                    createTower.tower.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    createTower.tower.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                }
                public class ApacheCommanderDisplay : ModTowerDisplay<ApacheCommander>
                {
                    public override string BaseDisplay => "6cbc51704a6befc40a2fe05a4c7a41b5";

                    public override bool UseForTower(int[] tiers)
                    {
                        return IsParagon(tiers);
                    }

                    public override int ParagonDisplayIndex => 0;

                    public override void ModifyDisplayNode(UnityDisplayNode node)
                    {

                    }
                }
                public class ApacheCommanderAirUnitDisplay : ModDisplay
                {
                    public override string BaseDisplay => "6ff55b62a3a5bb7459ceb731804aa039";
                    public override void ModifyDisplayNode(UnityDisplayNode node)
                    {
                        foreach (var renderer in node.genericRenderers)
                        {
                            renderer.material.mainTexture = GetTexture("ApacheCommander_Display");
                        }
                    }
                }
                public class ApacheCommanderAirUnitSmallDisplay : ModDisplay
                {
                    public override string BaseDisplay => "5643bdec5ee8b214b86651b78a0af9d1";
                    public override void ModifyDisplayNode(UnityDisplayNode node)
                    {
                        foreach (var renderer in node.genericRenderers)
                        {
                            renderer.material.mainTexture = GetTexture("ApacheCommander_Display");
                        }
                    }
                }
            }
        }
    }
    public class Settings : ModSettings
    {
        public static void OPConsoleList()
        {
            if (Settings.HeliParagonOP == true || Settings.DartlingParagonOP == true || Settings.SniperParagonOP == true || Settings.MortarParagonOP == true || Settings.SubParagonOP == true)
            {
                if (Settings.TogglePopup == true)
                {
                    PopupScreen.instance.ShowOkPopup("OP military paragon's have been loaded, check the console to see which ones are on.");
                }
                if (Settings.DartlingParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "///////////////////////////////////////////////////////////////");
                    if (Settings.SniperParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!         ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the balanced version!   ///");
                    }
                    if (Settings.MortarParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!         ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the balanced version!   ///");
                    }
                    if (Settings.SubParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the OP version!             ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version!       ///");
                    }
                    if (Settings.HeliParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the OP version!            ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the balanced version!      ///");
                    }
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the balanced version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "///////////////////////////////////////////////////////////////");
                }
                else if (Settings.SniperParagonOP == false || Settings.MortarParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////////");
                    if (Settings.SniperParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!       ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the balanced version! ///");
                    }
                    if (Settings.MortarParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!       ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the balanced version! ///");
                    }
                    if (Settings.SubParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the OP version!           ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version!     ///");
                    }
                    if (Settings.HeliParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the OP version!          ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the balanced version!    ///");
                    }
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the OP version!     ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////////");
                }
                else if (Settings.HeliParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "//////////////////////////////////////////////////////////");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!    ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!    ///");
                    if (Settings.SubParagonOP == true)
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the OP version!        ///");
                    }
                    else
                    {
                        MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version!  ///");
                    }
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the balanced version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the OP version!  ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "//////////////////////////////////////////////////////////");
                }
                else if (Settings.SubParagonOP == false)
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The sniper monkey paragon is on the OP version!   ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The mortar monkey paragon is on the OP version!   ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The submarine paragon is on the balanced version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The heli pilot paragon is on the OP version!      ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// The dartling gunner paragon is on the OP version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/////////////////////////////////////////////////////////");
                }
                else
                {
                    MelonLogger.Msg(System.ConsoleColor.Green, "////////////////////////////////////////////////////");
                    MelonLogger.Msg(System.ConsoleColor.Green, "/// All military paragons are on OP the version! ///");
                    MelonLogger.Msg(System.ConsoleColor.Green, "////////////////////////////////////////////////////");
                }
                MelonLogger.Msg(System.ConsoleColor.Red, "This chart is made on startup, this is not accurate if you change the settings, click reload start screen message in settings to reload this.");
            }
            else
            {
                if (Settings.TogglePopup == true)
                {
                    PopupScreen.instance.ShowOkPopup("All military paragons have been set to balanced, if you would like to change this, check the mod settings.");
                }
            }
        }

        private static readonly ModSettingCategory OPParagons = new ("Toggle OP Mode of Paragons")
        {
            modifyCategory = category =>
            {

            }
        };

        public static readonly ModSettingBool SniperParagonOP = new(false)
        {
            displayName = "OP Mode of Elite MOAB Crippler",
            button = true,
            category = OPParagons,
            icon = GetTextureGUID<Main>("EliteMOABCrippler_Portrait")
        };
        public static readonly ModSettingBool MortarParagonOP = new(false)
        {
            displayName = "OP Mode of Blooncineration and Awe",
            button = true,
            category = OPParagons,
            icon = GetTextureGUID<Main>("BlooncinerationAwe_Portrait")
        };
        public static readonly ModSettingBool SubParagonOP = new(false)
        {
            displayName = "OP Mode of First Strike Commander",
            button = true,
            category = OPParagons,
            icon = GetTextureGUID<Main>("FirstStrikeCommander_Portrait")
        };
        public static readonly ModSettingBool HeliParagonOP = new(false)
        {
            displayName = "OP Mode of Apache Commander",
            button = true,
            category = OPParagons,
            icon = GetTextureGUID<Main>("ApacheCommander_Portrait")
        };
        public static readonly ModSettingBool DartlingParagonOP = new(false)
        {
            displayName = "OP Mode of Ray of MAD",
            button = true,
            category = OPParagons,
            icon = GetTextureGUID<Main>("RayOfMAD_Portrait")
        };

        private static readonly ModSettingCategory ParagonCost = new("Paragon Costs")
        {
            modifyCategory = category =>
            {

            }
        };

        public static readonly ModSettingInt SniperParagonCost = new(650000)
        {
            displayName = "Elite MOAB Crippler Cost",
            category = ParagonCost,
            icon = GetTextureGUID<Main>("EliteMOABCrippler_Portrait")
        };
        public static readonly ModSettingInt MortarParagonCost = new(550000)
        {
            displayName = "Blooncineration and Awe Cost",
            category = ParagonCost,
            icon = GetTextureGUID<Main>("BlooncinerationAwe_Portrait")
        };
        public static readonly ModSettingInt SubParagonCost = new(950000)
        {
            displayName = "First Strike Commander Cost",
            category = ParagonCost,
            icon = GetTextureGUID<Main>("FirstStrikeCommander_Portrait")
        };
        public static readonly ModSettingInt HeliParagonCost = new(550000)
        {
            displayName = "Apache Commander Cost",
            category = ParagonCost,
            icon = GetTextureGUID<Main>("ApacheCommander_Portrait")
        };
        public static readonly ModSettingInt DartlingParagonCost = new(1700000)
        {
            displayName = "Ray of MAD Cost",
            category = ParagonCost,
            icon = GetTextureGUID<Main>("RayOfMAD_Portrait")
        };

        public static readonly ModSettingButton PushMe = new(() => OPConsoleList())
        {
            displayName = "Reload Start Screen Message",
            buttonText = "Reload",
            buttonSprite = VanillaSprites.YellowBtnLong
        };
        public static readonly ModSettingBool TogglePopup = new(true)
        {
            displayName = "Toggle Popup",
            description = "Toggles the popup on main menu that tells you your paragon versions (OP/Balanced)",
            button = true,
        };
    }
}