using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Summer
{
    public interface IBean
    {
        void Init();
    }

    public interface IInitializer
    {
        
    }
    
    public class Initializer
    {
        private readonly List<IBean> _beans;

        public Initializer(List<IBean> beans)
        {
            _beans = beans;
        }
    }
    
    public class Bootstrap : MonoBehaviour
    {
        [Inject] private Module1 _module1;
        [Inject] private Module2 _module2;
        [Inject] private Module3 _module3;
        
        private void Start()
        {
            if (_module1 == null)
            {
                Debug.LogError("_module1");
            }
            
            if (_module2 == null)
            {
                Debug.LogError("_module2");
            }
            
            if (_module3 == null)
            {
                Debug.LogError("_module3");
            }
            
            Debug.LogError("Working");
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