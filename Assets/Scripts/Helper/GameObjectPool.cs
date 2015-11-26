using UnityEngine;
using System.Collections.Generic;

public class GameObjectPool
{
    private Stack<GameObject> stack;
    private GameObject objectTemplate;
    public GameObjectPool(GameObject prefab,  int count)
    {
        stack = new Stack<GameObject>(count);
        objectTemplate = prefab;

        for (int i = 0; i < count; i++)
        {
            var element = GameObject.Instantiate(prefab);
            element.SetActive(false);

            stack.Push(element);
        }

    }
    public GameObject Get()
    {
        if (stack.Count == 0)
        {
            var element = GameObject.Instantiate(objectTemplate);
            element.SetActive(false);
            
            stack.Push(element);

            Debug.LogWarning("Object Pool size changed for object " + objectTemplate);
        }

        return stack.Pop();
    }
    public void Return(GameObject  element)
    {
        stack.Push(element);
    }

}

public class ObjectPool<T> where T : MonoBehaviour
{
    private Stack<T> stack;
    private T objectTemplate;
    public ObjectPool(T obj, int count)
    {
        stack = new Stack<T>(count);
        objectTemplate = obj;

        for (int i = 0; i < count; i++)
        {
            var element = GameObject.Instantiate(obj.gameObject);
            element.SetActive(false);

            stack.Push(element.GetComponent<T>());
        }

    }
    public T Get()
    {
        if (stack.Count == 0)
        {
            var element = GameObject.Instantiate(objectTemplate.gameObject);
            element.SetActive(false);

            stack.Push(element.GetComponent<T>());

            Debug.LogWarning("Object Pool size changed for object " + objectTemplate);
        }

        return stack.Pop();
    }
    public void Return(T element)
    {
        stack.Push(element);
    }

}

//public interface IPoolElement
//{
//    IPoolElement Create();
//    void Return();
//    IPoolElement Get();
//}

//public class ObjectPool<T> where T : IPoolElement
//{
//    private Stack<T> stack;
//    private T objectTemplate;
//    public ObjectPool(T obj, int count)
//    {
//        stack = new Stack<T>(count);
//        objectTemplate = obj;

//        for (int i = 0; i < count; i++)
//        {

//            var element = obj.Create();
//            //stack.Push(element);
//        }

//    }
//    public T Get()
//    {
//        if (stack.Count == 0)
//        {
//            //var element = GameObject.Instantiate(objectTemplate.gameObject);
//            //element.SetActive(false);

//            stack.Push(element.GetComponent<T>());

//            Debug.LogWarning("Object Pool size changed for object " + objectTemplate);
//        }

//        return stack.Pop();
//    }
//    public void Return(T element)
//    {
//        stack.Push(element);
//    }

//}
