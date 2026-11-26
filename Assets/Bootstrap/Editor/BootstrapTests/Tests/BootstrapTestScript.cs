using System;
using System.Collections;
using System.Collections.Generic;
using Bootstrap.Exceptions;
using Bootstrap.Strategies;
using Bootstrap.Validation;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

// макс дорофеев
// этапы
// разные сцены
// запуск с любой сцены

namespace Bootstrap.Editor.BootstrapTests.Tests
{
    public class BootstrapTestScript
    {
        private CoroutineRunner _runner;

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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }

            public int GetOrder()
            {
                return 3;
            }
        }

        // A Test behaves as an ordinary method
        [UnityTest]
        public IEnumerator BootstrapTestScriptSimplePasses()
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
            var bootstrap = GetBootstrap(initializer);
ToggleLogIgnore(true);
            
            // act действие
            
            var routine = bootstrap.GetEnumerator();
            
            bool exceptionThrown = false;
            while (true)
            {
                object current;
                try
                {
                    if (!routine.MoveNext()) break;
                    current = routine.Current;
                }
                catch (SummerModuleException)
                {
                    exceptionThrown = true;
                    break;
                }
                
                yield return current; 
            }
            
            // assert проверка

            Assert.IsTrue(exceptionThrown, "Ожидалось исключение SummerModuleException, но его не было.");
            
            //Assert.Throws<SummerModuleException>(() => bootstrap.Start());
            ToggleLogIgnore(false);
            yield return null;
        }

        [UnityTest]
        public IEnumerator BootstrapOrderInterface_Success()
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
            var bootstrap = GetBootstrap(initializer);

            // act действие
            
            bootstrap.Start();
            
            // assert проверка

            yield return new WaitUntil(bootstrap.IsComplete);
            
            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
            yield return null;
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }
        }

        [UnityTest]
        public IEnumerator BootstrapOrderAttribute_Success()
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
            var bootstrap = GetBootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            yield return new WaitUntil(bootstrap.IsComplete);
            
            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
            yield return null;
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }
        }

        [UnityTest]
        public IEnumerator BootstrapOrderDuplicates_Exception()
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
            var bootstrap = GetBootstrap(initializer);
            
            ToggleLogIgnore(true);
            
            // act действие
            
            var routine = bootstrap.GetEnumerator();
            
            bool exceptionThrown = false;
            while (true)
            {
                object current;
                try
                {
                    if (!routine.MoveNext()) break;
                    current = routine.Current;
                }
                catch (SummerModuleException)
                {
                    exceptionThrown = true;
                    break;
                }
                
                yield return current; 
            }
            
            // assert проверка

            Assert.IsTrue(exceptionThrown, "Ожидалось исключение SummerModuleException, но его не было.");
            
            //Assert.Throws<SummerModuleException>(() => bootstrap.Start());
            ToggleLogIgnore(false);
            yield return null;
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }
        }

        [UnityTest]
        public IEnumerator BootstrapModuleDuplicates_Exception()
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
            var bootstrap = GetBootstrap(initializer);
            
            ToggleLogIgnore(true);
            
            // act действие
            
            var routine = bootstrap.GetEnumerator();
            
            bool exceptionThrown = false;
            while (true)
            {
                object current;
                try
                {
                    if (!routine.MoveNext()) break;
                    current = routine.Current;
                }
                catch (SummerModuleException)
                {
                    exceptionThrown = true;
                    break;
                }
                
                yield return current; 
            }
            
            // assert проверка

            Assert.IsTrue(exceptionThrown, "Ожидалось исключение SummerModuleException, но его не было.");
            
            //Assert.Throws<SummerModuleException>(() => bootstrap.Start());
            ToggleLogIgnore(false);
            yield return null;
        }

        [SummerDuplicate]
        public class ModuleDuplicateAttributeTest : IExecutableModule, ISummerModule
        {
            private int _order;

            public ModuleDuplicateAttributeTest(int order)
            {
                _order = order;
            }

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }

            public int GetOrder()
            {
                return _order;
            }
        }

        [UnityTest]
        public IEnumerator BootstrapModuleDuplicateAttribute_Success()
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

            var bootstrap = GetBootstrap(initializer);

            // act действие
            bootstrap.Start();

            // assert проверка

            Assert.True(true);
            yield return null;
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
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

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                return Promise.Success();
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }
        }

        [UnityTest]
        public IEnumerator Stage_Success()
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
            }, list);

            var bootstrap = GetBootstrap(initializer);
            
            // act действие
            
            bootstrap.Start();

            // assert проверка

            yield return new WaitUntil(bootstrap.IsComplete);
            
            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
            yield return null;
        }
        
        [SummerSettings(typeof(ThirdStage), 1)]
        public class AttributeAsyncModule1 : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            private readonly CoroutineRunner _runner;
            private readonly float _delaySeconds;
            public int State;
            private Promise _promise;
            public AttributeAsyncModule1(Queue<int> states, CoroutineRunner runner, float delaySeconds)
            {
                _states = states;
                _runner = runner;
                _delaySeconds = delaySeconds;
            }

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                _promise = new Promise();
                _runner.StartCoroutine(Delay());
                return _promise;
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }

            private IEnumerator Delay()
            {
                if (_delaySeconds > 0)
                {
                    yield return new WaitForSeconds(_delaySeconds);
                }
                
                _promise.Complete(SummerStatus.Success);
            }
        }
        
        [SummerSettings(typeof(ThirdStage), 2)]
        public class AttributeAsyncModule2 : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            private readonly CoroutineRunner _runner;
            private readonly float _delaySeconds;
            public int State;
            private Promise _promise;
            public AttributeAsyncModule2(Queue<int> states, CoroutineRunner runner, float delaySeconds)
            {
                _states = states;
                _runner = runner;
                _delaySeconds = delaySeconds;
            }

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                _promise = new Promise();
                _runner.StartCoroutine(Delay());
                return _promise;
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }

            private IEnumerator Delay()
            {
                if (_delaySeconds > 0)
                {
                    yield return new WaitForSeconds(_delaySeconds);
                }
                
                _promise.Complete(SummerStatus.Success);
            }
        }
        
        [SummerSettings(typeof(ThirdStage), 3, new []{typeof(AttributeAsyncModule1), typeof(AttributeAsyncModule2)})]
        public class AttributeAsyncModule3 : IExecutableModule
        {
            private int _order;
            private readonly Queue<int> _states;
            private readonly CoroutineRunner _runner;
            private readonly float _delaySeconds;
            public int State;
            private Promise _promise;
            public AttributeAsyncModule3(Queue<int> states, CoroutineRunner runner, float delaySeconds)
            {
                _states = states;
                _runner = runner;
                _delaySeconds = delaySeconds;
            }

            public Promise PreExecute()
            {
                return Promise.Success();
            }

            public Promise Execute()
            {
                State = _states.Dequeue();
                _promise = new Promise();
                _runner.StartCoroutine(Delay());
                return _promise;
            }

            public Promise PostExecute()
            {
                return Promise.Success();
            }

            private IEnumerator Delay()
            {
                if (_delaySeconds > 0)
                {
                    yield return new WaitForSeconds(_delaySeconds);
                }
                
                _promise.Complete(SummerStatus.Success);
            }
        }
        
        [UnityTest]
        public IEnumerator AsyncModuleInitialization_Success()
        {
            // arrange инициализация
            const int firstState = 456;
            const int secondState = 321;
            const int thirdState = 999;

            Queue<int> states = new Queue<int>();
            states.Enqueue(firstState);
            states.Enqueue(secondState);
            states.Enqueue(thirdState);

            var module1 = new AttributeAsyncModule1(states, GetCoroutineRunner(), 1);
            var module2 = new AttributeAsyncModule2(states, GetCoroutineRunner(), 2);
            var module3 = new AttributeAsyncModule3(states, GetCoroutineRunner(), 0);

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
            }, list);

            var bootstrap = GetBootstrap(initializer);
            
            // act действие
            
            bootstrap.Start();

            // assert проверка

            yield return new WaitUntil(bootstrap.IsComplete);
            
            Assert.AreEqual(firstState, module1.State);
            Assert.AreEqual(secondState, module2.State);
            Assert.AreEqual(thirdState, module3.State);
            yield return null;
        }
        
        private Bootstrap GetBootstrap(IInitializer initializer)
        {
            var bootstrap = new Bootstrap(GetCoroutineRunner(), initializer);
            return bootstrap;
        }

        private CoroutineRunner GetCoroutineRunner()
        {
            if (_runner == null)
            {
                var go = new GameObject("TestRunner");
                _runner = go.AddComponent<CoroutineRunner>();
            }

            return _runner;
        }

        private IInitializer CreateInitializer(List<Type> stages, List<IExecutableModule> modules)
        {
            IInitializer initializer = new Initializer(
                stages,
                modules);

            return initializer;
        }
        
        private void ToggleLogIgnore(bool ignore)
        {
            LogAssert.ignoreFailingMessages = ignore;
        }
    }
}