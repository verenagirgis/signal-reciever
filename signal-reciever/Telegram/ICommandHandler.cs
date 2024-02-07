interface ICommandHandler
{
    void PostCommand(string command, params string[] args);
}