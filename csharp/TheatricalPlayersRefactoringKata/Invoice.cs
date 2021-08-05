using System.Collections.Generic;
using System.Linq;

namespace TheatricalPlayersRefactoringKata
{
    public class Invoice
    {
        private string _customer;
        private List<Performance> _performances;

        public string Customer { get => _customer; set => _customer = value; }
        public List<Performance> Performances { get => _performances; set => _performances = value; }

        public Invoice(string customer, List<Performance> performance)
        {
            this._customer = customer;
            this._performances = performance;
        }

        public StatementData GetPrintData(Dictionary<string, Play> plays)
        {
            var data = new StatementData();
            data.Customer = Customer;
            data.Performances = Performances.Select(x => EnrichPerfomance(x, plays)).ToList();
            return data;
        }

        private EnrichedPerformance EnrichPerfomance(Performance performance, Dictionary<string, Play> plays)
        {
            return new EnrichedPerformance(performance.PlayID, performance.Audience, GetPlay(plays, performance));
        }

        private static Play GetPlay(Dictionary<string, Play> plays, Performance performance)
        {
            return plays[performance.PlayID];
        }
    }
}
