using Runtime;

namespace VirtualMachine.UnitTests;

public class VariablesTableAncestorTest
{
    [Fact]
    public void GetAncestor_returns_self_when_depth_matches()
    {
        VariablesTable root = new VariablesTable();
        root.DefineVariable("marker", new Value(1));

        VariablesTable result = root.GetAncestor(1);

        Assert.Same(root, result);
    }

    [Fact]
    public void GetAncestor_returns_parent_at_depth_1_from_child()
    {
        VariablesTable root = new VariablesTable();
        root.DefineVariable("x", new Value(10));

        VariablesTable child = new VariablesTable(root);

        VariablesTable ancestor = child.GetAncestor(1);

        Assert.Equal(10, ancestor.GetVariable("x").AsInt());
    }

    [Fact]
    public void GetAncestor_returns_root_from_grandchild()
    {
        VariablesTable root = new VariablesTable();
        root.DefineVariable("root_val", new Value(42));

        VariablesTable child = new VariablesTable(root);
        VariablesTable grandchild = new VariablesTable(child);

        VariablesTable ancestor = grandchild.GetAncestor(1);

        Assert.Equal(42, ancestor.GetVariable("root_val").AsInt());
    }

    [Fact]
    public void GetAncestor_returns_middle_table_from_grandchild()
    {
        VariablesTable root = new VariablesTable();
        VariablesTable child = new VariablesTable(root);
        child.DefineVariable("mid", new Value(7));

        VariablesTable grandchild = new VariablesTable(child);

        VariablesTable ancestor = grandchild.GetAncestor(2);

        Assert.Equal(7, ancestor.GetVariable("mid").AsInt());
    }

    [Fact]
    public void GetAncestor_returns_grandchild_itself_at_its_own_depth()
    {
        VariablesTable root = new VariablesTable();
        VariablesTable child = new VariablesTable(root);
        VariablesTable grandchild = new VariablesTable(child);
        grandchild.DefineVariable("leaf", new Value(99));

        VariablesTable result = grandchild.GetAncestor(3);

        Assert.Equal(99, result.GetVariable("leaf").AsInt());
    }

    [Fact]
    public void GetAncestor_with_zero_depth_throws()
    {
        VariablesTable table = new VariablesTable();

        Assert.Throws<InvalidOperationException>(() => table.GetAncestor(0));
    }

    [Fact]
    public void GetAncestor_with_negative_depth_throws()
    {
        VariablesTable table = new VariablesTable();

        Assert.Throws<InvalidOperationException>(() => table.GetAncestor(-1));
    }

    [Fact]
    public void GetAncestor_with_depth_greater_than_chain_length_throws()
    {
        VariablesTable root = new VariablesTable();  // depth = 1
        VariablesTable child = new VariablesTable(root);  // depth = 2

        Assert.Throws<InvalidOperationException>(() => child.GetAncestor(3));
    }

    [Theory]
    [MemberData(nameof(GetAncestorData))]
    public void GetAncestor_at_each_depth_returns_correct_table(int targetDepth, string expectedVariable)
    {
        VariablesTable depth1 = new VariablesTable();
        depth1.DefineVariable("at_depth_1", new Value(1));

        VariablesTable depth2 = new VariablesTable(depth1);
        depth2.DefineVariable("at_depth_2", new Value(2));

        VariablesTable depth3 = new VariablesTable(depth2);
        depth3.DefineVariable("at_depth_3", new Value(3));

        VariablesTable ancestor = depth3.GetAncestor(targetDepth);

        Assert.Equal(targetDepth, ancestor.GetVariable(expectedVariable).AsInt());
    }

    public static TheoryData<int, string> GetAncestorData()
    {
        return new TheoryData<int, string>
        {
            { 1, "at_depth_1" },
            { 2, "at_depth_2" },
            { 3, "at_depth_3" },
        };
    }
}