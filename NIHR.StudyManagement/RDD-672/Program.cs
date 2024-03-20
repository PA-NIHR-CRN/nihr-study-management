using Dapper;
using MySqlConnector;
using OfficeOpenXml;
using System.Text;

namespace RDD_672
{
    internal class Program
    {
        static string realmsInputFile = @"<set to local input path>\realms.xlsx";
        static string cpmsInputFile = @"<set to local input path>\cpms.xlsx";
        const string connectionString = $"server=<set server>;database=<set schema>;user=<set user>;password=<set password>";

        static async Task Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var program = new Program();

            await program.ComputeResultsAsync();
        }

        private static void insert (List<MatchPair> matchPairs)
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                var tableName = $"results{DateTime.Now.ToString("HHmmss")}";
                var sql = $"CREATE TABLE `{tableName}` (`netccid` text,`cpmsid` int DEFAULT NULL,`distance` int DEFAULT NULL,`irasmatch` int DEFAULT NULL,`cimatch` int DEFAULT NULL,`percentage` double DEFAULT NULL) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;";

                connection.Execute(sql);

                sql = $"INSERT INTO {tableName} (`netccid`, `cpmsid`, `distance`, `irasmatch`, `cimatch`, `percentage`) VALUES (@netccid, @cpmsid, @distance, @irasmatchparam, @cimatchparam, @PercentageOfLength)";

