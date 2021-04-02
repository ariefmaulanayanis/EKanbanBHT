using EKanbanBHT.Helper;
using EKanbanBHT.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT.Data
{
    public class DbContext
    {
        string dbPath => FileAccessHelper.GetLocalFilePath("ekanban.db3");
        public SQLiteConnection conn;

        public DbContext()
        {
            conn = new SQLiteConnection(dbPath);
            //conn.DropTable<KanbanHeader>();
            conn.CreateTable<KanbanHeader>();
            //conn.DropTable<KanbanItem>();
            conn.CreateTable<KanbanItem>();
            //conn.DropTable<KanbanScan>();
            conn.CreateTable<KanbanScan>();
        }

        public KanbanItem KanbanItem { get; set; }
    }
}
