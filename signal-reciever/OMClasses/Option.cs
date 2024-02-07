using System.ComponentModel;
using System;

namespace SignalReciver.Elements
{
    // TODO: inherit from asset, add all the common members to asset, and remove it from here and from class stock
    [ImmutableObject(true)]
    internal sealed class Option : Asset
    {
        private readonly Asset _baseAsset;

        private readonly int _multiplier;

        private readonly DateTime _expiration;
        private readonly DateTime _realExpiration;

        private readonly double _strike;

        private readonly OptionContractType _right;

        internal Option(
            int id,
            string symbol,
            Asset baseAsset,
            string currency,
            string primaryExch,
            string exchange,
            int multiplier,
            DateTime expiration,
            double strike,
            OptionContractType right,
            string description = "")
            : this(id, symbol, baseAsset, currency, primaryExch, exchange, multiplier, expiration, expiration, strike, right, description)
        {
        }

        internal Option(
            int id,
            string symbol,
            Asset baseAsset,
            string currency,
            string primaryExch,
            string exchange,
            int multiplier,
            DateTime expiration,
            DateTime realExpiration,
            double strike,
            OptionContractType right,
            string description = "")
            : base(id, symbol, description, currency, primaryExch, exchange)
        {
            _multiplier = multiplier;
            _expiration = expiration;
            _realExpiration = realExpiration;
            _strike = strike;
            _right = right;
            _baseAsset = baseAsset;
        }



        internal int Multiplier => _multiplier;

        internal DateTime Expiration => _expiration;
        internal DateTime RealExpiration => _realExpiration;

        internal double Strike => _strike;

        internal Asset BaseAsset => _baseAsset;

        internal OptionContractType ContractType => _right;

        internal OptionExpirationMode OptionExpirationMode => _expiration == _realExpiration ? OptionExpirationMode.PM : OptionExpirationMode.AM;

        public override string ToString()
        {
            return Symbol;
        }
    }
}