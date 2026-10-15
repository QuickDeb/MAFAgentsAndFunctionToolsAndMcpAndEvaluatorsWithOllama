
using AgentFrameworkWithOllama.Demos;

if (args.Length == 0)
{
    Console.WriteLine("Agent runs......");
    Console.WriteLine();
    Console.WriteLine("Demos:");
    Console.WriteLine("  1  Film agent (Ollama only)");
    Console.WriteLine("  2  Sequential workflow (4 agents)");
    Console.WriteLine("  3  Sequential workflow + function calling tools (2 agents)");
    Console.WriteLine("  4  General agent + function calling tools ");
    Console.WriteLine("  5  Local Agent + MCP ");
    Console.WriteLine("  6  Local Agent + function calling tool + Evaluators ");
    //return;
}

Console.WriteLine();
Console.WriteLine("Type 1 or 2 or 3 or 4 or 5 or 6 to choose a demo....");
int userInput    = Convert.ToInt32(Console.ReadLine());

switch (userInput)
{
    case 1:
        await Demo1_FillmAgent.RunAsync();
        break;
    case 2:
        await Demo2_MultiAgentWorkflow.RunAsync();
        break;
    case 3:
        await Demo3_AgentsAndFunctionTools.RunAsync();
        break;
    case 4:
        await Demo4_GeneralAgentAndFunctionTools.RunAsync();
        break;
    case 5:
        await Demo5_AgentAndMCP.RunAsync();
        break;
    case 6:
        await Demo6_AgentAndToolAndEvaluators.RunAsync();
        break;
    default:
        Console.WriteLine($"Unknown demo: {userInput}");
        break;
}
