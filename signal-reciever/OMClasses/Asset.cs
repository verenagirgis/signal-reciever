using System.ComponentModel;

namespace SignalReciver.Elements
{
    [ImmutableObject(true)]
    internal abstract class Asset
    {
        private readonly string _primaryExch;

        private readonly string _currency;

        private readonly string _exchange;


        private readonly int _id;

        private readonly string _symbol;
        private readonly string _description;
        private readonly double _usdRatio;

        internal Asset(int id, string symbol, string description,
            string currency,
            string primaryExch,
            string exchange,
             double usdRatio = 1)
        {
            _id = id;
            _symbol = symbol.ToUpper();
            _description = description;
            _usdRatio = usdRatio;
            _primaryExch = primaryExch;
            _currency = currency;
            _exchange = exchange ?? "SMART";
        }

        internal string PrimaryExch => _primaryExch;

        internal string Exchange => _exchange;

        internal string Currency => _currency;

        internal int Id => _id;

        internal string Symbol => _symbol;

        internal double UsdRatio => _usdRatio;

        internal string Description => _description;

        public override string ToString()
        {
            return _symbol;
        }

        public override bool Equals(object other)
        {
            return _symbol.Equals(((Asset)other)._symbol);
        }

        public override int GetHashCode()
        {
            return _symbol.GetHashCode();
        }
    }
}