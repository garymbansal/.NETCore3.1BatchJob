
namespace Batch.AppSettings
{
    public interface IAppSetting
    {
        object SourceSetting { get; }
        object DestSetting { get; }
    }
}