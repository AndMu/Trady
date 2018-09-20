﻿using System;
using System.Collections.Generic;
using Trady.Core.Infrastructure;

namespace Trady.Analysis.Backtest
{
    public class Builder
    {
        private readonly IDictionary<IEnumerable<IOhlcv>, int> _weightings;
        private readonly Predicate<IIndexedOhlcv> _buyRule;
        private readonly Predicate<IIndexedOhlcv> _sellRule;
        private readonly decimal _flatExchangeFee;
        private readonly bool _buyInCompleteQuantity;
        private readonly decimal _premium;

        public Builder() : this(null, null, null, 0, true, 1)
        {
        }

        private Builder(
            IDictionary<IEnumerable<IOhlcv>, int> weightings, 
            Predicate<IIndexedOhlcv> buyRule, 
            Predicate<IIndexedOhlcv> sellRule,
            decimal flatExchangeFee,
            bool buyInCompleteQuantity,
            decimal premium)
        {
            _weightings = weightings ?? new Dictionary<IEnumerable<IOhlcv>, int>();
            _buyRule = buyRule;
            _sellRule = sellRule;
            _flatExchangeFee = flatExchangeFee;
            _buyInCompleteQuantity = buyInCompleteQuantity;
            _premium = premium;
        }

        public Builder Add(IEnumerable<IOhlcv> candles, int weighting = 1)
        {
            _weightings.Add(candles, weighting);
            return new Builder(_weightings, _buyRule, _sellRule, _flatExchangeFee, _buyInCompleteQuantity, _premium);
        }

        public Builder FlatExchangeFeeRate(decimal flatExchangeFeeRate)
            => new Builder(_weightings, _buyRule, _sellRule, flatExchangeFeeRate, _buyInCompleteQuantity, _premium);

        public Builder BuyWithAllAvailableCash()
            => new Builder(_weightings, _buyRule, _sellRule, _flatExchangeFee, false, _premium);
        
        public Builder Buy(Predicate<IIndexedOhlcv> rule)
            => new Builder(_weightings, ic => _buyRule == null ? rule(ic) : rule(ic) || _buyRule(ic), _sellRule, _flatExchangeFee, _buyInCompleteQuantity, _premium);

        public Builder Sell(Predicate<IIndexedOhlcv> rule)
            => new Builder(_weightings, _buyRule, ic => _sellRule  == null ? rule(ic) : rule(ic) || _sellRule(ic), _flatExchangeFee, _buyInCompleteQuantity, _premium);

        public Builder Premium(decimal premium)
            => new Builder(_weightings, _buyRule, _sellRule, _flatExchangeFee, _buyInCompleteQuantity, premium);

        public Runner Build()
            => new Runner(_weightings, _buyRule, _sellRule, _flatExchangeFee, _buyInCompleteQuantity, _premium);
    }
}
