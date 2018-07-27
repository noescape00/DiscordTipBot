﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TipBot.Database.Models
{
    public class DiscordUser
    {
        public int Id { get; set; }

        public string Username { get; set; }

        public ulong DiscordUserId { get; set; }

        public double Balance { get; set; }

        //TODO add history of Tips, Withdrawals, Deposits

        public override string ToString()
        {
            return $"{nameof(this.Id)}:{this.Id},{nameof(this.Username)}:{this.Username},{nameof(this.DiscordUserId)}:{this.DiscordUserId},{nameof(this.Balance)}:{this.Balance})";
        }
    }
}