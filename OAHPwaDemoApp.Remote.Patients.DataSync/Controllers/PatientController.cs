// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Datasync;
using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.AspNetCore.Mvc;
using OAHPwaDemoApp.Remote.Patients.DataSync.Db;

namespace OAHPwaDemoApp.Remote.Patients.DataSync.Controllers
{
    [Route("tables/patient")]
    public class PatientController : TableController<Patient>
    {
        public PatientController(AppDbContext context)
            : base(new EntityTableRepository<Patient>(context))
        {
        }
    }
}
