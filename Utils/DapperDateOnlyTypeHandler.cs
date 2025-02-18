using Dapper;
using System.Data;

namespace SparkApi.Utils
{
    // Need this for Dapper to be able to map to DateOnly property (User class)
    public class DapperDateOnlyTypeHandler : SqlMapper.TypeHandler<DateOnly>
    {
            public override void SetValue(IDbDataParameter parameter, DateOnly date)
                => parameter.Value = date.ToDateTime(new TimeOnly(0, 0));

            public override DateOnly Parse(object value)
                => DateOnly.FromDateTime((DateTime)value);
    }
}
