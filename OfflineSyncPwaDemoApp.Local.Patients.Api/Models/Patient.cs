using Microsoft.Datasync.Client;

namespace OfflineSyncPwaDemoApp.Local.Patients.Api.Models
{
    public class Patient : DatasyncClientData, IEquatable<Patient>
    {
        public  string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;

        public bool Equals(Patient other) => other != null && other.Id == Id;
    }
}
