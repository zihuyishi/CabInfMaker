using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CabInfMaker
{
    public class CabInfControl
    {
        public CabInfControl()
        { }
        public String DesFolder
        {
            set
            {
                if (!invalidFolder(value)) throw new Exception("无效的目录");
                _desFolder = value;
                _infMaker = new InfMaker(_desFolder);
            }
            get
            {
                return _desFolder;
            }
        }
        public List<FileItem> FileList
        {
            get
            {
                return _infMaker.FileList;
            }
        }
        private String _desFolder;
        private InfMaker _infMaker;
        static private String guidRegexPattern = @"\w{8}(-\w{4}){3}-\w{12}";
        /// <summary>
        /// 生成Inf文件的内容
        /// </summary>
        /// <returns>生成的文件内容</returns>    
        public String MakeInf()
        {
            if (_infMaker == null) throw new Exception("无效的参数");
            return _infMaker.MakeInf();
        }
        public void WriteToFile(String filePath)
        {
            if (_infMaker == null) throw new Exception("无效的参数");
            _infMaker.WriteToFile(filePath);
        }
        /// <summary>
        /// 判断字符串是否为GUID的格式
        /// </summary>
        /// <param name="guid">要判断的guid</param>
        /// <returns></returns>
        static public Boolean IsGuid(String guid)
        {
            if (String.IsNullOrEmpty(guid)) {
                return false;
            }
            //the length of guid is 36.
            if (guid.Length != 36)
            {
                return false;
            }
            var guidReg = new System.Text.RegularExpressions.Regex(guidRegexPattern);
            return guidReg.IsMatch(guid);
        }
        /// <summary>
        /// 判断是否为有效的文件夹路径
        /// </summary>
        /// <param name="folder">文件夹路径</param>
        /// <returns></returns>
        static private Boolean invalidFolder(String folder)
        {
            return Directory.Exists(folder);
        }
        
    }
}
