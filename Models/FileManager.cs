using Azure.Storage.Blobs;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AzureFileManager.Models
{
    public class FileManager
    {
        private readonly string _blobStorageConnectionString = "DefaultEndpointsProtocol=https;AccountName=mysitenew;AccountKey=kMZrTHPeDF9PtI/LNZCNHBIEX1Ee56m6iILP0GgqxLrU5ug7kPMIQu+OgNzaKnjp4vdXgWc+eoNu+AStNEQqkw==;EndpointSuffix=core.windows.net";
        private readonly string _blobStorageContainerName = "web";
        private CloudBlobClient _cloudBlobClient; 
        private CloudBlobContainer _cloudContainer; 
        private BlobContainerClient _container;
        private CloudBlockBlob _blockBlob;

        private List<string> files = new List<string>();
        public List<string> filesCollection;
        private string _searchInputText;
        private string _fileText;

        public string SelectedItem { get; set; }

        public FileManager()
        {
            _cloudBlobClient = CloudStorageAccount.Parse(_blobStorageConnectionString).CreateCloudBlobClient();
            _cloudContainer = _cloudBlobClient.GetContainerReference(_blobStorageContainerName);
            _container = new BlobContainerClient(_blobStorageConnectionString, _blobStorageContainerName);
            filesCollection = new List<string>(files);
        }

        public List<string> GetAllFiles()
        {
            List<string> fileList = _cloudContainer.ListBlobs().OfType<CloudBlockBlob>().Select(blob => blob.Name).ToList();
            if (files.Count > 0)
            {
                files.Clear();
            }
            foreach (var item in fileList)
            {
                FileInfo fileInfo = new FileInfo(item);
                
                files.Add($"{fileInfo.Name}");
            }

            return files;

        }
        public async void UploadFile()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            dialog.ShowDialog();
            if (!String.IsNullOrEmpty(dialog.FileName))
            {
                BlobClient blob = _container.GetBlobClient(dialog.SafeFileName);

                FileStream stream = File.OpenRead(dialog.FileName);
                await blob.UploadAsync(stream, overwrite: true);
            }
            GetAllFiles();
        }
        public void DeleteFile()
        {
            _container.GetBlobClient(SelectedItem).DeleteIfExists(); ;
            GetAllFiles();
        }
        public void DownloadFile()
        { 
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog.ShowDialog();
            if (!String.IsNullOrEmpty(saveFileDialog.FileName))
            {
                _blockBlob = _cloudContainer.GetBlockBlobReference(SelectedItem);
                _blockBlob.DownloadToFile(saveFileDialog.FileName, FileMode.Create);
            }
        }

        public void SearchFile()
        {
            if (_searchInputText != "")
            {
                List<string> tmpList = new List<string>(files);
                files.Clear();
                foreach (string item in tmpList)
                {
                    if (item.StartsWith(_searchInputText))
                    {
                        files.Add(item);
                    }
                }
            }
        }
        public async Task<string> GetFileText()
        {
            if (SelectedItem != null)
            {
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, SelectedItem);

                _blockBlob = _cloudContainer.GetBlockBlobReference(SelectedItem);
                _blockBlob.DownloadToFile(path, FileMode.Create);
                using (StreamReader reader = new StreamReader(path))
                {
                    _fileText = await reader.ReadToEndAsync();
                }
            }
            return _fileText;
        }
    }
}
