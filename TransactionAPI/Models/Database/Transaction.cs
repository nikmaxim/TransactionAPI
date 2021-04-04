using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionAPI.Models.Database
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; } //The unique identifier
        public string Status { get; set; } //Stores status. Usually it's "Completed", "Cancelled" or "Pending"
        public string Type { get; set; } //Stores type. Usually it's "Withdrawal" or "Refill"
        public string ClientName { get; set; } //Stores the client's first and last name
        public double Amount { get; set; } //Stores total amount
        public string UserId { get; set; } //Stores user's id that added this transaction

        public User User { get; set; } //User database table reference

        [NotMapped]
        public string AmountString { get; set; } //Used to format the amount
    }
}
