using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console_Test.Services.Data
{
    public static class SqlQueries
    {
        public const string InsertSingleData =
            "INSERT INTO DataTable (Date, LatinSymbols, RussianSymbols, IntegerNumber, FloatingPointNumber) " +
            "VALUES (@Date, @LatinSymbols, @RussianSymbols, @IntegerNumber, @FloatingPointNumber)";

        public const string InsertMultipleData =
            "INSERT INTO DataTable (Date, LatinSymbols, RussianSymbols, IntegerNumber, FloatingPointNumber) " +
            "VALUES {0}";

        public const string RetrieveData =
            "SELECT * FROM DataTable";

        public const string UpdateData =
            "UPDATE DataTable SET " +
            "Date = @Date, " +
            "LatinSymbols = @LatinSymbols, " +
            "RussianSymbols = @RussianSymbols, " +
            "IntegerNumber = @IntegerNumber, " +
            "FloatingPointNumber = @FloatingPointNumber " +
            "WHERE Id = @Id";

        public const string DeleteData =
            "DELETE FROM DataTable WHERE Id = @Id";

        public const string CreateTable =
            "CREATE TABLE DataTable " +
            "(" +
            "    Id INT PRIMARY KEY IDENTITY(1,1), " +
            "    Date DATETIME, " +
            "    LatinSymbols NVARCHAR(255), " +
            "    RussianSymbols NVARCHAR(255), " +
            "    IntegerNumber BIGINT, " +
            "    FloatingPointNumber FLOAT " +
    ")";

        public const string createProcedureScript = "Exec CalculateSumAndMedian";

        private const string Procedure = "CREATE PROCEDURE CalculateSumAndMedian" +
            "AS" +
            "BEGIN" +
            "DECLARE @SumOfIntegers BIGINT;" +
            "DECLARE @MedianOfFloats FLOAT;" +
            "        SELECT @SumOfIntegers = SUM(DataTable.IntegerNumber)" +
            "        FROM DataTable;" +
            "        SELECT TOP 1 @MedianOfFloats = AVG(Value)" +
            "        FROM" +
            "           (SELECT DataTable.FloatingPointNumber AS Value, ROW_NUMBER() OVER (ORDER BY DataTable.FloatingPointNumber ) AS RowNum" +
            "                     FROM DataTable) AS Temp" +
            "        WHERE RowNum IN ((SELECT COUNT(*) / 2 + 1 FROM DataTable), (SELECT (COUNT(*) + 1) / 2 FROM DataTable), (SELECT (COUNT(*) + 2) / 2 FROM DataTable));" +
            "SELECT @SumOfIntegers AS SumOfIntegers, @MedianOfFloats AS MedianOfFloats;" +
            "END;";

    }
}
