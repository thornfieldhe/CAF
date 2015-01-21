using System;
using System.Collections.Generic;
using Csla.Rules;
using Csla.Rules.CommonRules;

namespace CAF
{
    public class GuidRequired : CommonBusinessRule
    {
        /// <summary>
        /// Creates an instance of the rule.
        /// </summary>
        /// <param name="primaryProperty">Property to which the rule applies.</param>
        public GuidRequired(Csla.Core.IPropertyInfo primaryProperty)
            : base(primaryProperty)
        {
            InputProperties = new List<Csla.Core.IPropertyInfo>();
            InputProperties.Add(primaryProperty);
        }

        public GuidRequired(Csla.Core.IPropertyInfo primaryProperty, string message)
            : this(primaryProperty)
        {
            MessageText = message;
        }

        public GuidRequired(Csla.Core.IPropertyInfo primaryProperty, Func<string> messageDelegate)
            : this(primaryProperty)
        {
            MessageDelegate = messageDelegate;
        }

        protected override string GetMessage()
        {
            return HasMessageDelegate ? base.MessageText : Csla.Properties.Resources.StringRequiredRule;
        }

        protected override void Execute(RuleContext context)
        {
            var value = context.InputPropertyValues[PrimaryProperty];
            if (value == null || (Guid)value == new Guid())
            {
                var message = string.Format(GetMessage(), PrimaryProperty.FriendlyName);
                context.Results.Add(new RuleResult(RuleName, PrimaryProperty, message) { Severity = Severity });
            }
        }
    }
}