namespace Crosschat.Client.Model.Entities
{
    public class Country
    {
        public Country(int code, string name)
        {
            Code = code;
            Name = name;
        }

        public int Code { get; private set; }

        public string Name { get; private set; }

        public override string ToString()
        {
            return string.Format("+{0} ({1})", Code, Name);
        }
    }
}
