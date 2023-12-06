using System;
using System.Collections.Generic;
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

    }
}
