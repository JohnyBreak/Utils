using System.Collections.Generic;
using NUnit.Framework;

namespace Bootstrap.Editor.BootstrapTests.Tests
{

    [SummerSettings(1)]
    public class Module1Test : IOrdered
    {
        private readonly Queue<int> _states;
        public int State;

        public Module1Test(Queue<int> states)
        {
            _states = states;
        }

        public void StartExecute()
        {
            State = _states.Dequeue();
        }
    }
    
    [SummerSettings(2)]
    public class Module2Test : IOrdered
    {
        private readonly Queue<int> _states;
        public int State;

        public Module2Test(Queue<int> states)
        {
            _states = states;
        }

        public void StartExecute()
        {
            State = _states.Dequeue();
        }
    }

    public class BootstrapTestScript
{
    // A Test behaves as an ordinary method
    [Test]
    public void BootstrapTestScriptSimplePasses()
    {
        // Use the Assert class to test conditions
        
        // arrange инициализация
        const int firstState = 456;
        const int secondState = 321;
        
        Queue<int> states = new Queue<int>();
        states.Enqueue(firstState);
        states.Enqueue(secondState);

        var module1 = new Module1Test(states);
        var module2 = new Module2Test(states);
        
        var list = new List<IOrdered>()
        {
            module1,
            module2
        };
        
        IInitializer initializer = new Initializer(list);
        var bootstrap = new Bootstrap(initializer);
        
        // act действие
        bootstrap.Start();
        
        // assert проверка

        Assert.AreEqual(firstState, module1.State);
        Assert.AreEqual(secondState, module2.State);
    }
}
}