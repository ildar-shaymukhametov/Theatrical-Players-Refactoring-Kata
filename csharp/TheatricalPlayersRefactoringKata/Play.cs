using System;

namespace TheatricalPlayersRefactoringKata
{
    public class Play
    {
        private string _name;
        private string _type;

        public string Name { get => _name; set => _name = value; }
        public string Type { get => _type; set => _type = value; }

        public Play(string name, string type) {
            this._name = name;
            this._type = type;
        }

        public virtual int CalculateAmount(int audience)
        {
            throw new Exception();
        }

        public virtual int CalculateVolumeCredit(int audience)
        {
            throw new Exception();
        }
    }

    public class Comedy : Play
    {
        public Comedy(string name, string type) : base(name, type)
        {
        }

        public override int CalculateAmount(int audience)
        {
            var result = 30000;
            if (audience > 20)
            {
                result += 10000 + 500 * (audience - 20);
            }
            result += 300 * audience;
            return result;
        }

        public override int CalculateVolumeCredit(int audience)
        {
            var result = Math.Max(audience - 30, 0);
            result += (int)Math.Floor((decimal)audience / 5);
            return result;
        }
    }

    public class Tragedy : Play
    {
        public Tragedy(string name, string type) : base(name, type)
        {
        }

        public override int CalculateAmount(int audience)
        {
            var result = 40000;
            if (audience > 30)
            {
                result += 1000 * (audience - 30);
            }
            return result;
        }

        public override int CalculateVolumeCredit(int audience)
        {
            var result = Math.Max(audience - 30, 0);
            return result;
        }
    }
}
