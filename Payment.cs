using System;

namespace PaymentSystem
{
    public abstract class Payment
    {
        public double Amount { get; set; }
        public DateTime Date { get; set; }

        protected Payment(double amount)
        {
            Amount = amount;
            Date = DateTime.Now;
        }

        public virtual string ProcessPayment() => $"Обробка платежу на суму {Amount:C2}";
    }
}
