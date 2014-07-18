using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CabInfMaker
{
    public class FileItem
    {
        public FileItem(String fullFileName)
        {
            if (!File.Exists(fullFileName))
            {
                throw new FileNotFoundException("文件不存在");
            }
            FullName = fullFileName;
            NeedRegister = false;
        }
        public String FullName
        {
            set
            {
                _fullName = value;
                _name = _fullName.Substring(_fullName.LastIndexOf('\\') + 1);
            }
            get
            {
                return _fullName;
            }
        }
        public String Name
        {
            get
            {
                return _name;
            }
        }
        public String Clsid
        {
            set;
            get;
        }
        public Boolean NeedRegister
        {
            set;
            get;
        }
        private String _fullName;
        private String _name;
        /// <summary>
        /// 转换成需要的[xxxx.xxx]结构
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder str = new StringBuilder(300);
            str.AppendFormat("[{0}]\n", Name);
            str.Append("file-win32-x86=thiscab\n");
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(FullName);
            if (!String.IsNullOrEmpty(info.FileVersion))
            {
                str.AppendFormat("FileVersion={0}\n", info.FileVersion);
            }
            if (!String.IsNullOrEmpty(Clsid))
            {
                str.Append("clsid={" + Clsid +"}\n");
            }
            if (NeedRegister)
            {
                str.Append("RegisterServer=yes\n");
            }
            str.Append("\n");
            return str.ToString();
        }
    }
}
