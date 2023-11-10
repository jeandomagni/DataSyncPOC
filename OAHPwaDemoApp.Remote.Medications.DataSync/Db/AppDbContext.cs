// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace OAHPwaDemoApp.Remote.Medications.DataSync.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// The dataset for the TodoItems.
        /// </summary>
        public DbSet<Medication> Medications => Set<Medication>();

        /// <summary>
        /// Do any database initialization required.
        /// </summary>
        /// <returns>A task that completes when the database is initialized</returns>
        public async Task InitializeDatabaseAsync()
        {
            await this.Database.EnsureCreatedAsync().ConfigureAwait(false);
        }
    }
}
