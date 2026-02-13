namespace Domain.RequestFeatures
{
    public class WorkTaskParameters : RequestParameters
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool ValidDateRange => MaxDate > MinDate;
    }
}
