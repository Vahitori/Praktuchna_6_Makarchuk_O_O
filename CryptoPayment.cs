namespace PaymentSystem
{
    public class CryptoPayment : Payment
    {
        public string WalletAddress { get; set; }

        public CryptoPayment(double amount, string wallet) : base(amount)
        {
            WalletAddress = wallet;
        }

        public override string ProcessPayment() => $"Обробка криптоплатежу (Wallet: {WalletAddress}) на суму {Amount:C2}. Підтвердження мережі...";
    }
}
