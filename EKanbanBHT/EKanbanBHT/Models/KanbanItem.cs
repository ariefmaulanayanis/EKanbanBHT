using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace EKanbanBHT.Models
{
    [Table("KanbanItem")]
    public class KanbanItem
    {
        [PrimaryKey]
        public int ReqItemId { get; set; }
        public int KanbanReqId { get; set; }
        //public DateTime RequestDate { get; set; }
        //public int ReqNo { get; set; }
        public int PartId { get; set; }
        [MaxLength(15)]
        public string PartNo { get; set; }
        public double LotSize { get; set; }
        [MaxLength(25)]
        public string Zone { get; set; }
        public int? OrderQty { get; set; }
        public int? ScanQty { get; set; }
        public int? Balance { get; set; }
        //public DateTime? UploadDate { get; set; }
        public short? RowNumber { get; set; }
        public string BackgroundColor { get; set; }
    }
}
