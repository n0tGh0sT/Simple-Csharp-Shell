namespace Shpellbook
{
    public class Command
    {
        public readonly string[] args;

        public Command(string[] args)
        {
            this.args = args;
        }

        public override string ToString()
        {
            if (args == null || args.Length == 0)
                return "";

            var res = args[0];
            for (var i = 1; i < args.Length; i++)
                res += ' ' + args[i];

            return res;
        }
    }
}