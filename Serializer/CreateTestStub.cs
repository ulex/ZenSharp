using JetBrains.DocumentModel;
using JetBrains.ReSharper.Feature.Services.CodeCompletion;
using JetBrains.ReSharper.Feature.Services.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.CSharp.CodeCompletion.Infrastructure;
using JetBrains.ReSharper.Feature.Services.Lookup;
using JetBrains.ReSharper.Psi;
using JetBrains.ReSharper.Psi.CSharp;
using JetBrains.TextControl;

namespace Serializer
{
    [Language(typeof(CSharpLanguage))]
    internal class CreateTestStub : ItemsProviderOfSpecificContext<CSharpCodeCompletionContext>
    {
        protected override bool IsAvailable(CSharpCodeCompletionContext context)
        {
            return true;
        }

        protected override bool AddLookupItems(CSharpCodeCompletionContext context, GroupedItemsCollector collector)
        {
            var myLookupItem = new MyLookupItem("TextLong");
            collector.AddAtDefaultPlace(context.LookupItemsFactory.InitializeLookupItem(myLookupItem));
            return true;
        }
    }

    internal class MyLookupItem : TextLookupItem
    {
        public MyLookupItem(string text)
            : base(text, false)
        {
        }

        public override MatchingResult Match(string prefix, ITextControl textControl)
        {
            return new MatchingResult(1, "Another text", 10);
        }
    }
}