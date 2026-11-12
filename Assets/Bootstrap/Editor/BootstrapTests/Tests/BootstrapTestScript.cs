using System;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Strategies;
using Bootstrap.Validation;
using NUnit.Framework;

// макс дорофеев
// этапы
// разные сцены
// запуск с любой сцены

namespace Bootstrap.Editor.BootstrapTests.Tests
{
    public class BootstrapTestScript
    {
        [SummerSettings(typeof(MainStage), 1)]
        public class Module1Test : IExecutableModule, ISummerModule
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

        [SummerSettings(typeof(MainStage), 2)]
        public class Module2Test : IExecutableModule, ISummerModule
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

        public class ModuleInterface1Test : IExecutableModule, ISummerModule
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
                return 1;
            }
        }

        public class ModuleInterface2Test : IExecutableModule, ISummerModule
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
                return 2;
            }
        }

        public class ModuleInterface3Test : IExecutableModule, ISummerModule
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
                return 3;
            }
        }

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

            IInitializer initializer = CreateInitializer(new List<Type>() { typeof(MainStage) }, list);
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

            IInitializer initializer = CreateInitializer(new List<Type>() { typeof(MainStage) }, list);
            var bootstrap = new Bootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
        }

        [SummerSettings(typeof(MainStage), 1)]
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

        [SummerSettings(typeof(MainStage), 2)]
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

        [SummerSettings(typeof(MainStage), 3)]
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

            IInitializer initializer = CreateInitializer(new List<Type>() { typeof(MainStage) }, list);
            var bootstrap = new Bootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
        }

        [SummerSettings(typeof(MainStage), 0)]
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

        [SummerSettings(typeof(MainStage), 0)]
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

            IInitializer initializer = CreateInitializer(new List<Type>() { typeof(MainStage) }, list);
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

            IInitializer initializer = CreateInitializer(new List<Type>() { typeof(MainStage) }, list);
            var bootstrap = new Bootstrap(initializer);

            // act действие

            // assert проверка

            Assert.Throws<SummerModuleException>(() => bootstrap.Start());
        }

        [SummerDuplicate]
        public class ModuleDuplicateAttributeTest : IExecutableModule, ISummerModule
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

            IInitializer initializer = CreateInitializer(new List<Type>() { typeof(MainStage) }, list);
            
            var bootstrap = new Bootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            Assert.True(true);
        }

        public class FirstStage
        {
        }

        public class SecondStage
        {
        }

        public class ThirdStage
        {
        }

        [SummerSettings(typeof(FirstStage), 1)]
        public class ModuleStage1Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleStage1Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }

        [SummerSettings(typeof(SecondStage), 2)]
        public class ModuleStage2Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleStage2Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }

        [SummerSettings(typeof(ThirdStage), 3)]
        public class ModuleStage3Test : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            public int State;

            public ModuleStage3Test(Queue<int> states)
            {
                _states = states;
            }

            public void Execute()
            {
                State = _states.Dequeue();
            }
        }

        [Test]
        public void Stage_Success()
        {
            // arrange инициализация
            const int firstState = 456;
            const int secondState = 321;
            const int thirdState = 999;

            Queue<int> states = new Queue<int>();
            states.Enqueue(firstState);
            states.Enqueue(secondState);
            states.Enqueue(thirdState);

            var module1 = new ModuleStage1Test(states);
            var module2 = new ModuleStage2Test(states);
            var module3 = new ModuleStage3Test(states);

            var list = new List<IExecutableModule>()
            {
                module2,
                module3,
                module1
            };


            IInitializer initializer = CreateInitializer(new List<Type>()
            {
                typeof(FirstStage),
                typeof(SecondStage),
                typeof(ThirdStage)
            },list);
            var bootstrap = new Bootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
        }
        
        private IInitializer CreateInitializer(List<Type> stages, List<IExecutableModule> modules)
        {
            var map = GetStagesMap(stages);
            
            var infoGetter = CreateSummerInfoGetter();
            
            var validator = new SummerValidator(infoGetter);
            
            IInitializer initializer = new Initializer(
                map,
                modules,
                infoGetter,
                infoGetter,
                validator);

            return initializer;
            
            Dictionary<Type, int> GetStagesMap(List<Type> list)
            {
                Dictionary<Type, int> stages = new Dictionary<Type, int>();

                for (int i = 0; i < list.Count; i++)
                {
                    stages[list[i]] = i;
                }

                return stages;
            }
            
            SummerInfoGetter CreateSummerInfoGetter()
            {
                var result = new SummerInfoGetter(
                    new List<IGetStageIndexStrategy>()
                {
                    new InterfaceStageIndexGetter(GetStageIndex),
                    new AttributeStageIndexGetter(GetStageIndex),
                },
                new List<IGetOrderStrategy>()
                {
                    new InterfaceOrderGetter(),
                    new AttributeOrderGetter(),
                });

                return result;
            }
            
            int GetStageIndex(object obj, Type type)
            {
                if (type == null)
                {
                    throw new SummerModuleException($"У модуля {obj.GetType()} null стадия");
                }

                if (!map.TryGetValue(type, out var index))
                {
                    throw new SummerModuleException($"У модуля {obj.GetType()} не зарегистрированная стадия {type}");
                }

                return index;
            }
        }
    }
}