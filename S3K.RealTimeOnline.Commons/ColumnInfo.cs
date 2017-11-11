using System.ComponentModel.DataAnnotations.Schema;

namespace S3K.RealTimeOnline.Commons
{
    public class ColumnInfo
    {
        [Column("COLUMN_NAME")]
        public string ColumnName { get; set; }

        [Column("ORDINAL_POSITION")]
        public int OrdinalPosition { get; set; }

        [Column("IS_NULLABLE")]
        public string IsNullable { get; set; }

        [Column("DATA_TYPE")]
        public string DataType { get; set; }

        [Column("CHARACTER_MAXIMUM_LENGTH")]
        public int CharacterMaximumLength { get; set; }

        [Column("NUMERIC_PRECISION")]
        public int NumericPrecision { get; set; }

        [Column("NUMERIC_PRECISION_RADIX")]
        public int NumericPrecisionRadix { get; set; }

        [Column("NUMERIC_SCALE")]
        public int NumericScale { get; set; }

        [Column("DATETIME_PRECISION")]
        public int DateTimePrecision { get; set; }
    }
}