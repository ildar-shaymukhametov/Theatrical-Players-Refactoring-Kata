using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

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
                result += perf.GetAmount();
            }

            return result;
        }

        public int GetTotalVolumeCredit()
        {
            var result = 0;
            foreach (var perf in Performances)
            {
                result += perf.GetVolumeCredit();
            }

            return result;
        }
    }
    
    public class EnrichedPerformance : Performance
    {
        public EnrichedPerformance(string playID, int audience, Play play) : base(playID, audience)
        {
            Play = play;
        }

        public Play Play { get; }

        public int GetAmount()
        {
            var result = 0;
            switch (Play.Type)
            {
                case "tragedy":
                    result = 40000;
                    if (Audience > 30)
                    {
                        result += 1000 * (Audience - 30);
                    }
                    break;
                case "comedy":
                    result = 30000;
                    if (Audience > 20)
                    {
                        result += 10000 + 500 * (Audience - 20);
                    }
                    result += 300 * Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + Play.Type);
            }

            return result;
        }

        public int GetVolumeCredit()
        {
            var result = Math.Max(Audience - 30, 0);
            if ("comedy" == Play.Type)
            {
                result += (int)Math.Floor((decimal)Audience / 5);
            }
            return result;
        }
    }

    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var data = new StatementData();
            data.Customer = invoice.Customer;
            data.Performances = invoice.Performances.Select(x => EnrichPerfomance(x, plays)).ToList();
            return GetPlainText(data);
        }

        private EnrichedPerformance EnrichPerfomance(Performance performance, Dictionary<string, Play> plays)
        {
            return new EnrichedPerformance(performance.PlayID, performance.Audience, GetPlay(plays, performance));
        }

        private static string GetPlainText(StatementData data)
        {
            var result = string.Format("Statement for {0}\n", data.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach (var perf in data.Performances)
            {
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", perf.Play.Name, ToUsd(perf.GetAmount()), perf.Audience);
            }

            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", ToUsd(data.GetTotalAmount()));
            result += String.Format("You earned {0} credits\n", data.GetTotalVolumeCredit());
            return result;
        }

        private static decimal ToUsd(int amount)
        {
            return Convert.ToDecimal(amount / 100);
        }

        private static Play GetPlay(Dictionary<string, Play> plays, Performance performance)
        {
            return plays[performance.PlayID];
        }
    }
}
