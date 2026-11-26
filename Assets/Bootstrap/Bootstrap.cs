using System.Collections;
using UnityEngine;

namespace Bootstrap
{
    public class Bootstrap
    {
        private readonly CoroutineRunner _coroutineRunner;
        private readonly IInitializer _initializer;

        public Bootstrap(CoroutineRunner coroutineRunner, IInitializer initializer)
        {
            _coroutineRunner = coroutineRunner;
            _initializer = initializer;
        }

        public void Start()
        {
            _coroutineRunner.StartCoroutine(_initializer.Start());
        }

        public IEnumerator GetEnumerator()
        {
            return _initializer.Start();
        }

        public bool IsComplete()
        {
            return _initializer.IsComplete();
        }

        // 2 проекта 

        // мультиплеер тавер дифенс, 2 команды,
        // 1) мультиплеер и мультиплеерный кор
        // 2) мета и синглплеерный кор

        // сингл игра 1 команда

        // на обоих проектах 3 сцены старт, мета, кор

        //zenject

        // проблема: инициализация в непонятном порядке, уйти от Awake/Start
        // при добавлении новых модулей падают налрефы, идет снежный ком на инициализацию старых модулей

        //кто-то в awake, у кого-то вызывается метод инициализации

        // фишки
        // запуск с любой сцены

        // под асинхрон
        // учесть соединение с сервером
        // скачка конфигов

        // 1 step configuration
        // 2 step initialization

        // order порядок инициализации

        // отдельные правила реагирования на неудачную инициализацию
        // hard fail exception
        // skip fail log

        // правила ожидания инициализации
        // кого-то ждем
        //кого-то вызвал и пошел дальше
        //обдумать как это может все сломать
    }
}