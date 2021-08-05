using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach (var perf in invoice.Performances)
            {
                // print line for this order
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", GetPlay(plays, perf).Name, ToUsd(GetAmount(perf, plays)), perf.Audience);
                totalAmount += GetAmount(perf, plays);
            }

            var volumeCredits = GetTotalVolumeCredit(invoice, plays);

            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", ToUsd(totalAmount));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static int GetTotalVolumeCredit(Invoice invoice, Dictionary<string, Play> plays)
        {
            var result = 0;
            foreach (var perf in invoice.Performances)
            {
                result += GetVolumeCredit(perf, plays);
            }

            return result;
        }

        private static decimal ToUsd(int amount)
        {
            return Convert.ToDecimal(amount / 100);
        }

        private static int GetVolumeCredit(Performance performance, Dictionary<string, Play> plays)
        {
            var result = Math.Max(performance.Audience - 30, 0);
            if ("comedy" == GetPlay(plays, performance).Type)
            {
                result += (int)Math.Floor((decimal)performance.Audience / 5);
            }
            return result;
        }

        private static Play GetPlay(Dictionary<string, Play> plays, Performance performance)
        {
            return plays[performance.PlayID];
        }

        private static int GetAmount(Performance performance, Dictionary<string, Play> plays)
        {
            var result = 0;
            switch (GetPlay(plays, performance).Type)
            {
                case "tragedy":
                    result = 40000;
                    if (performance.Audience > 30)
                    {
                        result += 1000 * (performance.Audience - 30);
                    }
                    break;
                case "comedy":
                    result = 30000;
                    if (performance.Audience > 20)
                    {
                        result += 10000 + 500 * (performance.Audience - 20);
                    }
                    result += 300 * performance.Audience;
                    break;
                default:
                    throw new Exception("unknown type: " + GetPlay(plays, performance).Type);
            }

            return result;
        }
    }
}
