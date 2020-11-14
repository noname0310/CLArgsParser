namespace CLArgsParser.Args
{
    public struct ConversionSpecifier
    {
        public string Key { get; init; }
        public string Value { get; init; }

        public ConversionSpecifier(string value) : this(value, value) { }

        public ConversionSpecifier(string key, string value) => (Key, Value) = (key, value);
    }
}
