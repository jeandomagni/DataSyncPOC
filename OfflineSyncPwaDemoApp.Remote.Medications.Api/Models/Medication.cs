using OfflineSyncPwaDemoApp.Remote.Core;

namespace OfflineSyncPwaDemoApp.Remote.Medications.Api.Models
{
    public class Medication : PgEntityTableData
    {
        public string Name { get; set; }
        public string Dose { get; set; }
    }
}
