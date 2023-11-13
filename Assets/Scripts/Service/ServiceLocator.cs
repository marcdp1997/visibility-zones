using System;
using UnityEngine;
using System.Collections.Generic;

public class ServiceLocator
{
    public static ServiceLocator Instance => _instance ??= new ServiceLocator();
    private static ServiceLocator _instance;

    private readonly Dictionary<Type, object> _services;

    private ServiceLocator()
    {
        _services = new Dictionary<Type, object>();
    }

    public void RegisterService<T>(T service)
    {
        Type type = typeof(T);

        if (!_services.ContainsKey(type)) _services.Add(type, service);
        else Debug.Log($"Service {type} already registered");
    }

    public void UnregisterService<T>()
    {
        Type type = typeof(T);

        if (_services.ContainsKey(type)) _services.Remove(type);
        else Debug.Log($"Service {type} is not registered");
    }

    public T GetService<T>()
    {
        Type type = typeof(T);

        if (!_services.TryGetValue(type, out var service))
        {
            throw new Exception($"Service {type} not found");
        }

        return (T)service;
    }
}
