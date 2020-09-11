using Microsoft.Extensions.Configuration;
using System.IO;
namespace Batch.AppSettings
{
    public class AppSetting : IAppSetting
    {
        private readonly IConfiguration _config;
        public AppSetting()
        {
            _config = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .Build();
        }
        public string SourceType
        {
            get
            {
                return _config.GetValue<string>(KeysAppSetting.Source + ":" + KeysAppSetting.Type);
            }
        }
        public string DestType
        {
            get
            {
                return _config.GetValue<string>(KeysAppSetting.Destination + ":" + KeysAppSetting.Type);
            }
        }
        public object SourceSetting
        {
            get
            {
                var _sourceSetting = (object)null;
                if (SourceType == KeysAppSetting.File)
                {
                    _sourceSetting = new FileSetting();
                    _config.GetSection(KeysAppSetting.Source + ":" + KeysAppSetting.File).Bind(_sourceSetting);

                }
                return _sourceSetting;
            }
        }
        public object DestSetting
        {
            get
            {
                var _destSetting = (object)null;
                if (DestType == KeysAppSetting.DB)
                {
                    _destSetting = new DBSetting();
                    _config.GetSection(KeysAppSetting.Destination + ":" + KeysAppSetting.DB).Bind(_destSetting);
                }
                return _destSetting;
            }
        }
    }
    public class DBSetting
    {
        public string ConnectionString { get; set; }
        public string TableName { get; set; }
    }
    public class FileSetting
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
    }

}