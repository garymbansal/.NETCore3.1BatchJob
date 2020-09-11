using Batch.AppSettings;
namespace Batch.Services
{
    public interface IDataService
    {
        void ProcessData(object fileSetting, object dBSetting);
    }
}