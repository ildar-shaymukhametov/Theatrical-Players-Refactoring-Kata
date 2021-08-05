﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementPrinter
    {
        public string Print(Invoice invoice, Dictionary<string, Play> plays)
        {
            var totalAmount = 0;
            var volumeCredits = 0;
            var result = string.Format("Statement for {0}\n", invoice.Customer);
            CultureInfo cultureInfo = new CultureInfo("en-US");

            foreach(var perf in invoice.Performances)
            {
                volumeCredits += GetVolumeCredit(plays, perf);

                // print line for this order
                result += String.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", GetPlay(plays, perf).Name, Convert.ToDecimal(GetAmount(perf, GetPlay(plays, perf)) / 100), perf.Audience);
                totalAmount += GetAmount(perf, GetPlay(plays, perf));
            }
            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", Convert.ToDecimal(totalAmount / 100));
            result += String.Format("You earned {0} credits\n", volumeCredits);
            return result;
        }

        private static int GetVolumeCredit(Dictionary<string, Play> plays, Performance perf)
        {
            var result = Math.Max(perf.Audience - 30, 0);
            if ("comedy" == GetPlay(plays, perf).Type)
            {
                result += (int)Math.Floor((decimal)perf.Audience / 5);
            }
            return result;
        }

        private static Play GetPlay(Dictionary<string, Play> plays, Performance perf)
        {
            return plays[perf.PlayID];
        }

        private static int GetAmount(Performance performance, Play play)
        {
            var result = 0;
            switch (play.Type)
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
                    throw new Exception("unknown type: " + play.Type);
            }

            return result;
        }
    }
}
