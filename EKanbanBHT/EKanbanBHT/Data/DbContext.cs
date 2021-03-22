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
            conn.CreateTable<KanbanItem>();
        }

        public KanbanItem KanbanItem { get; set; }
    }
}
