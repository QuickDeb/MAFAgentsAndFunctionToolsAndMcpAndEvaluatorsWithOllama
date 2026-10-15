var builder = DistributedApplication.CreateBuilder(args);
//builder.AddProject<Projects.AgentFrameworkWithOllama.Demo6_AgentAndToolAndEvaluators>("My_Agent_Tool_App");
builder.AddProject<Projects.AgentFrameworkWithOllama>("MyAgentToolApp");
builder.Build().Run();


