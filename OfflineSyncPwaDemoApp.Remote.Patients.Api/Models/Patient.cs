using Microsoft.AspNetCore.Datasync.EFCore;
using OfflineSyncPwaDemoApp.Remote.Core;

namespace OfflineSyncPwaDemoApp.Remote.Patients.Api.Models
{
    public class Patient : PgEntityTableData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