                var rowsAffected = connection.Execute(sql, matchPairs);
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }

        private static List<MatchPair> GetMatchPairs(List<RealmsRow> realmsRows, List<CpmsRow> cpmsRows)
        {
            var matchPairs = new List<MatchPair>();

            foreach (var realmsRow in realmsRows)
            {
                var lowestMatch = -1;
                CpmsRow matchedCpmsRow = null;

                foreach (var cpmsRow in cpmsRows)
                {
                    var match = (Compute(realmsRow.Title, cpmsRow.Title));

                    if (lowestMatch == -1)
                    {
                        lowestMatch = match;
                    }
                    else if (match < lowestMatch)
                    {
                        lowestMatch = match;

                        matchedCpmsRow = cpmsRow;
                    }
                }

                var percentageOfTotal = ((decimal)lowestMatch / (decimal)realmsRow.Title.Length) * 100;

                var matchPair = new MatchPair
                {
                    CpmsRow = matchedCpmsRow,
                    RealmsRow = realmsRow,
                    Distance = lowestMatch,
                    PercentageOfLength = percentageOfTotal
                };

                matchPairs.Add(matchPair);
            }

            return matchPairs;
        }

        private async Task ComputeResultsAsync()
        {
            var realmsRows = GetRealmsRows();
            var cpmsRows = GetCPMSRows();
            var taskList = new List<Task<List<MatchPair>>>();

            for (int i = 0; i <= realmsRows.Count; i = i + 100)
            {
                var realmsRowsForBatch = realmsRows.Skip(i).Take(100).ToList();

                taskList.Add(Task.Run(() => GetMatchPairs(realmsRowsForBatch, cpmsRows)));
            }

            await Task.WhenAll(taskList);

            var results = new List<MatchPair>();

            foreach (var taskResult in taskList)
            {
                results.AddRange(taskResult.Result);
            }

            foreach (var result in results)
            {
                System.Diagnostics.Debug.Print($"{result.RealmsRow.NETSCCID} {result.CpmsRow?.CPMSID} {result.Distance} {result.IrasIdMatch} {result.CiMatch} {result.PercentageOfLength}");
            }

            insert(results);
        }

        /// <summary>
        /// https://www.dotnetperls.com/levenshtein
        /// </summary>
        static int Compute(string s, string t)
        {
            s = s.ToUpper();
            t = t.ToUpper();
            int n = s.Length;
            int m = t.Length;
            int[,] d = new int[n + 1, m + 1];

            // Verify arguments.
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Initialize arrays.
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Begin looping.
            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    // Compute cost.
                    int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                    Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                    d[i - 1, j - 1] + cost);
                }
            }
            // Return cost.
            return d[n, m];
        }

        private static List<RealmsRow> GetRealmsRows()
        {
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(realmsInputFile)))
            {


                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                var realmsRows = new List<RealmsRow>();

                for (int rowNum = 2; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    var realmsRow = new RealmsRow()
                    {
                        NETSCCID = GetValue(myWorksheet, rowNum, 1),
                        IRAS = GetValue(myWorksheet, rowNum, 2),
                        Column1 = GetValue(myWorksheet, rowNum, 3),
                        Programme = GetValue(myWorksheet, rowNum, 4),
                        FundingStream = GetValue(myWorksheet, rowNum, 5),
                        SubmissionDate = GetValue(myWorksheet, rowNum, 6),
                        FundDecisionDate = GetValue(myWorksheet, rowNum, 7),
                        CurrentStartDate = GetValue(myWorksheet, rowNum, 8),
                        CurrentEndDate = GetValue(myWorksheet, rowNum, 9),
                        Status = GetValue(myWorksheet, rowNum, 10),
                        ResearchType = GetValue(myWorksheet, rowNum, 11),
                        Title = GetValue(myWorksheet, rowNum, 12),
                        ShortTitle = GetValue(myWorksheet, rowNum, 13),
                        ChiefInvestigatorFormal = GetValue(myWorksheet, rowNum, 14),
                        Contractor = GetValue(myWorksheet, rowNum, 15),
                        CurrentCost = GetValue(myWorksheet, rowNum, 16)
                    };

                    realmsRows.Add(realmsRow);


                }

                return realmsRows;
            }
        }

        private static List<CpmsRow> GetCPMSRows()
        {
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(cpmsInputFile)))
            {


                var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                var totalRows = myWorksheet.Dimension.End.Row;
                var totalColumns = myWorksheet.Dimension.End.Column;

                var realmsRows = new List<CpmsRow>();

                for (int rowNum = 2; rowNum <= totalRows; rowNum++) //select starting row here
                {
                    var realmsRow = new CpmsRow()
                    {
                        CPMSID= GetValue(myWorksheet, rowNum, 1),
                        IRASID = GetValue(myWorksheet, rowNum, 2),
                        StudyRecordStatus = GetValue(myWorksheet, rowNum, 3),
                        LeadAdmin = GetValue(myWorksheet, rowNum, 4),
                        CommercialStudy= GetValue(myWorksheet, rowNum, 5),
                        ShortName = GetValue(myWorksheet, rowNum, 6),
                        Title = GetValue(myWorksheet, rowNum, 7),
                        StudyStatus = GetValue(myWorksheet, rowNum, 8),
                        PlannedOpeningDate = GetValue(myWorksheet, rowNum, 9),
                        ActualOpeningDate= GetValue(myWorksheet, rowNum, 10),
                        PlannedClosureDate = GetValue(myWorksheet, rowNum, 11),
                        ActualClosureDate = GetValue(myWorksheet, rowNum, 12),
                        CIName= GetValue(myWorksheet, rowNum, 13),
                        CIEmail= GetValue(myWorksheet, rowNum, 14),
                        CPMSCreatedDate= GetValue(myWorksheet, rowNum, 15),
                        FunderName= GetValue(myWorksheet, rowNum, 16),
                        FundingStreamName = GetValue(myWorksheet, rowNum, 17),
                        GrantCode= GetValue(myWorksheet, rowNum, 18),

                    };

                    realmsRows.Add(realmsRow);


                }

                return realmsRows;
            }
        }



        private static string GetValue(ExcelWorksheet myWorksheet,
            int rowNum,
            int columnId)
        {
            var cellValue = myWorksheet.Cells[rowNum, columnId] != null
                            && myWorksheet.Cells[rowNum, columnId].Value != null
                            ? myWorksheet.Cells[rowNum, columnId].Value.ToString()
                            : "null";
            return cellValue;
        }
    }

}
