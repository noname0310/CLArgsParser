namespace CLArgsParser.Args
{
    public struct ConversionSpecifier
    {
        public readonly string Key;
        public readonly string Value;

        public ConversionSpecifier(string value)
        {
            Key = value;
            Value = value;
        }

        public ConversionSpecifier(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}
