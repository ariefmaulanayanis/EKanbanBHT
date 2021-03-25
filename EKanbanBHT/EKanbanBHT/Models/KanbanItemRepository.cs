using EKanbanBHT.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace EKanbanBHT.Models
{
    public class KanbanItemRepository:DbContext
    {
        public string StatusMessage { get; set; }
        public bool IsError { get; set; }

        public void DeleteOldData()
        {
            int result = 0;
            IsError = false;
            StatusMessage = "";
            DateTime maxDate = DateTime.Now.AddMonths(-1);
            try
            {
                List<KanbanItem> itemList = conn.Table<KanbanItem>().Where(a => a.UploadDate < maxDate).ToList();
                foreach(KanbanItem item in itemList)
                {
                    result += conn.Delete<KanbanItem>(item);
                }
                StatusMessage = string.Format("{0} record(s) deleted.", result);
            }
            catch(Exception e)
            {
                IsError = true;
                StatusMessage = string.Format("Failed to delete old data.\nError: {0}", e.Message);
            }
        }

        public void DeleteAllData()
        {
            int result = 0;
            IsError = false;
            StatusMessage = "";
            try
            {
                List<KanbanItem> itemList = conn.Table<KanbanItem>().ToList();
                foreach (KanbanItem item in itemList)
                {
                    result += conn.Delete<KanbanItem>(item);
                }
                StatusMessage = string.Format("{0} record(s) deleted.", result);
            }
            catch (Exception e)
            {
                IsError = true;
                StatusMessage = string.Format("Failed to delete all data.\nError: {0}", e.Message);
            }
        }

        public void SyncData(List<KanbanItem> items)
        {
            int result = 0;
            IsError = false;
            StatusMessage = "";
            try
            {
                foreach (KanbanItem item in items)
                {
                    KanbanItem kanbanItem = conn.Table<KanbanItem>().Where(a => a.ReqItemId == item.ReqItemId).FirstOrDefault();
                    if (kanbanItem == null)
                    {
                        //items.Add(item);
                        item.ScanQty = 0;
                        item.Balance = item.OrderQty;
                        result += conn.Insert(item);
                    }
                    List<KanbanItem> test = conn.Table<KanbanItem>().ToList();
                }
                StatusMessage = string.Format("{0} record(s) inserted.", result);
            }
            catch(Exception e)
            {
                IsError = true;
                StatusMessage = string.Format("Failed to sync data.\nError: {0}", e.Message);
            }
        }
        
        public List<KanbanItem> GetKanbanItems(DateTime requestDate,int reqNo)
        {
            short i = 0;
            List<KanbanItem> kanbanItems = new List<KanbanItem>();
            kanbanItems = conn.Table<KanbanItem>().Where(a => a.RequestDate == requestDate && a.ReqNo == reqNo)
                .OrderBy(a=>a.ReqItemId).ToList();
            foreach(KanbanItem item in kanbanItems.ToArray())
            {
                try
                {
                    item.ScanQty = 0;
                    item.Balance = item.OrderQty;
                    i++;
                    item.RowNumber = i;
                    kanbanItems[i - 1] = item;
                }
                catch(Exception e)
                {

                }
            }
            return kanbanItems;
        }

        //public void Insert(string name)
        //{
        //    int result = 0;
        //    try
        //    {
        //        //basic validation to ensure a name was entered
        //        if (string.IsNullOrEmpty(name))
        //            throw new Exception("Valid name required");

        //        result = conn.Insert(new Person { Name = name });

        //        StatusMessage = string.Format("{0} record(s) added [Name: {1})", result, name);
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
        //    }
        //}

        //public List<Person> GetAllPeople()
        //{
        //    try
        //    {
        //        return conn.Table<Person>().ToList();
        //    }
        //    catch (Exception ex)
        //    {
        //        StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        //    }

        //    return new List<Person>();
        //}

    }
}
