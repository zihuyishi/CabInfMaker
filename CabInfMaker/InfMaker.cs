using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CabInfMaker
{
    public class InfMaker
    {
        public InfMaker(String folderPath)
        {
            IncludeSubFolder = false;
            FolderPath = folderPath;
        }
        
        /// <summary>
        /// 注释
        /// </summary>
        public String Tag
        {
            set;
            get;
        }
        public String FolderPath
        {
            set
            {
                _folderPath = value;
                makeFileList();
            }
            get
            {
                return _folderPath;
            }
        }
        public Boolean IncludeSubFolder
        {
            set;
            get;
        }
        public List<FileItem> FileList
        {
            set
            {
                m_fileList = value;
            }
            get
            {
                return m_fileList;
            }
        }
        private String[] _desExtent = { ".dll", ".ocx" };
        private String _folderPath;
        private List<FileItem> m_fileList;
        public String MakeInf()
        {
            StringBuilder infContent = new StringBuilder(500);
            if (!String.IsNullOrEmpty(Tag))
            {
                infContent.Append(Tag);
            }
            infContent.Append("[version]\nsignature=\"$CHICAGO$\"\n");
            infContent.Append("\n[Add.Code]\n");

            foreach (var file in m_fileList)
            {
                infContent.AppendFormat("{0}={0}\n", file.Name);
            }
            infContent.Append("\n");

            foreach (var file in m_fileList)
            {
                infContent.Append(file.ToString());
            }
            infContent.Append("\n;END OF INF FILE");
            return infContent.ToString();
        }
        public void WriteToFile(String filePath)
        {
            File.WriteAllText(filePath, MakeInf());
        }
        private void makeFileList()
        {
            if (String.IsNullOrEmpty(FolderPath))
            {
                throw new NullReferenceException("未设置目标文件夹");
            }
            if (!Directory.Exists(FolderPath))
            {
                throw new FileNotFoundException("目标文件夹不存在");
            }
            FileList = new List<FileItem>();
            DirectoryInfo directory = new DirectoryInfo(FolderPath);
            foreach (var file in directory.GetFiles())
            {
                try
                {
                    if (_desExtent.Any(str => str == file.Extension))
                    {
                        FileItem fileItem = new FileItem(file.FullName);
                        FileList.Add(fileItem);
                    }
                }
                catch (FileNotFoundException)
                {
                }
            }
        }
    }
}
