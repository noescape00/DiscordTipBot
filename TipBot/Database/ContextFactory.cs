﻿using System;
using Microsoft.EntityFrameworkCore;

namespace TipBot.Database
{
    public class ContextFactory : IContextFactory
    {
        private readonly Settings settings;

        public ContextFactory(Settings settings)
        {
            this.settings = settings;
        }

        public BotDbContext CreateContext()
        {
            string connectionString = this.settings.ConnectionString;

            DbContextOptions<BotDbContext> options = new DbContextOptionsBuilder<BotDbContext>().UseSqlServer(connectionString, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            }).Options;

            return new BotDbContext(options);
        }
    }

    public interface IContextFactory
    {
        BotDbContext CreateContext();
    }
}
