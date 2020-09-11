using System;
using System.IO;
using System.Globalization;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ComponentModel;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using CsvHelper;
using CsvHelper.TypeConversion;
using FluentValidation.Results;
using Batch.AppSettings;
using Batch.Models;
namespace Batch.Services
{
    public class DataService : IDataService
    {
        private readonly ILogger _logger;
        public DataService(ILogger<App> logger)
        {
            _logger = logger;
        }

        public void ProcessData(object sourceSetting, object destSetting)
        {
            if (sourceSetting != null && sourceSetting.GetType() == typeof(FileSetting))
            {
                try
                {
                    var fileSetting = (FileSetting)sourceSetting;
                    var sourceFile = Path.Combine(fileSetting.FilePath, fileSetting.FileName);
                    if (File.Exists(sourceFile))
                    {
                        using (var reader = new StreamReader(sourceFile))
                        {
                            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
                            {
                                var options = new TypeConverterOptions { Formats = new[] { "dd.MM.yy" } };
                                csvReader.Configuration.RegisterClassMap<StoreOrderMap>();
                                csvReader.Configuration.TypeConverterOptionsCache.AddOptions<DateTime>(options);

                                var records = csvReader.GetRecords<StoreOrder>();

                                var validRecords = GetValidRecords(records);
                                if (validRecords != null)
                                {
                                    if (destSetting != null && destSetting.GetType() == typeof(DBSetting))
                                    {
                                        var dbSetting = (DBSetting)destSetting;
                                        ImportData(dbSetting, validRecords);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        _logger.LogInformation("File {0} not found", sourceFile);
                        throw new FileNotFoundException();
                    }
                }
                catch (System.Exception)
                {
                    _logger.LogInformation("Exception in ProcessData()");
                    throw;
                }
            }
        }
        private IEnumerable<StoreOrder> GetValidRecords(IEnumerable<StoreOrder> records)
        {
            var validRecords = new List<StoreOrder>();
            foreach (var item in records)
            {
                var validator = new StoreOrderValidator();
                var validationResult = validator.Validate(item);

                if (validationResult.IsValid)
                {
                    validRecords.Add(item);
                }
                else
                {
                    IList<ValidationFailure> failures = validationResult.Errors;
                    _logger.LogInformation("Validation Errors for {0}", item.ORDER_ID);
                    foreach (var error in failures)
                    {
                        _logger.LogInformation(error.ToString());
                    }
                }
            }
            var uniqueRecords = validRecords.GroupBy(x => new { x.ORDER_ID })
                                  .Select(y => y.First())
                                  .GroupBy(x => new { x.PRODUCT_ID })
                                  .Select(y => y.First())
                                  .GroupBy(x => new { x.CUSTOMER_ID })
                                  .Select(y => y.First())
                                  .ToList();
           
            foreach (var item in uniqueRecords)
            {
                Console.WriteLine("{0} - {1} - {2}", item.ORDER_ID, item.PRODUCT_ID, item.CUSTOMER_ID);
            }
            return uniqueRecords;
        }
        private void ImportData(DBSetting dbSetting, IEnumerable<StoreOrder> records)
        {
            if (dbSetting != null)
            {
                using (var bulkCopy = new SqlBulkCopy(dbSetting.ConnectionString, SqlBulkCopyOptions.Default))
                {
                    bulkCopy.BatchSize = 100;
                    bulkCopy.DestinationTableName = dbSetting.TableName;
                    try
                    {
                        var dt = records.AsDataTable();
                        bulkCopy.WriteToServer(dt);
                    }
                    catch (SqlException)
                    {
                        _logger.LogInformation("SQL Operation failed in ImportData()");
                        throw;
                    }
                    catch (System.Exception)
                    {
                        _logger.LogInformation("Exception in ImportData()");
                        throw;
                    }
                }

            }
        }
    }

    public static class ServiceExtensions
    {
        public static DataTable AsDataTable<T>(this IEnumerable<T> data)
        {
            var properties = TypeDescriptor.GetProperties(typeof(T));
            var table = new DataTable();
            foreach (PropertyDescriptor prop in properties)
                table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
            foreach (T item in data)
            {
                DataRow row = table.NewRow();
                foreach (PropertyDescriptor prop in properties)
                    row[prop.Name] = prop.GetValue(item) ?? DBNull.Value;
                table.Rows.Add(row);
            }
            return table;
        }
    }
}