using Dapper;
using MySqlConnector;
using OfficeOpenXml;
using System.Text;

namespace RDD_672
{
    internal class Program
    {
        static string realmsInputFile = @"C:\Users\pgadz\Downloads\realms.xlsx";
        static string realmsOutputFile = @"C:\Users\pgadz\Downloads\realms-output.csv";
        static string cpmsInputFile = @"C:\Users\pgadz\Downloads\cpms.xlsx";
        static string cpmsOutputFile = @"C:\Users\pgadz\Downloads\cpms-output.csv";
        const string password = "LXKew.RXIydvqzCQc_uo0khH3rWs";

        static async Task Main(string[] args)
        {
            //var data = File.ReadLines(@"C:\Users\pgadz\Downloads\realms.tsv")
            //               .Skip(1)
            //               .Select(x => x.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries))
            //               .SelectMany(k => k);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var program = new Program();

            await program.ComputeExmaple4_Parallel();
        }

        private static void insert(List<RealmsRow> realmsRows)
        {
            using (var connection = new MySqlConnection($"server=nihrd-rds-aurora-sandbox-study-management.cluster-cyufumnedrbx.eu-west-2.rds.amazonaws.com;database=spike_analysis;user=admin;password={password}"))
            {
                var sql = "INSERT INTO realms (NETSCCID, IRAS, Column1, Programme, FundingStream, SubmissionDate, FundDecisionDate, CurrentStartDate, CurrentEndDate, Status, ResearchType,Title, ShortTitle, ChiefInvestigatorFormal, Contractor, CurrentCost) VALUES (@NETSCCID, @IRAS, @Column1, @Programme, @FundingStream, @SubmissionDate, @FundDecisionDate, @CurrentStartDate, @CurrentEndDate, @Status, @ResearchType,@Title, @ShortTitle, @ChiefInvestigatorFormal, @Contractor, @CurrentCost)";
                var rowsAffected = connection.Execute(sql, realmsRows);
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }

        private static void insert(List<CpmsRow> cpmsRows)
        {
            using (var connection = new MySqlConnection($"server=nihrd-rds-aurora-sandbox-study-management.cluster-cyufumnedrbx.eu-west-2.rds.amazonaws.com;database=spike_analysis;user=admin;password={password}"))
            {
                var sql = "INSERT INTO cpms (  `CPMS ID`,`IRAS ID` ,`Study Record Status` ,`Lead Admin` ,`Commercial Study` ,`Short Name` ,`Title` ,`Study Status` ,`Planned Opening Date` ,`Actual Opening Date` ,`Planned Closure Date` ,`Actual Closure Date` ,`CI Name` ,`CI Email` ,`CPMS Created Date` ,`Funder Name` ,`Funding Stream Name` ,`Grant Code`   ) VALUES (  @CPMSID,@IRASID ,@StudyRecordStatus ,@LeadAdmin ,@CommercialStudy ,@ShortName ,@Title ,@StudyStatus ,@PlannedOpeningDate ,@ActualOpeningDate ,@PlannedClosureDate ,@ActualClosureDate ,@CIName ,@CIEmail ,@CPMSCreatedDate ,@FunderName ,@FundingStreamName ,@GrantCode )";
                var rowsAffected = connection.Execute(sql, cpmsRows);
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }

        private static void insert (List<MatchPair> matchPairs)
        {
            using (var connection = new MySqlConnection($"server=nihrd-rds-aurora-sandbox-study-management.cluster-cyufumnedrbx.eu-west-2.rds.amazonaws.com;database=spike_analysis;user=admin;password={password}"))
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

        private async Task ComputeExmaple4_Parallel()
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

        //private async Task ComputeExmaple4_Parallel()
        //{
        //    var realmsRows = GetRealmsRows();
        //    var cpmsRows = GetCPMSRows();

        //    var batch1RowsToMatch = realmsRows.Take(200);
        //    var batch2RowsToMatch = realmsRows.Skip(200).Take(200);
        //    var batch3RowsToMatch = realmsRows.Skip(400).Take(200);
        //    var batch4RowsToMatch = realmsRows.Skip(600).Take(200);
        //    var batch5RowsToMatch = realmsRows.Skip(800).Take(200);

        //    var batch1Task = Task.Run(() => GetMatchPairs(batch1RowsToMatch.ToList(), cpmsRows));
        //    var batch2Task = Task.Run(() => GetMatchPairs(batch2RowsToMatch.ToList(), cpmsRows));
        //    var batch3Task = Task.Run(() => GetMatchPairs(batch3RowsToMatch.ToList(), cpmsRows));
        //    var batch4Task = Task.Run(() => GetMatchPairs(batch4RowsToMatch.ToList(), cpmsRows));
        //    var batch5Task = Task.Run(() => GetMatchPairs(batch5RowsToMatch.ToList(), cpmsRows));

        //    await Task.WhenAll(batch1Task, batch2Task, batch3Task, batch4Task, batch5Task);

        //    var results = new List<MatchPair>();

        //    results.AddRange(batch1Task.Result);
        //    results.AddRange(batch2Task.Result);
        //    results.AddRange(batch3Task.Result);
        //    results.AddRange(batch4Task.Result);
        //    results.AddRange(batch5Task.Result);
        //}

        private static void ComputeExample3()
        {
            var realmsRows = GetRealmsRows();
            var cpmsRows = GetCPMSRows();

            var matchPairs = new List<MatchPair>();

            var matchedResults = new List<Tuple<RealmsRow, CpmsRow>> ();

            foreach (var realmsRow in realmsRows)
            {
                var lowestMatch = -1;
                CpmsRow matchedCpmsRow = null;

                foreach (var cpmsRow in cpmsRows)
                {
                    var match = (Compute(realmsRow.Title, cpmsRow.Title));

                    if(lowestMatch == -1)
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
                if (matchedCpmsRow == null
                    || percentageOfTotal > 50.00M)
                {
                    //Console.WriteLine($"Realms row not matched {realmsRow.NETSCCID}");
                    //continue;
                }

                var matchPair = new MatchPair
                {
                    CpmsRow = matchedCpmsRow,
                    RealmsRow = realmsRow,
                    Distance = lowestMatch,
                    PercentageOfLength = percentageOfTotal
                };

                matchPairs.Add(matchPair);

                //matchedResults.Add(new Tuple<RealmsRow, CpmsRow>(realmsRow, matchedCpmsRow));
                //System.Diagnostics.Debug.Print($"Realms row matched {realmsRow.NETSCCID} with {matchedCpmsRow?.CPMSID} at distance {lowestMatch} length {realmsRow.Title.Length} with percentage {percentageOfTotal}");
                System.Diagnostics.Debug.Print($"{realmsRow.NETSCCID} {matchedCpmsRow?.CPMSID} {lowestMatch} {matchPair.IrasIdMatch} {matchPair.CiMatch} {percentageOfTotal}");
            }


            Console.WriteLine($"Of {realmsRows.Count}, {matchedResults.Count} were matched in probability tolerance.");
        }

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

        private static void V6_LoadAndMatch()
        {
            var realmsRows = GetRealmsRows();

            var cpmsRows = GetCPMSRows();

            //var matches = MatchOnCI(cpmsRows, realmsRows); // From 44 to 155
            var matches = MatchOnCI(cpmsRows, realmsRows); // From 

            Console.WriteLine($"{realmsRows.Count} RealmsRows, {cpmsRows.Count} CPMS rows. {matches.Count} matches.");

            //foreach (var match in matches)
            //{
            //    Console.WriteLine($"{match.Item1.IRAS} {match.Item2.IRASID}");
            //}
        }

        private static List<Tuple<RealmsRow, CpmsRow>> GetMatches(List<CpmsRow> cpmsRows, List<RealmsRow> realmsRows)
        {
            //var matches = from realms in realmsRows
            //              join cpms in cpmsRows on realms.IRAS equals cpms.IRASID
            //              select new Tuple<RealmsRow, CpmsRow>(realms, cpms);
            var matches = from realms in realmsRows
                          from cpms in cpmsRows
                          where SanitiseIrasId(cpms.IRASID) == SanitiseIrasId(realms.IRAS)
                          select new Tuple<RealmsRow, CpmsRow>(realms, cpms);


            return matches.ToList();
        }

        private static List<Tuple<RealmsRow, CpmsRow>> MatchOnCI(List<CpmsRow> cpmsRows, List<RealmsRow> realmsRows)
        {
            var matches = from realms in realmsRows
                          from cpms in cpmsRows
                          where SanitisePersonName(cpms.CIName).Equals(SanitisePersonName(realms.ChiefInvestigatorFormal), StringComparison.OrdinalIgnoreCase)
                          select new Tuple<RealmsRow, CpmsRow>(realms, cpms);


            return matches.ToList();
        }

        private static List<Tuple<RealmsRow, CpmsRow>> MatchOnShortTitle(List<CpmsRow> cpmsRows, List<RealmsRow> realmsRows)
        {
            var matches = from realms in realmsRows
                          from cpms in cpmsRows
                          where cpms.ShortName.Equals(realms.ShortTitle, StringComparison.OrdinalIgnoreCase)
                          select new Tuple<RealmsRow, CpmsRow>(realms, cpms);


            return matches.ToList();
        }

        private static string SanitisePersonName(string personName)
        {
            // NOT IN USE
            var sanitised = personName
                .Replace("Professor", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Associate", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Prof", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Mr", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Mrs", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Doctor", "", StringComparison.OrdinalIgnoreCase)
                .Replace("-", "", StringComparison.OrdinalIgnoreCase)
                .Replace("Dr", "", StringComparison.OrdinalIgnoreCase)
                .Trim();

            if(sanitised.StartsWith("ms ", StringComparison.OrdinalIgnoreCase))
            {
                sanitised = sanitised.Substring(2);
            }

            return sanitised.Trim();
        }

        private static List<Tuple<RealmsRow, CpmsRow>> MatchOnTitle(List<CpmsRow> cpmsRows, List<RealmsRow> realmsRows)
        {
            var matches = from realms in realmsRows
                          from cpms in cpmsRows
                          where cpms.Title.Equals(realms.Title, StringComparison.OrdinalIgnoreCase)
                          select new Tuple<RealmsRow, CpmsRow>(realms, cpms);


            return matches.ToList();
        }

        private static string SanitiseIrasId(string irasId)
        {
            return irasId
                    .Replace("IRAS", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("Project ID", "", StringComparison.OrdinalIgnoreCase)
                    .Replace(":", "", StringComparison.OrdinalIgnoreCase);
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

        private static void V5_LoadAndInsert()
        {
            var realmsRows = GetRealmsRows();

            insert(realmsRows);

            var cpmsRows = GetCPMSRows();

            insert(cpmsRows);
        }

        private static void V4()
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
                        Column1= GetValue(myWorksheet, rowNum, 3),
                        Programme = GetValue(myWorksheet, rowNum, 4),
                        FundingStream = GetValue(myWorksheet, rowNum, 5),
                        SubmissionDate= GetValue(myWorksheet, rowNum, 6),
                        FundDecisionDate= GetValue(myWorksheet, rowNum, 7),
                        CurrentStartDate = GetValue(myWorksheet, rowNum, 8),
                        CurrentEndDate= GetValue(myWorksheet, rowNum, 9),
                        Status= GetValue(myWorksheet, rowNum, 10),
                        ResearchType= GetValue(myWorksheet, rowNum, 11),
                        Title= GetValue(myWorksheet, rowNum, 12),
                        ShortTitle= GetValue(myWorksheet, rowNum, 13),
                        ChiefInvestigatorFormal= GetValue(myWorksheet, rowNum, 14),
                        Contractor= GetValue(myWorksheet, rowNum, 15),
                        CurrentCost= GetValue(myWorksheet, rowNum, 16)
                    };

                    realmsRows.Add(realmsRow);


                }

                insert(realmsRows);
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

        private static void V3()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(realmsInputFile)))
            {
                using (var outFile = File.OpenWrite(realmsOutputFile))
                {
                    var outWriter = new StreamWriter(outFile);


                    var myWorksheet = xlPackage.Workbook.Worksheets.First(); //select sheet here
                    var totalRows = myWorksheet.Dimension.End.Row;
                    var totalColumns = myWorksheet.Dimension.End.Column;

                    var sb = new StringBuilder(); //this is your data
                    for (int rowNum = 1; rowNum <= totalRows; rowNum++) //select starting row here
                    {
                        var outputRow = "";
                        for (int cellCount = 1; cellCount <= 16; cellCount++)
                        {

                            var cellValue = myWorksheet.Cells[rowNum, cellCount] != null
                                && myWorksheet.Cells[rowNum, cellCount].Value != null
                                ? myWorksheet.Cells[rowNum, cellCount].Value.ToString()
                                : "null";

                            //cellValue = cellValue.Replace("\"\r\n\"", ",");
                            cellValue = cellValue.Replace(Environment.NewLine, " ");

                            outputRow = string.IsNullOrEmpty(outputRow)
                                ? $"\"{cellValue}\""
                                : $"{outputRow},\"{cellValue}\"";
                        }

                        outWriter.WriteLine(outputRow);

                        //var x1 = myWorksheet.Cells[rowNum, 1].Value.ToString();

                        //var x = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns];
                        //var y = string.Join("|", x.Value.ToString());
                        //var row = myWorksheet.Cells[rowNum, 1, rowNum, totalColumns].Select(c => c.Value == null ? "x" : c.Value.ToString());
                        //sb.AppendLine(string.Join(",", row));
                    }
                }
            }
        }

        private static void V2()
        {
            using (var file = new StreamReader(realmsInputFile))
            {
                using (var outFile = File.OpenWrite(realmsOutputFile))
                {
                    var outWriter = new StreamWriter(outFile);

                    string line;

                    while ((line = file.ReadLine()) != null)
                    {
                        var delimiters = '\t';

                        var segments = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                        var segmentCount = segments.Count();

                        var outputLine = "";

                        foreach (var segment in segments)
                        {
                            outputLine = string.IsNullOrEmpty(outputLine)
                                ? $"{segmentCount}\"{segment}\""
                                : outputLine + $",\"{segment}\"";
                        }

                        outWriter.WriteLine(outputLine);
                    }
                }
            }
        }

        private static void V1()
        {
            var sepList = new List<string>();
            var inputResult = new InputResult();
            var rows = new List<InputResultRow>();

            // Read the file and display it line by line.
            using (var file = new StreamReader(realmsInputFile))
            {
                int lineIndex = 0;

                string line;

                while ((line = file.ReadLine()) != null)
                {
                    var delimiters = '\t';
                    var segments = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);

                    int segmentIndex = 0;

                    if (lineIndex == 0)
                    {
                        foreach (var segment in segments)
                        {
                            inputResult.Headings[segmentIndex] = segment ?? "null";

                            segmentIndex++;
                        }

                        lineIndex++;

                        continue;
                    }

                    var row = new InputResultRow();

                    foreach (var segment in segments)
                    {
                        row.Values[segmentIndex] = segment;

                        segmentIndex++;
                    }

                    rows.Add(row);

                    lineIndex++;
                }
            }
        }
    }

    public class InputResult
    {
        public Dictionary<int, string> Headings { get; set; }

        public InputResult()
        {
            Headings = new Dictionary<int, string>();
        }
    }

    public class InputResultRow
    {
        public Dictionary<int, string> Values { get; set; }

        public InputResultRow()
        {
            Values = new Dictionary<int, string>();
        }
    }

    public class InputResultHeading
    {

    }
}
