using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT.Models
{
    public class KanbanHeader
    {
        public string LineName { get; set; }
        public Int16 LineNo { get; set; }
        public DateTime RequestDate { get; set; }
        public int RequestNo { get; set; }
    }
}
