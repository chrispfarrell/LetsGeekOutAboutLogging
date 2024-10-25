using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Collections.ObjectModel;
using System.Data;
using SynergisticLogging.Web.Logging.ActionLogger;
using Serilog.Events;
using ILogger = Serilog.ILogger;
using SynergisticLogging.Web.External;
using SynergisticLogging.Web.Framework;

namespace SynergisticLogging.Web.Logging
{
    public class MyLogger
    {
        private readonly ILogger _actionLogger;
        private readonly ILogger _apiLogger;
        private readonly IDateTimeService _dateTimeService;

        public MyLogger(IConfiguration configuration,IDateTimeService dateTimeService)
        {
            _dateTimeService = dateTimeService;
            _actionLogger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    "Actions",
                    autoCreateSqlTable: true,
                    columnOptions: GetSqlColumnOptionsActions(),
                    batchPostingLimit: 1,
                    schemaName: "Log"
                )
                .CreateLogger();

            _apiLogger = new LoggerConfiguration()
                .WriteTo.MSSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    "ApiHistory",
                    autoCreateSqlTable: true,
                    columnOptions: GetSqlColumnOptionsApiHistory(),
                    batchPostingLimit: 1,
                    schemaName: "Log"
                )
                .CreateLogger();
        }

        private static ColumnOptions GetSqlColumnOptionsActions()
        {
            var colOptions = new ColumnOptions();
            colOptions.Store.Remove(StandardColumn.Properties);
            colOptions.Store.Remove(StandardColumn.MessageTemplate);
            colOptions.Store.Remove(StandardColumn.Message);
            colOptions.Store.Remove(StandardColumn.Exception);
            colOptions.Store.Remove(StandardColumn.TimeStamp);
            colOptions.Store.Remove(StandardColumn.Level);

            colOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new () {DataType = SqlDbType.DateTime, ColumnName = "DateTime" },
                new () {DataType = SqlDbType.VarChar, DataLength = 10,ColumnName = "Verb"},
                new () {DataType = SqlDbType.VarChar, DataLength = 100,ColumnName = "Controller"},
                new () {DataType = SqlDbType.VarChar, DataLength = 100,ColumnName = "Action"},
                new () {DataType = SqlDbType.VarChar, ColumnName = "ModelArguments"},
                new () {DataType = SqlDbType.VarChar, DataLength = 20, ColumnName = "RemoteIpAddress"},
                new () {DataType = SqlDbType.VarChar, DataLength = 100,ColumnName = "UserName"},
                new () {DataType = SqlDbType.Int, ColumnName = "ElapsedMilliseconds"},
                new () {DataType = SqlDbType.VarChar, ColumnName = "Exception"}
            };

            return colOptions;
        }

        private static ColumnOptions GetSqlColumnOptionsApiHistory()
        {
            var colOptions = new ColumnOptions();
            colOptions.Store.Remove(StandardColumn.Properties);
            colOptions.Store.Remove(StandardColumn.MessageTemplate);
            colOptions.Store.Remove(StandardColumn.Message);
            colOptions.Store.Remove(StandardColumn.Exception);
            colOptions.Store.Remove(StandardColumn.Level);
            
            colOptions.AdditionalColumns = new Collection<SqlColumn>
            {
                new () {DataType = SqlDbType.NVarChar, ColumnName = "Url"},
                new () {DataType = SqlDbType.NVarChar, ColumnName = "Verb"},
                new () {DataType = SqlDbType.Int, ColumnName = "StatusCode"},
                new () {DataType = SqlDbType.NVarChar, ColumnName = "ResponseBody"},
                new () {DataType = SqlDbType.Int, ColumnName = "ElapsedMilliseconds"},
                new () {DataType = SqlDbType.NVarChar, ColumnName = "CorrelationId"},
                new () {DataType = SqlDbType.VarChar, ColumnName = "Exception"}
            };

            return colOptions;
        }

        public void LogAction(ActionLog log)
        {
            _actionLogger?.Write(LogEventLevel.Information,
                "{DateTime}{Verb}{Controller}{Action}{ModelArguments}{RemoteIpAddress}{UserName}{ElapsedMilliseconds}{Exception}",
                DateTime.Now,
                log.Verb, 
                log.Controller, 
                log.Action, 
                log.ModelArguments, 
                log.RemoteIpAddress, 
                log.UserName,
                log.ElapsedMilliseconds, 
                log.Exception?.ToBetterString()
            );
        }

        public void WriteApiHistory(ApiResponse infoToLog)
        {
            _apiLogger.Write(LogEventLevel.Information,
                "{Timestamp}{ElapsedMilliseconds}{Exception}{CorrelationId}{Url}{Verb}{StatusCode}{ResponseBody}",
                _dateTimeService.Now(),
                infoToLog.ElapsedMilliseconds,
                infoToLog.Exception?.ToBetterString(),
                infoToLog.CorrelationId,
                infoToLog.Url,
                infoToLog.Verb,
                infoToLog.StatusCode,
                infoToLog.ResponseBody
            );
        }
        
    }
}
