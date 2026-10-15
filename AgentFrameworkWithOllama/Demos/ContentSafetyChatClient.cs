using Azure.AI.ContentSafety;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.AI.Evaluation;

//using Microsoft.Extensions.AI.ChatMessage;
//using OpenAI.Chat.ChatMessage;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using ChatMessage = Microsoft.Extensions.AI.ChatMessage;
using Conversations = OpenAI.Conversations;
using Azure;

namespace AgentFrameworkWithOllama.Demos
{
    public class ContentSafetyChatClient : IChatClient
    {
        // Conceptual .NET example of a Safe Chat Client wrapping Ollama

        private readonly IChatClient _ollamaClient;
        private readonly ContentSafetyClient _safetyClient;

        public ContentSafetyChatClient(IChatClient ollamaClient, ContentSafetyClient safetyClient)
        {
            _ollamaClient = ollamaClient;
            _safetyClient = safetyClient;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<ChatResponse> GetResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public object? GetService(Type serviceType, object? serviceKey = null)
        {
            throw new NotImplementedException();
        }

        public IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(IEnumerable<ChatMessage> messages, ChatOptions? options = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }



        //public async Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions options, CancellationToken cancellationToken)
        //{
        //    // 1. Analyze user prompt
        //    var latestPrompt = chatMessages.LastOrDefault()?.Text;
        //    var isSafe = await CheckContentSafetyAsync(latestPrompt);

        //    if (!isSafe)
        //        throw new InvalidOperationException("Prompt violates content safety policy.");

        //    // 2. Call local Ollama model
        //    var response = await _ollamaClient.CompleteAsync(chatMessages, options, cancellationToken);

        //    // 3. Analyze generated response
        //    var isResponseSafe = await CheckContentSafetyAsync(response.Message.Text);
        //    if (!isResponseSafe)
        //        return new ChatCompletion(new ChatMessage(ChatRole.Assistant, "The generated response was blocked due to safety policies."));

        //    return response;
        //}

        //public static bool CheckContentSafety(string text)
        //{
        //    var safetyResult = await ContentSafetyClient.AnalyzeAsync(context.UserMessage);
        //    if (safetyResult.ViolatesPolicy) throw new SecurityException("Blocked by safety policy");
        //    return await next(context);



        //    return true;

        //}


        //public static bool CheckContentSafety(object text)
        //{
        //    var safetyResult = await new ContentSafetyClient(new Uri(""), new Azure.AzureKeyCredential("")).AnalyzeAsync(text);
        //    if (safetyResult.ViolatesPolicy) throw new SecurityException("Blocked by safety policy");
        //    return await next(context);



        //    return true;

        //}



        //public async Task<ChatCompletion> CompleteAsync(IList<ChatMessage> chatMessages, ChatOptions options, CancellationToken cancellationToken)
        //{
        //    // 1. Analyze user prompt
        //    var latestPrompt = chatMessages.LastOrDefault()?.Text;
        //    var isSafe = await CheckContentSafetyAsync(latestPrompt);

        //    if (!isSafe)
        //        throw new InvalidOperationException("Prompt violates content safety policy.");

        //    // 2. Call local Ollama model
        //    var response = await _ollamaClient.GetResponseAsync(chatMessages, options, cancellationToken);

        //    // 3. Analyze generated response
        //    var isResponseSafe = await CheckContentSafetyAsync(response.Text);
        //    if (!isResponseSafe)
        //        return new ChatCompletion(new ChatMessage(ChatRole.Assistant, "The generated response was blocked due to safety policies."));

        //    return response;


        //}


        //public static async Task<bool> CheckContentSafetyAsync(string userPrompt)
        //{
        //    //bool isSafe = (!userPrompt.Contains("blood") || !userPrompt.Contains("sex"));
        //    //// var safetyResult = await CheckContentSafety(context.UserMessage);
        //    ////if (userPromptxt.Contains("blood") || userPromptxt.Contains("sex")) return false;
        //    ////return isSafe;

        //    //if (isSafe) return true;

        //    if (!userPrompt.Contains("blood") || !userPrompt.Contains("sex")) return true;          

        //    return false;

        //}
    }
}
