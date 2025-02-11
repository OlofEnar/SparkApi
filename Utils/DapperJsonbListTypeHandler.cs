using Dapper;
using Newtonsoft.Json;
using System.Data;

namespace SparkApi.Utils
{
    // JSONB <--> List Dapper type converter

    public class DapperJsonbListTypeHandler<T> : SqlMapper.TypeHandler<List<T>>
    {
        public override List<T> Parse(object value)
        {
            return value is string json ? JsonConvert.DeserializeObject<List<T>>(json) ?? [] : new List<T>();
        }

        public override void SetValue(IDbDataParameter parameter, List<T> value)
        {
            parameter.Value = JsonConvert.SerializeObject(value);
            parameter.DbType = DbType.String;
        }
    }
}
