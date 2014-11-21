﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using xSLx_Orbwalker;
using xSLx_TargetSelector;
using System.Threading.Tasks;

namespace Aggressive_Olaf
{
    class Program
    {
        public static Obj_AI_Base Player;
        public static Spell Q, W, E, R;
        public static Menu OlafKills;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += onLoad;
        }
        static void onLoad(EventArgs args)
        {
            Obj_AI_Base Player = ObjectManager.Player;
            if (Player.BaseSkinName != "Olaf") return;
            Q = new Spell(SpellSlot.Q, 1000f);
            W = new Spell(SpellSlot.W, Player.AttackRange);
            E = new Spell(SpellSlot.E, 325);
            R = new Spell(SpellSlot.R);
            Q.SetSkillshot(250, 90, 1600, false, SkillshotType.SkillshotLine);
            OlafKills = new Menu("Olaf", "Olaf", true);
            var orbwalker = new Menu("Orbwalker", "Orbwalker");
            var ts = new Menu("Targetselector", "Targetselector");
            SimpleTs.AddToMenu(ts);
            OlafKills.AddSubMenu(ts);
            xSLxOrbwalker.AddToMenu(orbwalker);
            OlafKills.AddSubMenu(orbwalker);
            OlafKills.AddItem(new MenuItem("NFE", "Packetcasting").SetValue(true));
            OlafKills.AddSubMenu(new Menu("Combo", "Combo"));
            OlafKills.AddSubMenu(new Menu("Misc", "Misc"));
            OlafKills.SubMenu("Combo").AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
            OlafKills.SubMenu("Combo").AddItem(new MenuItem("Save Mana for W", "Save Mana for W").SetValue(true));
            OlafKills.SubMenu("Misc").AddItem(new MenuItem("catchQ", "Auto Pickup Q").SetValue(true));
            OlafKills.AddToMainMenu();
            Game.OnGameUpdate += Game_OnGameUpdate;


        }
        static void Game_OnGameUpdate(EventArgs args)
        {
            if (OlafKills.Item("catchQ").GetValue<bool>())
            {
            }
            if (OlafKills.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
        }
        static void Combo()
        {
            var target = SimpleTs.GetTarget(Q.Range, SimpleTs.DamageType.Physical);
            if (target == null) return;
            if (target.Team != Player.Team)
            {
                if (target.IsValidTarget(Q.Range))
                {
                    Q.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                    Game.PrintChat("Casting Q");
                }
                if (target.IsValidTarget(E.Range)) 
                { 
                    E.Cast(target,OlafKills.Item("NFE").GetValue<bool>());
                    Game.PrintChat("Casting E");
                }
                if (target.IsValidTarget(Player.AttackRange))
                {
                    W.Cast(OlafKills.Item("NFE").GetValue<bool>());
                    R.Cast(OlafKills.Item("NFE").GetValue<bool>());
                    Game.PrintChat("Casting W");
                }
            }
        }

    }
}
