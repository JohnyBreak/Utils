using Summer;
using UnityEngine;
using Zenject;

class Module2 : MonoBehaviour
{
    [Inject] private Module1 _module1;
}
