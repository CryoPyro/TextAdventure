using System;
using System.Collections.Generic;
using System.Collections;
using TextAdventure.Classes.Characters;
using TextAdventure.Classes.Input;

namespace TextAdventure.Classes.Events;


public class CombatEvent : Event
{
    public CombatEvent(string name, List<Enemy> enemies, Event reward) : base(name, "")
    {
        Description = "You encountered " + string.Join(", ", enemies.Select(enemy => enemy.Name));
        var lastComma = Description.LastIndexOf(',');
        if (lastComma != -1) // if there is more than 1 enemy, have ' and ' before the last enemy
            Description = Description.Remove(lastComma, 1).Insert(lastComma, " and ");

        Enemies = enemies;
        Reward = reward;
    }

    public List<Enemy> Enemies;
    public int TurnCounter = 0;
    public Event Reward;

    public override Event? Update(ref Player player, Room[] world)
    {
        Description = "";
        TurnCounter++;
        if (TurnCounter % 2 == 1)
        {
            if (player.TakeTurn(this))
            {
                TurnCounter = 0;
                return null;
            }

            Enemies.RemoveAll(enemy => enemy.Health <= 0);
            if (Enemies.Count == 0)
            {
                RemoveEventFromWorld(world);
                InputHelper.DisplayAndWait("You won...");
                return Reward;
            }
            return this;
        }

        var random = new Random();

        Enemy? enemy;
        // If any enemy is charging select that one, else a random one
        enemy = Enemies.Find(enemy => enemy.IsChargingHeavy);
        enemy ??= Enemies[random.Next(0, Enemies.Count)];

        if (enemy.IsChargingHeavy || random.Next(0, 2) == 1)
        {
            int damage = (int)(enemy.GetDamage() * (player.isGuarding ? 0.5f : 1));
            Description += $"{enemy.Name} strikes you! for " + damage + " damage";
            player.Health -= damage;
            enemy.IsChargingHeavy = false;
            if (player.Health <= 0)
                return new GameOverEvent("", $"You cannot give up just yet... \n{player.Name}! Stay determined."); // 'name' is empty since it will never be listed as an option
        }
        else
        {
            enemy.IsChargingHeavy = true;
            InputHelper.DisplayAndWait("The " + enemy.Name + " is charging a heavy attack!");
        }

        return this;
    }
}
