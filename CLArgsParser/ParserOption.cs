namespace CLArgsParser
{
    public class ParserOption<T>
    {
        public bool Enable { get; set; }
        public T Value { get; set; }

        public ParserOption(bool enable, T value)
        {
            Enable = enable;
            Value = value;
        }
    }
}
