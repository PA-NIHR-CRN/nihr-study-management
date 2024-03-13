using System.Xml.Linq;

namespace RDD_672
{

    public class MatchPair
    {
        public string netccid
        {
            get
            {
                return this.RealmsRow.NETSCCID;
            }
        }

        public string cpmsid
        {
            get
            {
                return this.CpmsRow?.CPMSID ?? "";
            }
        }

        public int irasmatchparam
        {
            get
            {
                return this.IrasIdMatch ? 1 : 0;
            }
        }

        public int cimatchparam
        {
            get
            {
                return this.CiMatch ? 1 : 0;
            }
        }

        public RealmsRow RealmsRow { get; set; }

        public CpmsRow? CpmsRow { get; set; }

        public int Distance { get; set; }

        public decimal PercentageOfLength { get; set; }

        public bool CiMatch
        {
            get
            {
                if(CpmsRow == null)
                {
                    return false;
                }

                if(SanitisePersonName(RealmsRow.ChiefInvestigatorFormal).Equals(SanitisePersonName(CpmsRow.CIName), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        public bool IrasIdMatch
        {
            get
            {
                if (CpmsRow == null)
                {
                    return false;
                }

                if(SanitiseIrasId(RealmsRow.IRAS).Equals(SanitiseIrasId(CpmsRow.IRASID), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }

                return false;
            }
        }

        private static string SanitisePersonName(string personName)
        {
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

            if (sanitised.StartsWith("ms ", StringComparison.OrdinalIgnoreCase))
            {
                sanitised = sanitised.Substring(2);
            }

            return sanitised.Trim();
        }

        private static string SanitiseIrasId(string irasId)
        {
            return irasId
                    .Replace("IRAS", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("Project", "", StringComparison.OrdinalIgnoreCase)
                    .Replace("ID", "", StringComparison.OrdinalIgnoreCase)
                    .Replace(":", "", StringComparison.OrdinalIgnoreCase)
                    .Trim();
        }
    }

    public class RealmsRow
    {
        public string NETSCCID { get; set; }
        public string IRAS { get; set; }
        public string Column1 { get; set; }
        public string Programme { get; set; }
        public string FundingStream { get; set; }
        public string SubmissionDate { get; set; }
        public string FundDecisionDate { get; set; }
        public string CurrentStartDate { get; set; }
        public string CurrentEndDate { get; set; }
        public string Status { get; set; }
        public string ResearchType { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string ChiefInvestigatorFormal { get; set; }
        public string Contractor { get; set; }
        public string CurrentCost { get; set; }
    }

    public class CpmsRow
    {
        public string CPMSID { get; set; }
        public string IRASID { get; set; }
        public string StudyRecordStatus { get; set; }
        public string LeadAdmin { get; set; }
        public string CommercialStudy { get; set; }
        public string ShortName { get; set; }
        public string Title { get; set; }
        public string StudyStatus { get; set; }
        public string PlannedOpeningDate { get; set; }
        public string ActualOpeningDate { get; set; }
        public string PlannedClosureDate { get; set; }
        public string ActualClosureDate { get; set; }
        public string CIName { get; set; }
        public string CIEmail { get; set; }
        public string CPMSCreatedDate { get; set; }
        public string FunderName { get; set; }
        public string FundingStreamName { get; set; }
        public string GrantCode { get; set; }

    }
}
