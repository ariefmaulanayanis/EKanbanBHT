using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT.Models
{

    [Table("KanbanHeader")]
    public class KanbanHeader
    {
        [PrimaryKey]
        public int KanbanReqId { get; set; }
        public string LineName { get; set; }
        public short LineNo { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestNo { get; set; }
        public string PickerName { get; set; }
        public DateTime? PickStart { get; set; }
        public DateTime? PickEnd { get; set; }
        public DateTime? UploadDate { get; set; }
        public DateTime? ReturnDate { get; set; }
    }
}
