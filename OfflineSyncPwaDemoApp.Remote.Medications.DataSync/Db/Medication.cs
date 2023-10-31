// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Datasync.EFCore;
using OfflineSyncPwaDemoApp.Remote.Core;
using System.ComponentModel.DataAnnotations;

namespace OfflineSyncPwaDemoApp.Remote.Medications.DataSync.Db
{
    /// <summary>
    /// The fields in this class must match the fields in Models/TodoItem.cs
    /// for the TodoApp.Data project.
    /// </summary>
    public class Medication : PgEntityTableData 
    {
        [Required, MinLength(1)]
        public string Name { get; set; } = "";

        public string Dose { get; set; } = "";
    }
}
