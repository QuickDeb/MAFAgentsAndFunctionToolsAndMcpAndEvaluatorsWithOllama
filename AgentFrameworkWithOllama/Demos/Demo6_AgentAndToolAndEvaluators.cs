using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using ModelContextProtocol.Client;
using ModelContextProtocol;
using OllamaSharp;
using OpenAI;
using OpenAI.Chat;
using OpenAI.Responses;
using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.AI.ContentSafety;
using Microsoft.Extensions.Azure;
using System.Security;
using Microsoft.Extensions.AI.Evaluation;
using Microsoft.Extensions.Hosting;

namespace AgentFrameworkWithOllama.Demos
{
    public static class Demo6_AgentAndToolAndEvaluators
    {
        public static async Task RunAsync()
        {
            try
            {
                var builder = Host.CreateApplicationBuilder();
                builder.AddServiceDefaults();
                


                    // Configure your own HttpClient with a larger timeout
                    var httpClient = new HttpClient
                    {
                        BaseAddress = new Uri("http://localhost:11434"),
                        Timeout = TimeSpan.FromMinutes(30) // 10 minutes timeout
                    };

                    // Pass it into the OllamaApiClient
                    var chatClient = new OllamaApiClient(httpClient, "qwen2.5-coder:1.5b");

                    // Build MAF agent with function invocation middleware bridge
                    var blogAgent = new ChatClientAgent(
                        chatClient,
                        //tools: [.. discoveredTools.Cast<AITool>()],
                        tools : [SendEmailTool.EmailTool()],
                        name: "BlogAgent",
                        instructions: "You are a blog expert that writes and emails blogs.",
                        description: "A blog agent that writes blogs and emails them"
                    ).AsBuilder()
                     .UseToolApproval(new ToolApprovalAgentOptions()
                     {
                         AutoApprovalRules = [AgentSkillsProvider.AllToolsAutoApprovalRule]
                     })
                     .Build();

                var app = builder.Build();
                app.Start();
                

                    var localEval = new LocalEvaluator
                        (EvalChecks.KeywordCheck("Barcelona", "Catalonia"), 
                        EvalChecks.ToolCalledCheck("Send_an_email"));
                    
                    string[] text = ["""
                                    Write about the history of Barcelona in one paragraph. Send an email of the text to debs@yahoo.com.
                                    Include the paragraph in your response.
                                    """];

                    AgentEvaluationResults results = await blogAgent.EvaluateAsync(text, localEval);
                    Console.WriteLine($"{results.Passed} passed out of {results.Total} total");


                    foreach (var item in results.Items) 
                    {
                        foreach (var c in item.Metrics) 
                        {
                            var output = c.Value is NumericMetric mm && mm.Value.HasValue ? mm.Value.Value.ToString("F1") :
                             (c.Value.Interpretation?.Failed ?? true ) ? "Failed": "OK";
                        }                    
                    }                    
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
        }        
    }
}
