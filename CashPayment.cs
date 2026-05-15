namespace PaymentSystem
{
    public class CashPayment : Payment
    {
        public CashPayment(double amount) : base(amount) { }

        public override string ProcessPayment() => $"Прийом готівкового платежу на суму {Amount:C2}. Без комісії.";
    }
}
