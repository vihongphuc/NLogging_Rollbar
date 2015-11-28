using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Configurations
{
    public class FileConfigurationStore : ConfigurationStore
    {
        private readonly FileIniDataParser fileIniDataParser;
        private readonly IList<string> filePaths;

        protected override IniData GetIniData()
        {
            IniData mergedIniData = null;
            foreach (var filename in filePaths)
            {
                var iniData = fileIniDataParser.ReadFile(filename, Encoding.UTF8);
                if (mergedIniData == null)
                {
                    mergedIniData = iniData;
                }
                else
                {
                    mergedIniData.Merge(iniData);
                }
            }

            return mergedIniData;
        }

        public FileConfigurationStore(IList<string> filePaths)
        {
            if (filePaths == null ||
                filePaths.Count == 0 ||
                !filePaths.All(c => !String.IsNullOrWhiteSpace(c)))
                throw new ArgumentNullException("filePaths", "The parameter filePaths cannot be empty or contains empty string");

            this.fileIniDataParser = new FileIniDataParser();
            this.fileIniDataParser.Parser.Configuration.CommentString = "#";
            this.filePaths = filePaths;
        }
        public FileConfigurationStore(string filePath)
            : this(new string[] { filePath })
        {
        }
    }
}
