using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MSSQL.DIARY.SERVICE.EXCEL
{
    public static class QueryConstants
    {
        public static readonly string UpdateTableExtendedProperty =
           @"EXEC sys.sp_updateextendedproperty N'MS_Description',  @Table_value , N'SCHEMA', @Schema_Name, N'TABLE', @Table_Name";

        public static readonly string InsertTableExtendedProperty =
            @"EXEC sys.sp_addextendedproperty N'MS_Description',  @Table_value , N'SCHEMA', @Schema_Name, N'TABLE', @Table_Name";

        public static readonly string UpdateTableColumnExtendedProperty =
            @"EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=@Column_value,@level0type=N'SCHEMA',@level0name=@Schema_Name,@level1type=N'TABLE',@level1name=@Table_Name,@level2type=N'COLUMN',@level2name=@Column_Name";

        public static readonly string InsertTableColumnExtendedProperty =
            @"EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=@Column_value,@level0type=N'SCHEMA',@level0name=@Schema_Name,@level1type=N'TABLE',@level1name=@Table_Name,@level2type=N'COLUMN',@level2name=@Column_Name";

    }
}
