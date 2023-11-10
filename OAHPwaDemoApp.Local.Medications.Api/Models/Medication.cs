using Microsoft.Datasync.Client;

namespace OAHPwaDemoApp.Local.Medications.Api.Models
{
    public class Medication : DatasyncClientData, IEquatable<Medication>
    {
        public string Name { get; set; } = string.Empty;
        public string Dose { get; set; } = string.Empty;

        public bool Equals(Medication other) => other != null && other.Id == Id;
    }
}
