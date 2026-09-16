using Runtime;

namespace VirtualMachine;

public interface IEnvironment
{
    int ReadInt();

    double ReadFloat();

    string ReadString();

    void Print(Value value);
}