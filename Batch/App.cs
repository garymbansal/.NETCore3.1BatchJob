using System;
using Microsoft.Extensions.Logging;
using Batch.AppSettings;
using Batch.Services;   
namespace Batch
{
    public class App
    {
        private readonly ILogger _logger;
        private readonly IAppSetting _appSetting;
        private readonly IDataService _dataService;
        public App(ILogger<App> logger, IAppSetting appSetting, IDataService dataService)
        {
            _logger = logger;
            _appSetting = appSetting;
            _dataService = dataService;
        }
        public void Start()
        {
            try
            {
                var sourceSetting = this._appSetting.SourceSetting;
                var destSetting = this._appSetting.DestSetting;

                this._dataService.ProcessData(sourceSetting, destSetting);    
            }
            catch (System.Exception ex)
            {
                _logger.LogInformation(ex.Message);
            }
            

        }
    }
}