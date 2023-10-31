using Microsoft.AspNetCore.Datasync.EFCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OfflineSyncPwaDemoApp.Remote.Core
{
    public class PgEntityTableData : EntityTableData
    {
        /// <summary>
        /// The row version for the entity.
        /// </summary>
        [NotMapped]
        public override byte[] Version
        {
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
