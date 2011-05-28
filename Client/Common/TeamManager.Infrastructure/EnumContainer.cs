namespace TeamManager.Infrastructure
{
    public sealed class EnumContainer
    {
        public int EnumValue { get; set; }
        public string EnumDescription { get; set; }
        public object EnumOriginalValue { get; set; }

        public override string ToString()
        {
            return EnumDescription;
        }

        public override bool Equals(object obj)
        {
            return obj != null && EnumValue.Equals(((EnumContainer) obj).EnumValue);
        }

        public override int GetHashCode()
        {
            return EnumValue.GetHashCode();
        }
    }
}