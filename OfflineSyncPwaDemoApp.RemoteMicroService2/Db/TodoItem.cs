// Copyright (c) Microsoft Corporation. All Rights Reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Datasync.EFCore;
using System.ComponentModel.DataAnnotations;

namespace OfflineSyncPwaDemoApp.RemoteMicroService2.Db
{
    /// <summary>
    /// The fields in this class must match the fields in Models/TodoItem.cs
    /// for the TodoApp.Data project.
    /// </summary>
    public class TodoItem : PgEntityTableData 
    {
        [Required, MinLength(1)]
        public string Title { get; set; } = "";

        public bool IsComplete { get; set; }
    }
}
