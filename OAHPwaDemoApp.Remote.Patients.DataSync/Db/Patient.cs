// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Datasync.EFCore;
using OAHPwaDemoApp.Remote.Core;
using System.ComponentModel.DataAnnotations;

namespace OAHPwaDemoApp.Remote.Patients.DataSync.Db
{
    /// <summary>
    /// The fields in this class must match the fields in Models/TodoItem.cs
    /// for the TodoApp.Data project.
    /// </summary>
    public class Patient : PgEntityTableData 
    { 
        [Required, MinLength(1)]
        public string FirstName { get; set; } = "";

        public string LastName { get; set; } = "";
    }
}
