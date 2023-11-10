// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.EntityFrameworkCore;

namespace OAHPwaDemoApp.Remote.Patients.DataSync.Db
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// The dataset for the TodoItems.
        /// </summary>
        public DbSet<Patient> Patient => Set<Patient>();

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
