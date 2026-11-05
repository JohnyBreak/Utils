using System.Collections.Generic;
using NUnit.Framework;
// макс дорофеев
// этапы
// разные сцены
// запуск с любой сцены

namespace Bootstrap.Editor.BootstrapTests.Tests
{
    [SummerSettings(1)]
    public class Module1Test : IExecutableModule, IOrdered
    {
        private int _order;
        private readonly Queue<int> _states;
        public int State;

        public Module1Test(Queue<int> states)
        {
            _states = states;
        }

        public void Execute()
        {
            State = _states.Dequeue();
        }

        public int GetOrder()
        {
            return _order;
        }
    }

    [SummerSettings(2)]
    public class Module2Test : IExecutableModule, IOrdered
    {
        private int _order;
        private readonly Queue<int> _states;
        public int State;

        public Module2Test(Queue<int> states)
        {
            _states = states;
        }

        public void Execute()
        {
            State = _states.Dequeue();
        }

        public int GetOrder()
        {
            return _order;
        }
    }

    public class ModuleInterface1Test : IExecutableModule, IOrdered
    {
        private int _order;
        private readonly Queue<int> _states;
        public int State;

        public ModuleInterface1Test(Queue<int> states)
        {
            _states = states;
        }

        public void Execute()
        {
            State = _states.Dequeue();
        }

        public int GetOrder()
        {
            return _order;
        }
    }

    public class ModuleInterface2Test : IExecutableModule, IOrdered
    {
        private int _order;
        private readonly Queue<int> _states;
        public int State;

        public ModuleInterface2Test(Queue<int> states)
        {
            _states = states;
        }

        public void Execute()
        {
            State = _states.Dequeue();
        }

        public int GetOrder()
        {
            return _order;
        }
    }

    public class ModuleInterface3Test : IExecutableModule, IOrdered
    {
        private int _order;
        private readonly Queue<int> _states;
        public int State;

        public ModuleInterface3Test(Queue<int> states)
        {
            _states = states;
        }

        public void Execute()
        {
            State = _states.Dequeue();
        }

        public int GetOrder()
        {
            return _order;
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

            var list = new List<IExecutableModule>()
            {
                module1,
                module2
            };

            IInitializer initializer = new Initializer(list);
            var bootstrap = new Bootstrap(initializer);

            // act действие

            // assert проверка

            Assert.Throws<SummerModuleException>(() => bootstrap.Start());
        }

        [Test]
        public void BootstrapOrderInterface_Success()
        {
            // arrange инициализация
            const int firstState = 456;
            const int secondState = 321;
            const int thirdState = 999;

            Queue<int> states = new Queue<int>();
            states.Enqueue(firstState);
            states.Enqueue(secondState);
            states.Enqueue(thirdState);

            var module1 = new ModuleInterface1Test(states);
            var module2 = new ModuleInterface2Test(states);
            var module3 = new ModuleInterface3Test(states);

            var list = new List<IExecutableModule>()
            {
                module2,
                module3,
                module1
            };

            IInitializer initializer = new Initializer(list);
            var bootstrap = new Bootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
        }

        [SummerSettings(1)]
        public class ModuleAttribute1Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleAttribute1Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }

        [SummerSettings(2)]
        public class ModuleAttribute2Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleAttribute2Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }

        [SummerSettings(3)]
        public class ModuleAttribute3Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleAttribute3Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }

        [Test]
        public void BootstrapOrderAttribute_Success()
        {
            // arrange инициализация
            const int firstState = 456;
            const int secondState = 321;
            const int thirdState = 999;

            Queue<int> states = new Queue<int>();
            states.Enqueue(firstState);
            states.Enqueue(secondState);
            states.Enqueue(thirdState);

            var module1 = new ModuleAttribute1Test(states);
            var module2 = new ModuleAttribute2Test(states);
            var module3 = new ModuleAttribute3Test(states);

            var list = new List<IExecutableModule>()
            {
                module2,
                module3,
                module1
            };

            IInitializer initializer = new Initializer(list);
            var bootstrap = new Bootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
        }

        [SummerSettings(0)]
        public class ModuleOrderDuplicate1Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleOrderDuplicate1Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }
        
        [SummerSettings(0)]
        public class ModuleOrderDuplicate2Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleOrderDuplicate2Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }
        
        [Test]
        public void BootstrapOrderDuplicates_Exception()
        {
            // Use the Assert class to test conditions

            // arrange инициализация
            const int firstState = 456;
            const int secondState = 321;

            Queue<int> states = new Queue<int>();
            states.Enqueue(firstState);
            states.Enqueue(secondState);

            var module1 = new ModuleOrderDuplicate1Test(states);
            var module2 = new ModuleOrderDuplicate2Test(states);

            var list = new List<IExecutableModule>()
            {
                module2,
                module1,
            };

            IInitializer initializer = new Initializer(list);
            var bootstrap = new Bootstrap(initializer);

            // act действие

            // assert проверка

            Assert.Throws<SummerModuleException>(() => bootstrap.Start());
        }
        
        public class ModuleDuplicate1Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleDuplicate1Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }
        
        [Test]
        public void BootstrapModuleDuplicates_Exception()
        {
            // Use the Assert class to test conditions

            // arrange инициализация
            const int firstState = 456;
            const int secondState = 321;

            Queue<int> states = new Queue<int>();
            states.Enqueue(firstState);
            states.Enqueue(secondState);

            var module1 = new ModuleDuplicate1Test(states);
            var module2 = new ModuleDuplicate1Test(states);

            var list = new List<IExecutableModule>()
            {
                module2,
                module1,
            };

            IInitializer initializer = new Initializer(list);
            var bootstrap = new Bootstrap(initializer);

            // act действие

            // assert проверка

            Assert.Throws<SummerModuleException>(() => bootstrap.Start());
        }
        
        [SummerDuplicate]
        public class ModuleDuplicateAttributeTest : IExecutableModule, IOrdered
        {
            private int _order;

            public ModuleDuplicateAttributeTest(int order)
            {
                _order = order;
            }

            public void Execute()
            {
            }

            public int GetOrder()
            {
                return _order;
            }
        }
        
        [Test]
        public void BootstrapModuleDuplicateAttribute_Success()
        {
            // arrange инициализация

            var module1 = new ModuleDuplicateAttributeTest(1);
            var module2 = new ModuleDuplicateAttributeTest(2);

            var list = new List<IExecutableModule>()
            {
                module2,
                module1,
            };

            IInitializer initializer = new Initializer(list);
            var bootstrap = new Bootstrap(initializer);
            
            // act действие
            bootstrap.Start();
            
            // assert проверка

            Assert.True(true);
        }
    }
}