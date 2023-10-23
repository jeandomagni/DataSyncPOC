using Microsoft.AspNetCore.Datasync.EFCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace OfflineSyncPwaDemoApp.RemoteMicroService2.Db {
    public class PgEntityTableData : EntityTableData {
        /// <summary>
        /// The row version for the entity.
        /// </summary>
        [NotMapped]
        public override byte[] Version {
            get => BitConverter.GetBytes(RowVersion);
            set => BitConverter.ToUInt32(value);
        }

        /// <summary>
        /// The actual version
        /// </summary>
        [JsonIgnore]
        [Timestamp]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [Column("xmin", TypeName = "xid")]
        public uint RowVersion { get; set; }
    }
}
