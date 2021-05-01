namespace MTA.Core.Application.Dtos
{
    public class TopFactionByBankBalanceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long BankBalance { get; set; }
        public int WorkersCount { get; set; }
    }
}