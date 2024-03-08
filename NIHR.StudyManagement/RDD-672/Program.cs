using Dapper;
using MySqlConnector;
using OfficeOpenXml;
using System.Text;

namespace RDD_672
{
    internal class Program
    {
        static string inputPath = @"C:\Users\pgadz\Downloads\realms.xlsx";
        static string outPath = @"C:\Users\pgadz\Downloads\realms-output.csv";

        static void Main(string[] args)
        {
            //var data = File.ReadLines(@"C:\Users\pgadz\Downloads\realms.tsv")
            //               .Skip(1)
            //               .Select(x => x.Split(new char[] { '\t' }, StringSplitOptions.RemoveEmptyEntries))
            //               .SelectMany(k => k);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            V4();
        }

        private static void insert(List<RealmsRow> realmsRows)
        {
            using (var connection = new MySqlConnection("server=nihrd-rds-aurora-sandbox-study-management.cluster-cyufumnedrbx.eu-west-2.rds.amazonaws.com;database=spike_analysis;user=admin;password=e0FqP|Q8UWIqaP+V:eRSK:Dq::x|"))
            {
                var sql = "INSERT INTO realms (NETSCCID, IRAS, Column1, Programme, FundingStream, SubmissionDate, FundDecisionDate, CurrentStartDate, CurrentEndDate, Status, ResearchType,Title, ShortTitle, ChiefInvestigatorFormal, Contractor, CurrentCost) VALUES (@NETSCCID, @IRAS, @Column1, @Programme, @FundingStream, @SubmissionDate, @FundDecisionDate, @CurrentStartDate, @CurrentEndDate, @Status, @ResearchType,@Title, @ShortTitle, @ChiefInvestigatorFormal, @Contractor, @CurrentCost)";
                var rowsAffected = connection.Execute(sql, realmsRows);
                Console.WriteLine($"{rowsAffected} row(s) inserted.");
            }
        }

        private static List<RealmsRow> GetRealmsRows()
        {
            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(inputPath)))
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

        private static void V5()
        {

        }

        private static void V4()
        {


            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(inputPath)))
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

            using (ExcelPackage xlPackage = new ExcelPackage(new FileInfo(inputPath)))
            {
                using (var outFile = File.OpenWrite(outPath))
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
            using (var file = new StreamReader(inputPath))
            {
                using (var outFile = File.OpenWrite(outPath))
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
            using (var file = new StreamReader(inputPath))
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
