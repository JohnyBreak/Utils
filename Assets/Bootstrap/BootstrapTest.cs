using System;
using UnityEngine;

namespace Summer
{
    public class BootstrapTest : MonoBehaviour
    {
        [SerializeField] private Module2 _module2;

        private void Start()
        {
            
        }
    }

    class Module1
    {
        public Module1()
        {
            
        }
        
        public void Init()
        {
            
        }
    }
    
   
    
    class Module3
    {
        public Module3()
        {
            
        }
        
        public void Init()
        {
            
        }
    }

    // есть модули, 
    // монобех с созданием, конфигурацией и инициализацией модулей
    
    // сюда прикрутить бутстрап
}