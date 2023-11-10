using Microsoft.AspNetCore.Datasync.EFCore;
using OAHPwaDemoApp.Remote.Core;

namespace OAHPwaDemoApp.Remote.Patients.Api.Models
{
    public class Patient : PgEntityTableData
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
