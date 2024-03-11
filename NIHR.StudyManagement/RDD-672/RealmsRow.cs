using System.Xml.Linq;

namespace RDD_672
{
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
