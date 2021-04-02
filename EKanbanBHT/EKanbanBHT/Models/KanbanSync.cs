using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT.Models
{
    public class KanbanSync
    {
        public int ReqItemId { get; set; }
        public int KanbanReqId { get; set; }
        public int RequestNo { get; set; }
        public DateTime RequestDate { get; set; }
        public int ReqNo { get; set; }
        public int PartId { get; set; }
        public string PartNo { get; set; }
        public double LotSize { get; set; }
        public string Zone { get; set; }
        public short LineNo { get; set; }
        public string LineName { get; set; }
        public int OrderQty { get; set; }
    }
}
