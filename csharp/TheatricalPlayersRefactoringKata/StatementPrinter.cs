using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementData
    {
        public string Customer { get; internal set; }
        public List<EnrichedPerformance> Performances { get; internal set; }

        public int GetTotalAmount()
        {
            var result = 0;
            foreach (var perf in Performances)
            {
                result += perf.Amount;
            }

            return result;
        }

        public int GetTotalVolumeCredit()
        {
            var result = 0;
            foreach (var perf in Performances)
            {
                result += perf.VolumeCredit;
            }

            return result;
        }
    }
    
    public class EnrichedPerformance : Performance
    {
        public EnrichedPerformance(string playID, int audience, Play play) : base(playID, audience)
        {
            Play = play;
            Amount = play.CalculateAmount(audience);
            VolumeCredit = play.CalculateVolumeCredit(audience);
        }

        public Play Play { get; }
        public int Amount { get; }
        public int VolumeCredit { get; }
    }

    public class StatementPrinter
    {
        public string Print(StatementData data)
        {
            return GetPlainText(data);
        }

        private static string GetPlainText(StatementData data)
        {
            var result = string.Format("Statement for {0}\n", data.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach (var perf in data.Performances)
            {
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", perf.Play.Name, ToUsd(perf.Amount), perf.Audience);
            }

            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", ToUsd(data.GetTotalAmount()));
            result += String.Format("You earned {0} credits\n", data.GetTotalVolumeCredit());
            return result;
        }

        private static decimal ToUsd(int amount)
        {
            return Convert.ToDecimal(amount / 100);
        }
    }
}
