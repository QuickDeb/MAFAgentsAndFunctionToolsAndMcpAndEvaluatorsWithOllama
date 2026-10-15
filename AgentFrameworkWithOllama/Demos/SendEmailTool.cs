using Microsoft.Agents.AI.Workflows;
using Microsoft.Extensions.AI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentFrameworkWithOllama.Demos
{
    public static class SendEmailTool
    {   
        [Description("Sends email to a xpcified email address")]
        public static string sendEmail([Description("Email address of the recipient")]string emailAddress, [Description("Email content")] string emailcontent)
        {
            return $"Email to {emailAddress} sent.";
        }

        public static AIFunction EmailTool() => AIFunctionFactory.Create(sendEmail, "Send_an_email");

    }
}
