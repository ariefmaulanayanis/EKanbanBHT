using EKanbanBHT.Helper;
using EKanbanBHT.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using Xamarin.Essentials;

namespace EKanbanBHT.ViewModels
{
    public class UploadViewModel:ViewModelBase
    {
        public static KanbanItemRepository kanbanItemRepo { get; private set; }
        private string FTPHost { get; set; }
        private string FTPUser { get; set; }
        private string FTPPassword { get; set; }
        private string FTPPort { get; set; }

        private string empNo;
        public string EmpNo
        {
            get => empNo;
            set
            {
                empNo = value;
                OnPropertyChanged();
            }
        }

        private string content;
        public string Content
        {
            get => content;
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }

        private string statusMessage;
        public string StatusMessage
        {
            get => statusMessage;
            set
            {
                statusMessage = value;
                OnPropertyChanged();
            }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set
            {
                isBusy = value;
                OnPropertyChanged();
            }
        }


        public UploadViewModel()
        {
        }

        public void UploadDatFiles()
        {
            Content = "";
            StatusMessage = "";
            //string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "file.txt");
            //Content = File.ReadAllText(file);
            string filePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            kanbanItemRepo = new KanbanItemRepository();
            //validasi data availability
            List<KanbanHeader> headerList = kanbanItemRepo.GetSavedKanban();
            if (headerList.Count == 0)
            {
                StatusMessage = "There's no data to be uploaded.";
                return;
            }
            //validasi ftp
            FTPHost = Preferences.Get("ftpHost", "");
            FTPUser = Preferences.Get("ftpUser", "");
            FTPPassword = Preferences.Get("ftpPassword", "");
            FTPPort = Preferences.Get("ftpPort", "");
            if (string.IsNullOrEmpty(FTPHost) || string.IsNullOrEmpty(FTPUser) ||
                string.IsNullOrEmpty(FTPPassword) || string.IsNullOrEmpty(FTPPort))
            {
                StatusMessage = "FTP setting is not complete, please entry it in setting menu.";
                return;
            }
                        
            //loop data
            foreach (KanbanHeader header in headerList)
            {
                //get filename
                string filename = "OS";
                filename += header.PickEnd.Value.ToString("yy") + header.PickEnd.Value.ToString("MM") +
                    header.PickEnd.Value.ToString("dd") + header.PickEnd.Value.ToString("HH") +
                    header.PickEnd.Value.ToString("mm") + header.PickEnd.Value.ToString("ss");
                filename += Preferences.Get("device", "");
                filename += ".DAT";
                Content += filename + " generating file...\n";

                //get content
                List<KanbanScan> scanList = kanbanItemRepo.GetKanbanScan(header.KanbanReqId);
                string content = scanList.Count.ToString() + "\n";
                foreach (KanbanScan item in scanList)
                {
                    content += " "; //Slit Reader Address
                    content += "03"; //Transaction No.
                    content += char.IsNumber(item.PartNo[0]) ? "10" : "11"; //Tag Data Code
                    content += item.PartNo.PadRight(15, ' '); //Part No.
                    content += "".PadLeft(8, ' '); //Production Day
                    content += "".PadLeft(5, ' '); //Process Code
                    content += item.TagSeqNo.ToString().PadLeft(7, '0'); // Tag Sequence No.
                    content += "".PadLeft(9, ' '); //Production Order No.
                    content += item.QtyUnit.ToString().PadLeft(7, '0'); //Qty per Unit of Tag
                    content += "".PadLeft(7, ' '); //Truck No.
                    content += item.ScanDateTime.ToString("yyyy") + item.ScanDateTime.ToString("MM") + item.ScanDateTime.ToString("dd"); //Stock Issued Date
                    content += "2"; //Normal / Cancel Clasification
                    content += item.QtyUnit.ToString().PadLeft(7, '0'); //Stock Issued Qty
                    content += "".PadLeft(8, ' '); //Customer Code
                    content += "01"; //Ware House
                    content += item.ScanDateTime.ToString("yyyy") + item.ScanDateTime.ToString("MM") + item.ScanDateTime.ToString("dd"); //Process Date
                    content += item.ScanDateTime.ToString("HH") + item.ScanDateTime.ToString("mm") + item.ScanDateTime.ToString("ss"); //Process Time
                    content += Preferences.Get("device", ""); //Device ID
                    content += "".PadLeft(1, ' '); //Terminal No.
                    content += "".PadLeft(5, ' '); //QR Sequence No.
                    content += "".PadLeft(4, ' '); //Text Sequence No.
                    content += "".PadLeft(4, ' '); //Number of Text
                    content += "".PadLeft(10, ' '); //Server Name
                    content += "".PadLeft(7, ' '); //Receive Batch No.
                    content += "".PadLeft(4, ' '); //Receive Sequence No.
                    content += "".PadLeft(5, ' '); //Processing Status
                    content += (item.SupplierCode == null ? "" : item.SupplierCode).PadRight(5, ' '); //Supplier Code
                    content += "".PadLeft(20, ' '); //Receive Time
                    content += Preferences.Get("user", "").PadRight(9, ' '); //User BHT/Scanner
                    content += "\n"; //new line
                }

                //write text file
                string fullPath = Path.Combine(filePath, filename);
                File.WriteAllText(fullPath, content.ToString());

                //copy to FTP
                Content += filename + " uploading file...\n";
                UploadFTP(filename);
                if (StatusMessage == "")
                {
                    Content += filename + " upload success.\n";
                    kanbanItemRepo.UpdateUploadDate(header.KanbanReqId);
                }
                else
                {
                    Content += filename + " upload fail.\n";
                    Content += "Upload process stopped.\n";
                    return;
                }
            }
        }

        private void UploadFTP(string filename)
        {
            //StatusMessage = "";
            //string FTPHost = Preferences.Get("ftpHost", "");
            //string FTPUser = Preferences.Get("ftpUser", "");
            //string FTPPassword = Preferences.Get("ftpPassword", "");
            //string FTPPort = Preferences.Get("ftpPort", "");
            //if (string.IsNullOrEmpty(FTPHost) || string.IsNullOrEmpty(FTPUser) ||
            //    string.IsNullOrEmpty(FTPPassword) || string.IsNullOrEmpty(FTPPort))
            //{
            //    //menuVM.IsBusy = false;
            //    StatusMessage = "FTP setting is not complete, please entry it in setting menu.";
            //}
            //else
            //{
            //}

            try
            {
                string file = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);

                //copy file
                //string destination = "ftp://files.000webhost.com/tmp/file.txt";
                string destination = FTPHost + ":" + FTPPort + "/" + filename;
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(destination);
                request.Method = WebRequestMethods.Ftp.UploadFile;
                //request.UseBinary = true;
                //request.UsePassive = false;//true;
                //request.KeepAlive = false;
                //request.Timeout = System.Threading.Timeout.Infinite;
                //request.AuthenticationLevel = System.Net.Security.AuthenticationLevel.None;
                //request.Credentials = new NetworkCredential("alhijrahshop123", "maniez1982");
                request.Credentials = new NetworkCredential(FTPUser, FTPPassword);

                StreamReader sourceStream = new StreamReader(file);
                byte[] fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd());
                sourceStream.Close();
                request.ContentLength = fileContents.Length;

                Stream requestStream = request.GetRequestStream();
                requestStream.Write(fileContents, 0, fileContents.Length);
                requestStream.Close();
                requestStream.Dispose();

                //
                FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                response.Close();
                response.Dispose();
                //menuVM.IsBusy = false;
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }
    }
}
