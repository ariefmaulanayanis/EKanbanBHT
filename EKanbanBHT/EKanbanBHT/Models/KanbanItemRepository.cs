using EKanbanBHT.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EKanbanBHT.Models
{
    public class KanbanItemRepository:DbContext
    {
        public string StatusMessage { get; set; }
        public bool IsError { get; set; }

        public void DeleteOldData()
        {
            int numHead = 0;
            int numItem = 0;
            int numScan = 0;
            IsError = false;
            StatusMessage = "";
            DateTime maxDate = DateTime.Now.AddMonths(-1);
            try
            {
                //get uploaded kanban more than 1 month ago
                List<KanbanHeader> headerList = conn.Table<KanbanHeader>().Where(a => a.UploadDate < maxDate).ToList();
                //loop kanban
                foreach(KanbanHeader header in headerList)
                {
                    //delete kanban item
                    List<KanbanItem> itemList = conn.Table<KanbanItem>().Where(a => a.KanbanReqId==header.KanbanReqId).ToList();
                    foreach (KanbanItem item in itemList)
                    {
                        numItem += conn.Delete(item);
                    }

                    //delete scan result
                    List<KanbanScan> scanList = conn.Table<KanbanScan>().Where(a => a.KanbanReqId == header.KanbanReqId).ToList();
                    foreach (KanbanScan scan in scanList)
                    {
                        numScan += conn.Delete(scan);
                    }

                    //delete kanban header
                    numHead += conn.Delete(header);
                }

                //StatusMessage = string.Format("{0} record(s) deleted.", result);
                StatusMessage = string.Format("{0} kanban(s) deleted.\n{1} kanban item(s) deleted.\n{2} scan result(s) deleted.\n", numHead, numItem, numScan);
            }
            catch (Exception e)
            {
                IsError = true;
                StatusMessage = string.Format("Failed to delete old data.\nError: {0}", e.Message);
            }
        }

        public void DeleteAllData()
        {
            int numHead = 0;
            int numItem = 0;
            int numScan = 0;
            IsError = false;
            StatusMessage = "";
            try
            {
                //delete kanban header
                List<KanbanHeader> headerList = conn.Table<KanbanHeader>().ToList();
                foreach (KanbanHeader header in headerList)
                {
                    numHead += conn.Delete(header);
                }

                //delete kanban item
                List<KanbanItem> itemList = conn.Table<KanbanItem>().ToList();
                foreach (KanbanItem item in itemList)
                {
                    numItem += conn.Delete(item);
                }

                //delete kanban scan
                List<KanbanScan> scanList = conn.Table<KanbanScan>().ToList();
                foreach (KanbanScan scan in scanList)
                {
                    numScan += conn.Delete(scan);
                }

                //StatusMessage = string.Format("{0} record(s) deleted.", result);
                StatusMessage = string.Format("{0} kanban(s) deleted.\n{1} kanban item(s) deleted.\n{2} scan result(s) deleted.\n", numHead, numItem, numScan);
            }
            catch (Exception e)
            {
                IsError = true;
                StatusMessage = string.Format("Failed to delete all data.\nError: {0}", e.Message);
            }
        }

        public void SyncData(List<KanbanSync> items)
        {
            int result = 0;
            IsError = false;
            StatusMessage = "";
            try
            {
                foreach (KanbanSync item in items)
                {
                    //KanbanHeader
                    KanbanHeader kanbanHeader = conn.Table<KanbanHeader>().Where(a => a.KanbanReqId == item.KanbanReqId).FirstOrDefault();
                    if (kanbanHeader == null)
                    {
                        kanbanHeader = new KanbanHeader();
                        kanbanHeader.KanbanReqId = item.KanbanReqId;
                        kanbanHeader.LineName = item.LineName;
                        kanbanHeader.LineNo = item.LineNo;
                        kanbanHeader.RequestDate = item.RequestDate;
                        kanbanHeader.RequestNo = item.RequestNo;
                        conn.Insert(kanbanHeader);
                    }

                    //insert update KanbanItem
                    KanbanItem kanbanItem = conn.Table<KanbanItem>().Where(a => a.ReqItemId == item.ReqItemId).FirstOrDefault();
                    if (kanbanItem == null)
                    {
                        //items.Add(item);
                        kanbanItem = new KanbanItem();
                        kanbanItem.ReqItemId = item.ReqItemId;
                        kanbanItem.KanbanReqId = item.KanbanReqId;
                        kanbanItem.PartId = item.PartId;
                        kanbanItem.PartNo = item.PartNo;
                        kanbanItem.LotSize = item.LotSize;
                        kanbanItem.Zone = item.Zone;
                        kanbanItem.OrderQty = item.OrderQty;
                        kanbanItem.ScanQty = 0;
                        kanbanItem.Balance = item.OrderQty;
                        //kanbanItem.RowNumber = item.ReqItemId;
                        //item.ScanQty = 0;
                        //item.Balance = item.OrderQty;
                        //result += conn.Insert(item);
                        conn.Insert(kanbanItem);
                    }
                    List<KanbanHeader> testHeader = conn.Table<KanbanHeader>().ToList();
                    List<KanbanItem> testItem = conn.Table<KanbanItem>().ToList();
                    result++;
                }
                StatusMessage = string.Format("{0} record(s) inserted.", result);
            }
            catch(Exception e)
            {
                IsError = true;
                StatusMessage = string.Format("Failed to sync data.\nError: {0}", e.Message);
            }
        }

        public KanbanHeader GetKanbanHeader(DateTime requestDate, int reqNo)
        {
            KanbanHeader kanbanHeader = conn.Table<KanbanHeader>().Where(a => a.RequestDate == requestDate && a.RequestNo == reqNo && a.UploadDate == null).FirstOrDefault();
            return kanbanHeader;
        }

        public List<KanbanItem> GetKanbanItems(int kanbanReqId)
        {
            short i = 0;
            List<KanbanItem> kanbanItems = new List<KanbanItem>();
            //kanbanItems = conn.Table<KanbanItem>().Where(a => a.KanbanReqId == kanbanReqId).OrderBy(a=>a.ReqItemId).ToList();
            kanbanItems = conn.Table<KanbanItem>().Where(a => a.KanbanReqId == kanbanReqId)
                .OrderBy(a => a.Zone).ThenBy(a=>a.PartNo).ToList();
            foreach (KanbanItem item in kanbanItems.ToArray())
            {
                try
                {
                    //item.ScanQty = 0;
                    //item.Balance = item.OrderQty;
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

        public List<KanbanScan> GetKanbanScan(int kanbanReqId)
        {
            short i = 0;
            List<KanbanScan> scanList = new List<KanbanScan>();
            scanList = conn.Table<KanbanScan>().Where(a => a.KanbanReqId == kanbanReqId)
                .OrderBy(a => a.ReqItemId).ToList();
            return scanList;
        }

        public void KanbanSave(int kanbanReqId,DateTime pickStart,bool isCompleted)
        {
            KanbanHeader header = conn.Table<KanbanHeader>().Where(a => a.KanbanReqId == kanbanReqId).FirstOrDefault();
            header.PickStart = pickStart;
            if(isCompleted) header.PickEnd = DateTime.Now;
            header.PickerName = Preferences.Get("user", "");
            conn.Update(header);
        }

        public void UpdateUploadDate(int kanbanReqId)
        {
            KanbanHeader header = conn.Table<KanbanHeader>().Where(a => a.KanbanReqId == kanbanReqId).FirstOrDefault();
            header.UploadDate = DateTime.Now;
            conn.Update(header);
        }

        //public void KanbanScanSave(List<KanbanScan> scanList)
        //{
        //    foreach(KanbanScan kanbanScan in scanList)
        //    {
        //        conn.Insert(kanbanScan);
        //    }
        //}

        public void KanbanScanSave(KanbanScan kanbanScan)
        {
            conn.Insert(kanbanScan);
        }

        public List<string> GetScanDate()
        {
            List<string> scanDateList=new List<string>();
            List<KanbanHeader> headerList = conn.Table<KanbanHeader>()
                .Where(a => a.PickEnd != null && a.UploadDate == null).OrderBy(a => a.PickEnd).ToList();
            foreach(KanbanHeader header in headerList)
            {
                string scanDate = header.PickEnd.Value.ToString("yy") + header.PickEnd.Value.ToString("MM") +
                    header.PickEnd.Value.ToString("dd") + header.PickEnd.Value.ToString("HH") +
                    header.PickEnd.Value.ToString("mm") + header.PickEnd.Value.ToString("ss");
                if (!scanDateList.Exists(a => a.Equals(scanDate)))
                {
                    scanDateList.Add(scanDate);
                }
            }
            return scanDateList;
        }

        public List<KanbanHeader> GetSavedKanban()
        {
            List<KanbanHeader> headerList = conn.Table<KanbanHeader>()
                .Where(a => a.PickEnd != null && a.UploadDate == null).OrderBy(a => a.PickEnd).ToList();
            return headerList;
        }

        public List<KanbanHeader> GetSavedKanbanForReturn()
        {
            List<KanbanHeader> headerList = conn.Table<KanbanHeader>()
                .Where(a => a.PickEnd != null).ToList();
            return headerList;
        }

        public void UpdateReturnDate(int kanbanReqId)
        {
            KanbanHeader header = conn.Table<KanbanHeader>().Where(a => a.KanbanReqId == kanbanReqId).FirstOrDefault();
            header.ReturnDate = DateTime.Now;
            conn.Update(header);
        }

        public void UpdateBalance(KanbanItem item)
        {
            conn.Update(item);
        }
    }
}
