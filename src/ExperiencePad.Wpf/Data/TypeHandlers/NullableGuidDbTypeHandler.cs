using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ExperiencePad.Data
{
    public class NullableGuidDbTypeHandler : SqlMapper.TypeHandler<Guid?>
    {
        public override void SetValue(IDbDataParameter parameter, Guid? guid)
        {
            parameter.Value = guid.HasValue
                ? guid.Value.ToString()
                : null;
        }

        public override Guid? Parse(object value)
        {
            return value == null
                ? null
                : (Guid?)new Guid((string)value);
        }
    }
}
