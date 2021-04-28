using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT.Models
{
    [Table("KanbanScan")]
    public class KanbanScan
    {
        [PrimaryKey, AutoIncrement]
        public int KanbanScanId { get; set; }
        public int ReqItemId { get; set; }
        public int KanbanReqId { get; set; }
        public string PartNo { get; set; }
        public string TagDataCode { get; set; }
        public int? TagSeqNo { get; set; }
        public double QtyUnit { get; set; }
        public DateTime ScanDateTime { get; set; }
        public string DeviceId { get; set; }
        public string SupplierCode { get; set; }
        public string EmpNo { get; set; }
        public int QRLength { get; set; }
    }
}
