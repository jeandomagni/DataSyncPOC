// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
using OAHPwaDemoApp.Remote.Medications.DataSync.Db;

namespace OAHPwaDemoApp.Remote.Medications.DataSync.Controllers
{
    [Route("tables/medication")]
    public class MedicationController : TableController<Medication>
    {
        public MedicationController(AppDbContext context)
            : base(new EntityTableRepository<Medication>(context))
        {
        }
    }
}
