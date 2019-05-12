namespace BankApp.Domain.Entities
{
    public class Account
    {
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int Id { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
    }
}