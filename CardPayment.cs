namespace PaymentSystem
{
    public class CardPayment : Payment
    {
        public string CardNumber { get; set; }

        public CardPayment(double amount, string cardNumber) : base(amount)
        {
            CardNumber = cardNumber;
        }

        public override string ProcessPayment() => $"Обробка карткового платежу ({CardNumber}) на суму {Amount:C2}. Комісія 1.5%.";
    }
}
