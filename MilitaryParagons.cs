using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using MelonLoader;
using ModHelperData = MilitaryParagons.ModHelperData;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Projectiles;
using Il2CppAssets.Scripts.Models.Towers.Weapons;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using System.Linq;
using UnityEngine;
using Il2Cpp;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;

[assembly: MelonInfo(typeof(MilitaryParagons.Main), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]

namespace MilitaryParagons
{
    public class Main : BloonsTD6Mod
    {
        public override void OnApplicationStart()
        {
            MelonLogger.Msg(System.ConsoleColor.Green, "Military Paragons Loaded!");
        }
        public class DartlingParagon
        {
            public class RayOfMad : ModVanillaParagon
            {
                public override string BaseTower => "DartlingGunner-250";
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
                    attackModel.GetDescendants<DamageModifierForTagModel>().ForEach(damage => damage.damageMultiplier = 0.5f);
                    attackModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 0.002f);
                    attackModel.GetDescendants<ProjectileModel>().ForEach(proj => proj.ApplyDisplay<DartlingGunnerParagonDisplayProj>());
                    towerModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("EPASEM")));
                    attackModel.GetDescendants<ProjectileModel>().ForEach(projectile => projectile.AddBehavior(Game.instance.model.GetTowerFromId("DartlingGunner-025").GetWeapon().projectile.GetBehavior<KnockbackModel>().Duplicate()));
                    towerModel.AddBehavior(new OverrideCamoDetectionModel("OverrideCamoDetectionModel_", true));
                    towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model2 => model2.isActive = false);
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<RotateToTargetModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetFirstPrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetLastPrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetClosePrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetStrongPrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().RemoveBehavior<TargetPointerModel>();
                    towerModel.GetBehavior<AttackModel>().RemoveBehavior<TargetSelectedPointModel>();
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
                }
            }
            public class DartlingGunnerParagonDisplayProj : ModDisplay
            {
                public override string BaseDisplay => "17d97a491cfa0154095f42ec1c5dae2d";
                public override void ModifyDisplayNode(UnityDisplayNode node)
                {
                    SetMeshTexture(node, "RayOfMADProj_Display");

                }
            }
        }
        public class SniperParagon
        {
            public class EliteMOABCrippler : ModVanillaParagon
            {
                public override string BaseTower => "SniperMonkey-025";
            }
            public class EliteMOABCripplerUpgrade : ModParagonUpgrade<EliteMOABCrippler>
            {
                public override int Cost => 1000000;
                public override string Description => "A fast firing, smart, MOAB crippling rifle can deal with almost anything.";
                public override string DisplayName => "Elite MOAB Crippler";

                public override string Icon => "EliteMOABCrippler_Icon";
                public override string Portrait => "EliteMOABCrippler_Portrait";
                public override void ApplyUpgrade(TowerModel towerModel)
                {
                    var attackModel = towerModel.GetAttackModel();
                    attackModel.weapons[0].Rate = 0.01f;
                    attackModel.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                    attackModel.weapons[0].projectile.GetDamageModel().damage = 250.0f;
                    attackModel.weapons[0].projectile.GetBehavior<EmitOnDamageModel>().projectile.GetDamageModel().damage = 200.0f;
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
                    towerModel.AddBehavior(new HeroXpScaleSupportModel("HeroXpScaleSupportModel_", true, submerge.heroXpScale, null, towerModel.icon.guidRef, towerModel.icon.guidRef));
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
                    towerModel.RemoveBehavior<RemoveMutatorOnUpgradeModel>();
                    var attackModel = towerModel.GetAttackModel();
                    attackModel.weapons[0].projectile = Game.instance.model.GetTowerFromId("MortarMonkey-520").GetAttackModel().weapons[0].projectile.Duplicate();
                    attackModel.weapons[0].projectile.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 10.0f);
                    foreach (var create in Game.instance.model.GetTowerFromId("MortarMonkey-205").Duplicate().GetDescendants<CreateProjectileOnExhaustFractionModel>().ToList())
                    {
                        var proj = create.Duplicate();
                        proj.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 5f);
                        attackModel.weapons[0].projectile.AddBehavior(proj);
                    }
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<RotateToTargetModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetFirstPrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetLastPrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetClosePrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().AddBehavior(Game.instance.model.GetTowerFromId("DartMonkey").GetDescendant<TargetStrongPrioCamoModel>().Duplicate());
                    towerModel.GetBehavior<AttackModel>().RemoveBehavior<TargetSelectedPointModel>();
                    towerModel.towerSelectionMenuThemeId = "Default";
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
                    towerModel.GetBehavior<AirUnitModel>().display = CreatePrefabReference<ApacheCommanderAirUnitDisplay>();
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("BananaFarm-005").GetBehavior<CollectCashZoneModel>().Duplicate());
                    towerModel.GetBehavior<CollectCashZoneModel>().collectRange = 9999999;
                    towerModel.GetBehavior<CollectCashZoneModel>().attractRange = 9999999;
                    towerModel.GetBehavior<CollectCashZoneModel>().speed = 1;
                    towerModel.GetDescendants<EmissionWithOffsetsModel>().ForEach(emission => emission.projectileCount = 3);
                    towerModel.GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate *= 0.33f);
                    towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.damage *= 50.0f);
                    towerModel.GetDescendants<DamageModel>().ForEach(damage => damage.immuneBloonProperties = BloonProperties.None);
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities().Last().Duplicate());
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities()[0].Duplicate());
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities()[1].GetBehavior<ActivateAttackModel>().attacks[0].Duplicate());
                    towerModel.GetAttackModels().Last().GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 30.0f);
                    towerModel.AddBehavior(Game.instance.model.GetTowerFromId("HeliPilot-050").GetAbilities()[1].GetBehavior<ActivateAttackModel>().attacks[1].Duplicate());
                    towerModel.GetAttackModels().Last().GetDescendants<WeaponModel>().ForEach(weapon => weapon.Rate = 50.0f);
                    foreach (var behavior in Game.instance.model.GetTowerFromId("EngineerMonkey-200").GetAttackModels())
                    {
                        if (behavior.name.Contains("Spawner"))
                        {
                            towerModel.AddBehavior(behavior.Duplicate());
                        }
                    }
                    var createTower = towerModel.GetBehaviors<AttackModel>().Last().GetDescendant<CreateTowerModel>();
                    createTower.tower = Game.instance.model.GetTowerFromId("HeliPilot-502").Duplicate();
                    createTower.tower.cost = 0.0f;
                    createTower.tower.radius = 0.0f;
                    createTower.tower.isSubTower = true;
                    createTower.tower.display = new() { guidRef = "" };
                    createTower.tower.ignoreTowerForSelection = true;
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
}