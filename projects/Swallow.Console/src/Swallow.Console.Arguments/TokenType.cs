namespace Swallow.Console.Arguments;

[Flags]
public enum TokenType
{
    Option = 0b0000_0001,
    OptionValue = 0b0000_0010,
    Parameter = 0b0010_0000,

    ParameterOrOptionValue = OptionValue | Parameter
}
