﻿namespace Swallow.ChainOfInjection.Test;

internal interface IChainMember { }
internal sealed record ChainingMember(IChainMember Next) : IChainMember;
internal sealed record TerminatingMember : IChainMember;

internal sealed record MemberWithoutPublicConstructor : IChainMember
{
    private MemberWithoutPublicConstructor() { }
}

internal sealed record MemberWithMultipleConstructors : IChainMember
{
    public MemberWithMultipleConstructors(string first) { }

    public MemberWithMultipleConstructors(string first, string second) { }
}

internal sealed record MemberThatIsNotAChainMember;
internal sealed record MemberThatAlsoRequiresANonMember(IChainMember Next, string SomeString) : IChainMember;
