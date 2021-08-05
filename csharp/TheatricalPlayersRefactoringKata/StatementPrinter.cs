﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace TheatricalPlayersRefactoringKata
{
    public class StatementData
    {
        public string Customer { get; internal set; }
        public List<EnrichedPerformance> Performances { get; internal set; }
    }
    
    public class EnrichedPerformance : Performance
    {
        public EnrichedPerformance(string playID, int audience, Play play) : base(playID, audience)
        {
            Play = play;
        }

        public Play Play { get; }
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
                result += string.Format(cultureInfo, "  {0}: {1:C} ({2} seats)\n", perf.Play.Name, ToUsd(GetAmount(perf)), perf.Audience);
            }

            result += String.Format(cultureInfo, "Amount owed is {0:C}\n", ToUsd(GetTotalAmount(data)));
            result += String.Format("You earned {0} credits\n", GetTotalVolumeCredit(data));
            return result;
        }

        private static int GetTotalAmount(StatementData data)
        {
            var result = 0;
            foreach (var perf in data.Performances)
            {
                result += GetAmount(perf);
            }

            return result;
        }

        private static int GetTotalVolumeCredit(StatementData data)
        {
            var result = 0;
            foreach (var perf in data.Performances)
            {
                result += GetVolumeCredit(perf);
            }

            return result;
        }

        private static decimal ToUsd(int amount)
        {
            return Convert.ToDecimal(amount / 100);
        }

        private static int GetVolumeCredit(EnrichedPerformance performance)
        {
            var result = Math.Max(performance.Audience - 30, 0);
            if ("comedy" == performance.Play.Type)
            {
                result += (int)Math.Floor((decimal)performance.Audience / 5);
            }
            return result;
        }

        private static Play GetPlay(Dictionary<string, Play> plays, Performance performance)
        {
            return plays[performance.PlayID];
        }

        private static int GetAmount(EnrichedPerformance performance)
        {
            var result = 0;
            switch (performance.Play.Type)
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
                    throw new Exception("unknown type: " + performance.Play.Type);
            }

            return result;
        }
    }
}
