namespace Domain.RequestFeatures
{
    public class WorkTaskParameters : RequestParameters
    {
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public bool ValidDateRange => !MinDate.HasValue ||
            !MaxDate.HasValue || 
            MaxDate >= MinDate;
    }
}
